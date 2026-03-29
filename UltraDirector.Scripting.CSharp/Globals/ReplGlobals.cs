using JetBrains.Annotations;
using UltraDirector.CameraLogic;
using UltraDirector.Scripting.CSharp.ReplManagement;
using Logger = plog.Logger;

namespace UltraDirector.Scripting.CSharp.Globals;

[PublicAPI]
public class ReplGlobals(Logger logger) : GlobalsBase(logger)
{
    [Description("The currently selected camera. Use it to access camera properties and methods.")]
    public UKCamera? SelectedCamera => CameraReplManager.Instance!.SelectedCamera;

    [Description("Select a camera by name. Use 'CameraManager.Instance.Cameras' to see available cameras.")]
    public void SelectCamera(string name)
    {
        if (!CameraManager.Instance!.Cameras.TryGetValue(name, out var camera))
        {
            Logger.Error($"Camera with name '{name}' not found.");
            return;
        }
        CameraReplManager.Instance!.SelectedCamera = camera;
        Logger.Info("Selected camera: " + name);
    }
    public void PrintCameraNames()
    {
        var cameraNames = CameraManager.Instance!.Cameras.Keys;
        Logger.Info("Available cameras: " + string.Join(", ", cameraNames));
    }
}