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
        if (!Input.GetKeyDown(CameraManager.Instance.SpawnCameraKey)) return;
        
        Log("Spawning player camera...");
        var playerPos = __instance.transform.position + new Vector3(0, 1.5f, 0);
        var playerRot = Quaternion.Euler(__instance.transform.eulerAngles);
        var name = $"ukcam_{System.IO.Path.GetRandomFileName()}";
        Log(CameraManager.Instance.TrySpawnCamera(name, playerPos, playerRot, out _)
            ? $"Created camera: {name} at position {playerPos} with rotation {playerRot.eulerAngles}"
            : "Camera already exists!");
    }
}