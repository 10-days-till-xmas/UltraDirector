using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using UltraDirector.Scripting.CSharp.Patchers;
using UnityEngine.SceneManagement;

namespace UltraDirector.Scripting.CSharp.AutoScripter;
[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public sealed class AutoScripter : MonoSingleton<AutoScripter>
{
    private class ScriptFile
    {
        public string FilePath { get; private set; }
        private readonly FileSystemWatcher _watcher;
        public CoroutineScript CoroutineScript { get; private set; } = null!;
        public AutoScriptTrigger[] Triggers { get; set; } = [];
        private readonly object _lock = new();

        public ScriptFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException($"Script file not found: {filePath}");
            FilePath = filePath;
            _watcher = new FileSystemWatcher(Directory.GetDirectoryRoot(filePath), Path.GetFileName(filePath)); // renames are messy to handle
            _watcher.IncludeSubdirectories = false;
            _watcher.Changed += (_, args) =>
            {
                if (args.ChangeType == WatcherChangeTypes.Changed) Recompile();
            };
            Recompile();
        }

        public void Recompile()
        {
            lock (_lock)
            {
                CoroutineScript = CoroutineScript.CreateAsync(FilePath).GetAwaiter().GetResult();
            }
        }
    }
    // TODO: let scripts be loadable from a config
    private List<ScriptFile> _scripts = [];

    private Action<CheckPoint> _onCheckpointLoaded = null!;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _onCheckpointLoaded = c => OnCheckpointLoaded(c.name);
        CheckpointPatcher.OnCheckpointReset += _onCheckpointLoaded;
        // TODO: load script paths from config
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        CheckpointPatcher.OnCheckpointReset -= _onCheckpointLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) =>
        _scripts.Where(static path => path.Triggers.Any(static t => t.OnSceneLoad))
                .Do(path => StartCoroutine(path.CoroutineScript.GetNewEnumerator()));

    private void OnCheckpointLoaded(string checkpointName) =>
        _scripts.Where(scriptFile => scriptFile.Triggers
                                               .OfType<OnCheckpointLoadAutoScriptTrigger>()
                                               .Any(t => t.CheckpointName == checkpointName))
                .Do(scriptFile => StartCoroutine(scriptFile.CoroutineScript.GetNewEnumerator()));
}