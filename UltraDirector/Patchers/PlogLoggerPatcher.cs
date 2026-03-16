using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using plog;
using UltraDirector.LogUtils;

namespace UltraDirector.Patchers;

[HarmonyPatch]
public static class PlogLoggerPatcher
{
    [UsedImplicitly]
    [HarmonyTargetMethods]
    public static IEnumerable<MethodBase> FindConstructors() => typeof(Logger).GetConstructors();

    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Postfix(Logger __instance)
    {
        var frame = new StackFrame(1); // caller of the constructor
        var caller = frame.GetMethod();
        Plugin.Logger.LogInfo($"Called from {caller.DeclaringType?.FullName}.{caller.Name}");
        if (!(caller.DeclaringType?.Namespace?.Contains("UltraDirector") ?? false)) return;
        __instance.AddHandler(new BepinexPlogHandler(Plugin.Logger));
        Plugin.Logger.LogInfo($"Added Handler");
    }
}