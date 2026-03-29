using System;
using HarmonyLib;

namespace UltraDirector.Scripting.CSharp.Patchers;
[HarmonyPatch(typeof(CheckPoint))]
public static class CheckpointPatcher
{ // TODO: just replace with a hook
    public static event Action<CheckPoint>? OnCheckpointReset = null;

    [HarmonyPostfix]
    [HarmonyPatch("OnRespawn")]
    public static void HookCheckpointReset(CheckPoint __instance)
    {
        OnCheckpointReset?.Invoke(__instance);
    }
}