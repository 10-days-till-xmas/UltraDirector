using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using UltraDirector.Utils;
using UnityEngine;

namespace UltraDirector.CameraLogic.Input.Video;

public sealed class VideoRecorder : VideoProviderBase
{
    public Camera camera = null!;

    private RenderTexture? _rt = null;
    private Texture2D? _readback = null;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public override void AddVideo(ref MediaBuilder mediaBuilder)
    {
        var width = camera.pixelWidth;
        var height = camera.pixelHeight;

        mediaBuilder.WithVideo(new VideoEncoderSettings(
            width,
            height,
            RecordOptions.Fps,
            codec: VideoCodec.H264)
        {
            CRF = RecordOptions.CRF,
            EncoderPreset = RecordOptions.EncoderPreset,
            FlipVertically = true
        });
    }

    protected override bool StartRecording()
    {
        if (!base.StartRecording())
        {
            LogWarning("VideoProviderBase.StartRecording() could not start recording");
            return false;
        }
        var width = camera.pixelWidth;
        var height = camera.pixelHeight;
        _rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        _readback = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: false);

        camera.targetTexture = _rt;
        return true;
    }

    public override bool TryStopRecording(out VideoOptions options)
    {
        if (!base.TryStopRecording(out options)) return false;

        camera.targetTexture = null;

        _readback?.Destroy();
        if (_rt != null)
        {
            _rt.Release();
            _rt.Destroy();
        }

        _readback = null;
        _rt = null;
        return true;
    }

    private void LateUpdate()
    {
        if (Recording != RecordingState.Recording) return;
        if (_rt == null || _readback == null || OutputStream == null)
        {
            return;
        }
        var prev = RenderTexture.active;
        RenderTexture.active = _rt;
        camera.Render();
        _readback.ReadPixels(new Rect(x: 0, y: 0, _rt.width, _rt.height),
            destX: 0, destY: 0, recalculateMipMaps: false);
        _readback.Apply(updateMipmaps: false, makeNoLongerReadable: false);
        var pixels = _readback.GetRawTextureData<byte>().ToArray();
        using var frame = ImageData.FromArray(pixels, ImagePixelFormat.Rgba32, _rt.width, _rt.height);
        OutputStream.AddFrame(frame, GetElapsedTime());
        RenderTexture.active = prev;
    }
}