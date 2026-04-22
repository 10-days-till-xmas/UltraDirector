using System;
using UltraDirector.Utils;
using UnityEngine;

namespace UltraDirector.CameraLogic.Input.Audio;
/// <summary>
/// Place on an <see cref="AudioListener"/> to read audio samples from it.
/// The samples will be written directly to <see cref="AudioProviderBase.OutputStream"/>
/// </summary>
public sealed class AudioCompReader : AudioProviderBase
{
    protected override int SampleRate => AudioSettings.outputSampleRate;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (Recording != RecordingState.Recording) return;
        if (Channels == 0)
            Channels = channels;
        else if (Channels != channels)
            LogWarning($"Channel number changed from {Channels} to {channels}");

        if (OutputStream == null)
        {
            LogWarning("OutputStream is null but is meant to be recording");
            return;
        }
        try
        {
            OutputStream.Configuration.SamplesPerFrame = data.Length / channels;
            OutputStream.AddFrame(AudioDataHelper.UnityDataToSampleArray(data, channels));
        }
        catch (Exception e)
        {
            LogError($"Could not add frame:\n{e}");
            throw;
        }
    }
}