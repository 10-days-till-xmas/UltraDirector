using GameConsole.CommandTree;
using UltraDirector.PreviewWindow;
using UltraDirector.Utils;
using UnityEngine;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand
{
    public Branch Window => Branch("window",
        Leaf<string>("create", CreateCameraWindow),    // <camera-id>
        Leaf<string>("destroy", DestroyCameraWindow)); // <camera-id>

    private void CreateCameraWindow(string obj) =>
        TryGetCamera(obj).IfNotNullDo(c =>
        {
            var window = c.gameObject.AddComponent<Window>();
            window.camera = c.Camera;
            window.Show();
        });

    private void DestroyCameraWindow(string obj) =>
        TryGetCamera(obj).IfNotNullDo(c =>
        {
            var window = c.GetComponent<Window>();
            if (window == null) return;
            window.Close();
            Object.Destroy(window);
        });
}