using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using plog;

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
        // 0: postfix
        // 1: ctor
        // 2: ctor caller
        var frame = new StackFrame(2); // if you transpile this instead you can use 1
        var caller = frame.GetMethod();
        if (!(caller.DeclaringType?.Namespace?.Contains("UltraDirector") ?? false)) return;
        __instance.AddHandler(Plugin.CreateBepinexPlogHandler());
    }
}