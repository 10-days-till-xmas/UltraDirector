using GameConsole.CommandTree;
using UltraDirector.Utils;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand
{
    private Branch CameraGet_ =>
        Branch("get",
            Leaf<string>("summary", GetCameraSummary),
            Leaf<string>("depth", GetCameraDepth),
            Leaf<string>("rectSize", GetCameraRectSize),
            Leaf<string>("rectPosition", GetCameraRectPosition)
        );

    private void GetCameraSummary(string name) => TryGetCamera(name)
       .IfNotNullDo(c => Log.Info($"Camera '{c.Name}' details: {c}"));

    private void GetCameraDepth(string name) => TryGetCamera(name)
       .IfNotNullDo(c => Log.Info($"Camera '{c.Name}' depth: {c.Camera.depth}"));

    private void GetCameraRectSize(string name) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            var rectSize = camera.Camera.rect.size;
            Log.Info($"Camera \"{name}\" rect size: ({rectSize.x}, {rectSize.y})");
        });

    private void GetCameraRectPosition(string name) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            var rectPos = camera.Camera.rect.position;
            Log.Info($"Camera \"{name}\" rect position: ({rectPos.x}, {rectPos.y})");
        });
}