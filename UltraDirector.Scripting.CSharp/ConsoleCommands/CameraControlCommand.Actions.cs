using UltraDirector.CameraLogic;
using UltraDirector.Scripting.CSharp.ReplManagement;
using UltraDirector.Scripting.CSharp.Utils;

namespace UltraDirector.Scripting.CSharp.ConsoleCommands;

public sealed partial class CameraControlCommand
{
    private void ActivateRepl()
    {
        Log.Info("Entering edit mode");
        Plugin.Logger.LogInfo("Entering edit mode");
        CameraReplManager.Instance!.Activate();
    }

    private void SelectCamera(string cameraName)
    {
        if (string.IsNullOrEmpty(cameraName))
        {
            Log.Error("No camera name provided.");
            return;
        }

        Plugin.Logger.LogInfo("Selecting camera: " + cameraName);
        var cm = SingletonHelpers.GetSingleton<CameraManager>();
        Plugin.Logger.LogInfo("SingletonHelpers.GetSingleton<CameraManager>() = " + cm);

        if (!cm.Cameras.TryGetValue(cameraName, out var ukCamera))
        {
            Log.Error($"Camera '{cameraName}' not found.");
            return;
        }
        Plugin.Logger.LogInfo($"Camera '{cameraName}' = {ukCamera}");
        CameraReplManager.Instance!.SelectedCamera = ukCamera;
        Log.Info($"Selected camera: {ukCamera.Name}");
    }
}