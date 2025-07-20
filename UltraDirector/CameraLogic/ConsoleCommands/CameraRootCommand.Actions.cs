using System;
using System.Diagnostics.CodeAnalysis;
using UltraDirector.CameraLogic.Extensions;
using UnityEngine;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand
{
    private bool TryGetCamera(string name, [NotNullWhen(true)] out UKCamera? camera)
    {
        if (MonoSingleton<CameraManager>.Instance.Cameras.TryGetValue(name, out camera)) 
            return true;
        Log.Warning($"Camera '{name}' not found.");
        return false;
    }

    private void RemoveCamera(string name)
    {
        CameraManager.Instance.RemoveCamera(name);
        Log.Info($"Camera '{name}' removed successfully.");
    }

    private void SpawnCamera(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            name ??= $"ukcam_{System.IO.Path.GetRandomFileName()}";

        var nm = NewMovement.Instance;
        var playerPos = nm.transform.position + new Vector3(0, 1.5f, 0);
        var playerRot = Quaternion.Euler(nm.transform.eulerAngles);

        if (CameraManager.Instance.TrySpawnCamera(name, playerPos, playerRot, out _))
        {
            Log.Info($"Created camera: {name} at position {playerPos} with rotation {playerRot.eulerAngles}");
        }
        else
        {
            Log.Warning($"Camera {name} already exists!");
        }
    }

    private void ListCameras()
    {
        var cameras = MonoSingleton<CameraManager>.Instance.Cameras;
        if (cameras.Count == 0)
        {
            Log.Warning("No cameras found in the scene.");
            return;
        }

        Log.Info("Cameras in the scene:");
        foreach (var (name, camera) in cameras)
        {
            Log.Info($"- {name}: {camera}");
        }
    }
    
    private void SetSpawnCameraKey(string spawnKey)
    {
        if (Enum.TryParse<KeyCode>(spawnKey, out var keyCode) &&
            Enum.IsDefined(typeof(KeyCode), keyCode))
        {
            CameraManager.Instance.SpawnCameraKey = keyCode;
            Log.Info($"Spawn camera key set to: {keyCode}");
        }
        else
        {
            Log.Warning($"Invalid key code: {spawnKey}. Please use a valid KeyCode.");
        }

    }

    private void ListKeyCodes()
    {
        var msg = "Available KeyCodes: " + string.Join(", ", Enum.GetNames(typeof(KeyCode)));
        Log.Info(msg);
    }
    
    private void GetCameraSummary(string name)
    {
        if (TryGetCamera(name, out var camera))
            Log.Info($"Camera '{name}' details: {camera}");
    }

    private void GetCameraDepth(string name)
    {
        if (!TryGetCamera(name, out var camera)) return;
        var depth = camera.UnityCamera.depth;
        Log.Info($"Camera \"{name}\" depth: {depth}");
    }
    private void SetCameraDepth(string name, float value)
    {
        if (!TryGetCamera(name, out var camera)) return;
        camera.UnityCamera.depth = value;
        Log.Info($"Camera \"{name}\" depth set to: {value}");
    }

    private void GetCameraRectSize(string name)
    {
        if (!TryGetCamera(name, out var camera)) return;
        var rectSize = camera.UnityCamera.rect.size;
        Log.Info($"Camera \"{name}\" rect size: ({rectSize.x}, {rectSize.y})");
    }
    private void SetCameraRectSize(string name, float width, float height)
    {
        if (!TryGetCamera(name, out var camera)) return;
        camera.SetRectSize(width, height);
        Log.Info($"Camera \"{name}\" rect size set to: {width}, {height}");
    }

    private void GetCameraRectPosition(string name)
    {
        if (!TryGetCamera(name, out var camera)) return;
        var rectPos = camera.UnityCamera.rect.position;
        Log.Info($"Camera \"{name}\" rect position: ({rectPos.x}, {rectPos.y})");
    }
    
    private void SetCameraRectPosition(string name, float x, float y)
    {
        if (!TryGetCamera(name, out var camera)) return;
        camera.SetRectPosition(x, y);
        Log.Info($"Camera \"{name}\" rect position set to: ({x}, {y})");
    }
}