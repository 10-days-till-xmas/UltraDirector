using System.Diagnostics.CodeAnalysis;
using System.IO;
using UltraDirector.LogUtils;
using UltraDirector.Utils;
using UnityEngine;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand
{
    private bool TryGetCamera(string name, [NotNullWhen(true)] out UKCamera? camera)
    {
        if (MonoSingleton<CameraManager>.Instance!.Cameras.TryGetValue(name, out camera))
            return true;
        Log.LogWarning($"Camera '{name}' not found.");
        return false;
    }
    private UKCamera? TryGetCamera(string name)
    {
        if (MonoSingleton<CameraManager>.Instance!.Cameras.TryGetValue(name, out var camera))
            return camera;
        Log.LogWarning($"Camera '{name}' not found.");
        return null;
    }

    private void RemoveCamera(string name)
    {
        CameraManager.Instance!.RemoveCamera(name);
        Log.LogInfo($"Camera '{name}' removed successfully.");
    }

    private void SpawnCamera(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            name ??= $"ukcam_{Path.GetRandomFileName()}";

        var nm = NewMovement.Instance!;
        var playerPos = nm.transform.position + new Vector3(0, 1.5f, 0);
        var playerRot = Quaternion.Euler(nm.transform.eulerAngles);

        if (CameraManager.Instance!.TrySpawnCamera(name, playerPos, playerRot, out _))
            Log.Info($"Created camera: {name} at position {playerPos} with rotation {playerRot.eulerAngles}");
        else
            Log.Warning($"Camera {name} already exists!");
    }

    private void ListCameras()
    {
        var cameras = MonoSingleton<CameraManager>.Instance!.Cameras;
        if (cameras.Count == 0)
        {
            Log.LogWarning("No cameras found in the scene.");
            return;
        }

        Log.Info("Cameras in the scene:");
        foreach (var (name, camera) in cameras)
        {
            Log.LogInfo($"- {name}: {camera}");
        }
    }

    private void StartRecording(string name) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            var path = Path.Combine(Plugin.Directory, $"{name}_recording.mkv");
            camera.StartRecording(path);
            Log.Info($"Started recording on camera '{name}' to path: {path}");
        });

    private void StartRecordingWithTime(string name, float secs) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            var path = Path.Combine(Plugin.Directory, $"{name}_recording.mkv");
            camera.StartRecording(path);
            Log.Info($"Started recording on camera '{name}' to path: {path}");
            camera.StartCoroutine(CoroutineHelper.DoAfterDelay(5, camera.StopRecording)); // Record for 10 seconds
        });

    private void StopRecording(string name) => TryGetCamera(name)
       .IfNotNullDo(camera => camera.StopRecording());

    private static void RemuxVideo(string filePath) => Remux.ToMp4(filePath);
}