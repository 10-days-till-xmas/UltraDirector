using System;

namespace UltraDirector.Utils;

public static class AudioDataHelper
{
    public static void ThrowIfInvalidChannels(int channels)
    {
        if (channels <= 0)
            throw new ArgumentOutOfRangeException(nameof(channels),
                "Number of channels must be greater than zero.");
    }

    private static void ThrowIfInvalidInputArray(ReadOnlySpan<float> interleaved, int channels)
    {
        if (interleaved.Length % channels != 0)
            throw new ArgumentException(
                $"Length of interleaved data must be a multiple of the number of channels. " +
                $"Received length: {interleaved.Length}, channels: {channels}.");
    }

    public static float[][] UnityDataToSampleArray(ReadOnlySpan<float> interleaved, int channels)
    {
        ThrowIfInvalidChannels(channels);
        ThrowIfInvalidInputArray(interleaved, channels);
        // data has channels interleaved
        var samplesPerChannel = interleaved.Length / channels;
        var arr = new float[channels][];
        for (var ch = 0; ch < channels; ch++)
            arr[ch] = new float[samplesPerChannel];

        for (var i = 0; i < samplesPerChannel; i++)
        {
            var bi = i * channels;
            for (var ch = 0; ch < channels; ch++)
                arr[ch][i] = interleaved[bi + ch];
        }

        return arr;
    }
}