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
            Leaf<string, float, float>("rectPosition", SetCameraRectPosition),
            Branch("cullingMask",
                Leaf<string, int>("set", SetCullingMask),
                Leaf<string, GameObjectLayer>("add", AddToCullingMask),
                Leaf<string, GameObjectLayer>("remove", RemoveFromCullingMask),
                Leaf<string, GameObjectLayer>("toggle", ToggleCullingMask))
        );

    private void ToggleCullingMask(string name, GameObjectLayer layer) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.Camera.cullingMask ^= ((int)layer);
            var state = ((GameObjectLayer)camera.Camera.cullingMask).HasFlagFast(layer);
            Log.Info($"Camera \"{name}\" toggled layer: {layer}, state: {(state ? "active" : "inactive")}");
        });


    private void RemoveFromCullingMask(string name, GameObjectLayer layer) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.Camera.cullingMask &= ~((int)layer);
            Log.Info($"Camera \"{name}\" removed layer: {layer}");
        });

    private void AddToCullingMask(string name, GameObjectLayer layer) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.Camera.cullingMask |= (int)layer;
            Log.Info($"Camera \"{name}\" added layer: {layer}");
        });

    private void SetCullingMask(string name, int mask) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.Camera.cullingMask = mask;
            Log.Info($"Camera \"{name}\" mask set to: {mask}");
        });

    private void SetCameraDepth(string name, float value) => TryGetCamera(name)
       .IfNotNullDo(camera =>
        {
            camera.Camera.depth = value;
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