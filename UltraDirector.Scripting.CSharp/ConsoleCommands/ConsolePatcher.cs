using System.Collections.Generic;
using HarmonyLib;
using GameConsole;
using UltraDirector.Scripting.CSharp.ReplManagement;

namespace UltraDirector.Scripting.CSharp.ConsoleCommands;

[HarmonyPatch(typeof(Console))]
public class ConsolePatcher
{
    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    private static void AddCheatCodes(Console __instance)
    {
        __instance.RegisterCommand(new CameraControlCommand(__instance));

        Plugin.Logger.LogInfo("Added camera control command");
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Console.ProcessInput))]
    private static bool CSharpEditMode(Console __instance, string text)
    {
        if (!CameraReplManager.Instance!.IsActive) return true;
        Plugin.Logger.LogInfo("Running C# REPL in Console.ProcessInput");
        Plugin.Logger.LogInfo($"text = {text}");

        var cm = CameraReplManager.Instance;
        if (text is "Exit" or "exit" or "quit" or "Quit")
        {
            cm.Log.Info("Exiting C# REPL");
            Plugin.Logger.LogInfo("Exiting C# REPL!");
            CameraReplManager.Instance.Deactivate();
        }
        else
        {
            Plugin.Logger.LogInfo($"Evaluating expression in C# REPL: {text}");
            cm.EvaluateExpression(text);
        }
        return false;
    }

    private static readonly AccessTools.FieldRef<Console, List<string>> suggestionsRef =
        AccessTools.FieldRefAccess<Console, List<string>>("suggestions");

    [HarmonyPrefix]
    [HarmonyPatch("FindSuggestions")]
    private static bool DisableSuggestions(Console __instance)
    {
        if (!CameraReplManager.Instance!.IsActive) return true;
        suggestionsRef(__instance).Clear();
        return false;
    }
}