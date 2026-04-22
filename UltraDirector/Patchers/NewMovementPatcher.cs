using System.IO;
using HarmonyLib;
using UltraDirector.CameraLogic;
using UnityEngine;

namespace UltraDirector.Patchers;

[HarmonyPatch(typeof(NewMovement))]
public class NewMovementPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch("Update")]
    public static void UpdatePrefix(NewMovement __instance)
    {
        SpawnCameraIfKeyPressed(__instance);
    }

    private static void SpawnCameraIfKeyPressed(NewMovement __instance)
    {
        if (!Input.GetKeyDown(CameraManager.Instance!.SpawnCameraKey)) return;

        LogInfo("Spawning player camera...");
        var playerPos = __instance.transform.position + new Vector3(0, 1.5f, 0);
        var name = $"ukcam_{Path.GetRandomFileName()}";
        LogInfo(CameraManager.Instance.TrySpawnCamera(name, playerPos, __instance.transform.rotation, out var c)
            ? $"Created camera: {name} at position {playerPos} with rotation {c.transform.rotation}"
            : "Camera already exists!");
    }
}