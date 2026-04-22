using System;
using System.IO;
using GameConsole.CommandTree;
using UltraDirector.CameraLogic.Extensions;
using UltraDirector.Utils;
using UnityEngine;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand
{
    private Branch Debug => 
        Branch("debug",
            Branch("prefs",
                Leaf<string>("setSpawnKey", SetSpawnCameraKey),
                Leaf("listKeyCodes", ListKeyCodes)
            ),
            Branch("recordTests",
                Leaf("do5", () => RunRecordTests(5)),
                Leaf<float>("do", RunRecordTests))
        );

    private void RunRecordTests(float secs)
    {
        Log.Info("Running record tests");
        SpawnCamera("RecordTestsCamera");
        if (!TryGetCamera("RecordTestsCamera", out var cam))
        {
            Log.Error("Failed to spawn test camera. Aborting record tests.");
            return;
        }

        cam.AddCameraRenderGizmo();
        Log.Info("Added camera render gizmo");
        cam.OnUpdate += camera =>
        {
            camera.transform.LookAt(NewMovement.Instance!.transform);
        };
        Log.Info("Added update");
        try
        {
            cam.StartRecording(Path.Combine(Plugin.Directory, "record_test_output.mkv"));
            Log.Info($"Recording for {secs} seconds...");
            cam.StartCoroutine(CoroutineHelper.DoAfterDelay(secs, cam.StopRecording));
        }
        catch (Exception e)
        {
            Log.Error(e.Message, stackTrace: e.StackTrace);
            throw;
        }
    }

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