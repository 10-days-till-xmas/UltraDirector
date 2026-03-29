using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace UltraDirector.Scripting.CSharp;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(UltraDirector.MyPluginInfo.PLUGIN_GUID)]
public sealed class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger { get; private set; } = null!;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        gameObject.hideFlags = HideFlags.DontSaveInEditor;
        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
        // TODO: Allow multi-line input (shift+enter to add line, enter to run)
        // TODO: Implement windows for showing camera output, and ffmpeg for recording it (for the core)
    }
}