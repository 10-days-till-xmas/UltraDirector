global using static UltraDirector.Plugin.LogHelper;
using System;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using FFMediaToolkit;
using HarmonyLib;
using HarmonyLib.Tools;
using UltraDirector.LogUtils;
using UltraDirector.Utils;
using UnityEngine;

namespace UltraDirector;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public sealed partial class Plugin : BaseUnityPlugin
{
    private new static ManualLogSource Logger { get; set; } = null!;
    internal static readonly string Directory =
        System.IO.Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName;
    internal static BepinexPlogHandler CreateBepinexPlogHandler() => new(Logger);
    private Task? installTask;
    private void Awake()
    {
        Logger = base.Logger;
        gameObject.hideFlags = HideFlags.DontSaveInEditor;

        FFmpegLoader.FFmpegPath = FFMpegInstaller.FFMpegBinDir;
        if (FFMpegInstaller.CheckPresence())
        {
            LogInfo("FFMpeg binaries found.");
            installTask = Task.CompletedTask;
        }
        else
        {
            LogWarning("FFMpeg binaries not found, installing");
            installTask = FFMpegInstaller.InstallAsync();
        }
        FFmpegLoader.LogCallback += LogInfo;
        DoPatching();
        LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private async void Update()
    {
        try
        {
            if (installTask is null) return;
            await installTask;
            FFmpegLoader.LoadFFmpeg();
            LogInfo("FFmpeg binaries loaded successfully!");
        }
        catch (Exception e)
        {
            LogError(e);
        }
        finally
        {
            installTask = null;
        }
    }

    private static void DoPatching()
    {
        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        HarmonyFileLog.Enabled = true;
        harmony.PatchAll();
        LogInfo("Patching completed successfully!");
    }
}