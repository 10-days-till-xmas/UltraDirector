#if false // Unused for now
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltraDirector.CameraLogic.Audio;

public sealed class AudioTap : MonoBehaviour
{
    [Flags]
    public enum CapturedGroups
    {
        None = 0,
        Gore = 1 << 0,
        Music = 1 << 1,
        Sounds = 1 << 2,
        Doors = 1 << 3,
        Unfreezable = 1 << 4,
        All = Gore | Music | Sounds | Doors | Unfreezable
    }
    private readonly List<float> _buffer = [];
    private readonly object _lock = new();
    private int channelCount = -1;
    public CapturedGroups CapturedGroup = CapturedGroups.All;

    private AudioSource _source = null!;
    private void Init(AudioSource source)
    {
        _source = source;
    }
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (channelCount == -1)
            channelCount = channels;
        if (channelCount != channels)
            LogWarning($"Channel number changed from {channelCount} to {channels}");
        lock (_lock)
            _buffer.AddRange(data);
    }

    private bool CheckCapturedGroup()
    {
        if (_source.outputAudioMixerGroup == null)
            return false;
        var groupName = _source.outputAudioMixerGroup.name;
        return groupName switch
        {
            "Gore" => CapturedGroup.HasFlag(CapturedGroups.Gore),
            "music" => CapturedGroup.HasFlag(CapturedGroups.Music),
            "sounds" => CapturedGroup.HasFlag(CapturedGroups.Sounds),
            "doors" => CapturedGroup.HasFlag(CapturedGroups.Doors),
            "unfreezeable" => CapturedGroup.HasFlag(CapturedGroups.Unfreezable),
            _ => false
        };
    }
}
#endif