using GameConsole.CommandTree;
using UltraDirector.CameraLogic.Extensions;
using UltraDirector.Utils;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand
{
    private Branch CameraSet_ =>
        Branch("set",
            Leaf<string, float>("depth", SetCameraDepth),
            Leaf<string, float, float>("rectSize", SetCameraRectSize),
            Leaf<string, float, float>("rectPosition", SetCameraRectPosition)
        );

    private void SetCameraDepth(string name, float value) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.UnityCamera.depth = value;
            Log.Info($"Camera \"{name}\" depth set to: {value}");
        });

    private void SetCameraRectSize(string name, float width, float height) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.SetRectSize(width, height);
            Log.Info($"Camera \"{name}\" rect size set to: {width}, {height}");
        });

    private void SetCameraRectPosition(string name, float x, float y) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.SetRectPosition(x, y);
            Log.Info($"Camera \"{name}\" rect position set to: ({x}, {y})");
        });
}