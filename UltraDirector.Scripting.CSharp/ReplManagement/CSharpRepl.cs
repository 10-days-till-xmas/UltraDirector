using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using UltraDirector.LogUtils;
using UltraDirector.Scripting.CSharp.Globals;
using UnityEngine;
using Logger = plog.Logger;
namespace UltraDirector.Scripting.CSharp.ReplManagement;

public sealed class CSharpRepl : MonoBehaviour
{
    private Logger Log { get; } = new(nameof(CSharpRepl));
    private ScriptState? _scriptState;
    private readonly Queue<string> _scriptRunQueue = [];
    public event Action? OnReplExit = null;
    private static readonly ScriptOptions ScriptOptions = // maybe add common usings
        ScriptOptions.Default.WithReferences(
            AppDomain.CurrentDomain
                     .GetAssemblies()
                     .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location)))
                     .AddImports("System", "System.Linq", "System.Collections.Generic", "UnityEngine");

    private void Awake()
    {
        DontDestroyOnLoad(this);
        gameObject.hideFlags |= HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
        Log.RegisterToConsole();
    }
    public void RunLine(string line)
    {
        if (!line.Equals("exit", StringComparison.OrdinalIgnoreCase))
            _scriptRunQueue.Enqueue(line);
        else
            OnReplExit?.Invoke();
    }

    private async Task ActuallyRunLine(string line)
    {
        _scriptState = _scriptState is null
                           ? await CSharpScript.RunAsync(line,
                                 ScriptOptions,
                                 globals: new ReplGlobals(Log))
                           : await _scriptState.ContinueWithAsync(line, ScriptOptions);
    }

    private bool running = false;
    // ReSharper disable once AsyncVoidMethod
    private async void Update()
    {
        if (running) return;
        if (!_scriptRunQueue.TryDequeue(out var line)) return;
        Plugin.Logger.LogInfo($"Running {line}");
        Log.Info("> " + line);
        running = true;
        try
        {
            await ActuallyRunLine(line!);
            Log.Info("=> " + (_scriptState!.ReturnValue ?? "null"));
            Plugin.Logger.LogInfo($"Return value: {_scriptState.ReturnValue}");
        }
        catch (TaskCanceledException)
        {
            Log.Warning("Script execution was canceled.");
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
        }
        running = false;
    }

    public void Reset()
    {
        _scriptState = null;
        _scriptRunQueue.Clear();
        OnReplExit = null;
    }

    public Compilation CompileAll()
    {
        var sb = new StringBuilder();
        Stack<string> stack = new();
        var current = _scriptState?.Script;
        while (current is not null)
        {
            stack.Push(current.Code);
            current = current.Previous;
        }

        while (stack.TryPop(out var str)) sb.Append(str);
        var script = CSharpScript.Create(sb.ToString(), ScriptOptions);
        return script.GetCompilation();
    }
}