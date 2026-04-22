using System;
using UltraDirector.PreviewWindow.Windows;
using UnityEngine;

namespace UltraDirector.PreviewWindow;
// TODO: maybe fix this a little
[Serializable]
public sealed partial class Window : MonoBehaviour
{
    private readonly Win32Window _win32Window = new();

    public Camera camera = null!;
    public WindowConfig config = new();
    private CameraBinder cameraBinder = new();
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (_win32Window.IsWindowCreated) cameraBinder.SetWindowToCameraOutput(camera, _win32Window);
    }

    private void OnDestroy() => cameraBinder.Cleanup();

    public void Show() => _win32Window.CreateWindow(config);

    public void Hide() => _win32Window.HideWindow();

    public void Close() => _win32Window.DestroyWindow();
}