using System;
using UnityEngine;

namespace UltraDirector.CameraLogic;

public abstract class InputProviderBase<TOptions> : MonoBehaviour
where TOptions : IRecordOptions, new()
{
    public enum RecordingState
    {
        NotRecording,
        Recording,
    }
    public RecordingState Recording { get; protected set; } = RecordingState.NotRecording;
    protected TOptions RecordOptions { get; private set; } = new();
    private TimeSpan RecordingStartTime { get; set; }
    private static TimeSpan GetCurrentTime() => TimeSpan.FromSeconds(Time.realtimeSinceStartupAsDouble);
    protected TimeSpan GetElapsedTime() => GetCurrentTime() - RecordingStartTime;

    public void WithOptions(TOptions options) => RecordOptions = options;
    protected virtual bool StartRecording()
    {
        if (Recording == RecordingState.Recording) {
            LogWarning("Already recording, cannot start a new recording session.");
            return false; // already recording, abort
        }
        Recording = RecordingState.Recording;
        RecordingStartTime = GetCurrentTime();
        return true;
    }

    public virtual bool TryStopRecording(out TOptions? options)
    {
        options = default;
        if (Recording != RecordingState.Recording) return false; // not recording, abort
        Recording = RecordingState.NotRecording;
        return true;
    }

    protected void OnDisable() => TryStopRecording(out _);

    protected void OnDestroy() => TryStopRecording(out _);
}