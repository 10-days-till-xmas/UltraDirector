using FFMediaToolkit.Encoding;
using UltraDirector.CameraLogic.Input.Audio;
using UltraDirector.CameraLogic.Input.Video;

namespace UltraDirector.CameraLogic;

public sealed class AudioVideoContainer(AudioProviderBase? audioProvider, VideoProviderBase? videoProvider)
{
    public AudioProviderBase? AudioProvider { get;} = audioProvider;
    public VideoProviderBase? VideoProvider { get; } = videoProvider;

    public RecordOptions RecordOptions
    {
        set
        {
            AudioProvider?.WithOptions(value.AudioOptions);
            VideoProvider?.WithOptions(value.VideoOptions);
        }
    }

    public void StartRecording(MediaOutput output)
    {
        if (AudioProvider?.StartRecording(output.Audio) is false)
            LogWarning("Could not start audio provider recording");
        if (VideoProvider?.StartRecording(output.Video) is false)
            LogWarning("Could not start video provider recording");
    }

    public void AddToMediaBuilder(ref MediaBuilder builder)
    {
        AudioProvider?.AddAudio(ref builder);
        VideoProvider?.AddVideo(ref builder);
    }

    public RecordOptions StopRecording()
    {
        AudioOptions aOpt = default;
        VideoOptions vOpt = default;
        AudioProvider?.TryStopRecording(out aOpt);
        VideoProvider?.TryStopRecording(out vOpt);

        return new RecordOptions("", vOpt, aOpt);
    }
}