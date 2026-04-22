using System;
using FFMediaToolkit.Common;
using FFMediaToolkit.Encoding;
using UltraDirector.CameraLogic.Input.Audio;
using UltraDirector.CameraLogic.Input.Video;
using UnityEngine;

namespace UltraDirector.CameraLogic;

public sealed partial class UKCamera
{

    public AudioVideoContainer AVContainer { get; set; } = null!;
    public AudioProviderBase? AudioSource { get; private set; } = null;
    public VideoProviderBase? VideoSource { get; private set; } = null;
    public bool Recording { get; private set; } = false;
    private MediaOutput? _output = null!;

    private TimeSpan _recordingStartTime;

    public void StartRecording(string outputPath, 
        VideoOptions videoOptions = default, AudioOptions audioOptions = default)
    {
        LogInfo("Starting recording...");
        if (AudioSource == null) LogWarning("AudioSource is null");
        if (VideoSource == null) LogWarning("VideoSource is null");
        AVContainer = new AudioVideoContainer(AudioSource, VideoSource)
        {
            RecordOptions = new RecordOptions(outputPath, videoOptions, audioOptions)
        };
        var builder = MediaBuilder.CreateContainer(outputPath, ContainerFormat.MKV)
                                  .UseMetadata(new ContainerMetadata() { Description = "Created with UltraDirector" });
        AVContainer.AddToMediaBuilder(ref builder);

        _output = builder.Create();

        Recording = true;
        AVContainer.StartRecording(_output);
        _recordingStartTime = TimeSpan.FromMilliseconds(Time.realtimeSinceStartupAsDouble);
    }

    public void StopRecording()
    {
        var options = AVContainer.StopRecording();
        _output?.Dispose();
        _output = null;
        Recording = false;
        Remux.ToMp4(options.FilePath);
        LogInfo("Remuxing complete");
    }
}