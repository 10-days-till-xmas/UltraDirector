using System;
using UnityEngine;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand
{
    private void SetSpawnCameraKey(string spawnKey)
    {
        if (Enum.TryParse<KeyCode>(spawnKey, out var keyCode) &&
            Enum.IsDefined(typeof(KeyCode), keyCode))
        {
            CameraManager.Instance!.SpawnCameraKey = keyCode;
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
}