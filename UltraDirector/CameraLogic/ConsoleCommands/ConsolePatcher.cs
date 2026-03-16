using System.Collections.Generic;
using System.Reflection.Emit;
using GameConsole;
using HarmonyLib;

namespace UltraDirector.CameraLogic.ConsoleCommands;
// TODO: add scripting extensions for advanced control over the cameras
[HarmonyPatch(typeof(Console))]
public sealed class ConsolePatcher
{
    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    private static void RegisterCommands(Console __instance)
    {
        __instance.RegisterCommand(new CameraRootCommand(__instance));

        Plugin.Logger.LogInfo("Added camera root command");
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Console.Parse))]
    private static bool ParseImproved(string text, ref string[] __result)
    {
        __result = CommandLineParser.Parse(text);
        return false; // i cba to transpile
    }

    [HarmonyTranspiler]
    [HarmonyPatch("FindSuggestions")]
    private static IEnumerable<CodeInstruction> TranspileFindSuggestions(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var customParseMethod = AccessTools.Method(typeof(CommandLineParser), nameof(CommandLineParser.Parse));
        // IL_0020: ret
        // // string[] array = Parse(value);
        // IL_0021: ldarg.0
        // IL_0022: ldarg.1
        // IL_0023: call instance string[] GameConsole.Console::Parse(string)
        // IL_0028: stloc.0
        var codeMatcher = new CodeMatcher(instructions, generator);
        codeMatcher.Start()
            .MatchForward(false,
                new CodeMatch(OpCodes.Ret),
                new CodeMatch(OpCodes.Ldarg_0), // instance of Console
                new CodeMatch(OpCodes.Ldarg_1), // string value for Parse
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Console), nameof(Console.Parse))),
                new CodeMatch(OpCodes.Stloc_0))
            .Advance(1); // Move to the ldarg.0 instruction
        var labels = codeMatcher.Labels;
        codeMatcher
            .RemoveInstructions(3)
            .Insert(new CodeInstruction(OpCodes.Ldarg_1)) // Load the string argument for Parse
            .AddLabels(labels)
            .Advance(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldc_I4_0), // isComplete = false
                new CodeInstruction(OpCodes.Call, customParseMethod)
                );

        return codeMatcher.InstructionEnumeration();
    }
}