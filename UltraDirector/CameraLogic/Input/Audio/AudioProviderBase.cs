using FFMediaToolkit.Audio;
using FFMediaToolkit.Encoding;

namespace UltraDirector.CameraLogic.Input.Audio;

public abstract class AudioProviderBase : InputProviderBase<AudioOptions>
{
    protected int Channels { get; set; } = 0;
    protected abstract int SampleRate { get; }
    public AudioOutputStream? OutputStream { get; set; } = null;

    public void AddAudio(ref MediaBuilder mediaBuilder)
    {
        Channels = Channels == 0 ? 2 : Channels;
        LogInfo("Channels: " + Channels);
        LogInfo("SampleRate: " + SampleRate);
        mediaBuilder.WithAudio(new AudioEncoderSettings(
            SampleRate,
            Channels,
            AudioCodec.AAC)
        {
            Bitrate = 128_000,
            SampleFormat = SampleFormat.SingleP,
            SamplesPerFrame = 1024
        });
    }

    public bool StartRecording(AudioOutputStream outputStream)
    {
        if (!RecordOptions.RecordAudio) return false;
        if (!base.StartRecording()) return false;
        OutputStream = outputStream;
        return true;
    }
}