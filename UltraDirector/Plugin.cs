global using static UltraDirector.Utils.LogHelper;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using UltraDirector.CameraLogic.ConsoleCommands;
using UnityEngine;
[assembly: InternalsVisibleTo("UltraDirector.Scripting.CSharp")]
namespace UltraDirector;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public sealed class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger { get; private set; } = null!;
    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        gameObject.hideFlags = HideFlags.DontSaveInEditor;
        DoPatching();
    }

    private static void DoPatching()
    {
        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        HarmonyFileLog.Enabled = true;
        harmony.PatchAll(typeof(ConsolePatcher));
        Log("Patching completed successfully!");
    }
}