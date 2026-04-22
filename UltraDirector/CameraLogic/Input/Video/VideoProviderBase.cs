using FFMediaToolkit.Encoding;

namespace UltraDirector.CameraLogic.Input.Video;

public abstract class VideoProviderBase : InputProviderBase<VideoOptions>
{
    protected VideoOutputStream? OutputStream;

    public bool StartRecording(VideoOutputStream? outputStream)
    {
        if (!base.StartRecording()) return false;
        OutputStream = outputStream;
        return true;
    }

    public abstract void AddVideo(ref MediaBuilder mediaBuilder);
}