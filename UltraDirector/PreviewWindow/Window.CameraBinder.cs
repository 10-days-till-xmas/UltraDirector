using System;
using UltraDirector.PreviewWindow.Windows;
using UnityEngine;

namespace UltraDirector.PreviewWindow;

public sealed partial class Window
{
    /// <summary>
    /// Utility class for connecting Unity cameras to Win32 windows.
    /// </summary>
    internal sealed class CameraBinder
    {
        private RenderTexture? _renderTexture;
        private Texture2D? _readbackTexture;
        private byte[]? _pixelBuffer;

        /// <summary>
        /// Captures the current frame from the camera and displays it on the window.
        /// Call this method every frame (e.g., in Update or LateUpdate) to continuously update the window.
        /// </summary>
        /// <param name="camera">The Unity camera to capture from.</param>
        /// <param name="window">The Win32 window to display the output on.</param>
        /// <remarks>
        /// This method renders the camera to a RenderTexture, reads the pixels back to the CPU,
        /// converts from RGBA to BGRA format, and updates the window texture.
        /// For best performance, call this in LateUpdate after all rendering is complete.
        /// </remarks>
        public void SetWindowToCameraOutput(Camera camera, Win32Window window)
        {
            if (camera == null)
                throw new ArgumentNullException(nameof(camera));
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            var width = camera.pixelWidth;
            var height = camera.pixelHeight;

            // Create or resize the RenderTexture if needed
            if (_renderTexture == null || _renderTexture.width != width || _renderTexture.height != height)
            {
                if (_renderTexture != null)
                    _renderTexture.Release();

                _renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
                _renderTexture.Create();
            }

            // Create or resize the readback texture if needed
            if (_readbackTexture == null || _readbackTexture.width != width || _readbackTexture.height != height)
            {
                _readbackTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            }

            // Allocate pixel buffer if needed
            var bufferSize = width * height * 4;
            if (_pixelBuffer == null || _pixelBuffer.Length != bufferSize)
            {
                _pixelBuffer = new byte[bufferSize];
            }

            // Store the camera's original target
            var originalTarget = camera.targetTexture;

            // Render the camera to our RenderTexture
            camera.targetTexture = _renderTexture;
            camera.Render();
            camera.targetTexture = originalTarget;

            // Read the pixels from the RenderTexture
            RenderTexture.active = _renderTexture;
            _readbackTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
            _readbackTexture.Apply();
            RenderTexture.active = null;

            // Get the raw pixel data
            Span<byte> pixels = _readbackTexture.GetRawTextureData();


            CameraUtil.ConvertRgbaToBgra(ref pixels, ref _pixelBuffer, width, height);

            // Update the window texture
            window.SetTexture(_pixelBuffer, width, height);
            window.Invalidate();
        }

        /// <summary>
        /// Cleans up any resources used by the camera utility.
        /// Call this when you're done using the camera output feature.
        /// </summary>
        public void Cleanup()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
                Destroy(_renderTexture);
                _renderTexture = null;
            }

            if (_readbackTexture != null)
            {
                Destroy(_readbackTexture);
                _readbackTexture = null;
            }

            _pixelBuffer = null;
        }
    }
}