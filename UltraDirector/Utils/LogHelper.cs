using UnityEngine;

namespace UltraDirector.Utils;

public static class LogHelper
{
    internal static void Log(object data) => Plugin.Logger.LogInfo(data);

    internal static void LogWarning(object data) => Plugin.Logger.LogWarning(data);

    internal static void LogError(object data) => Plugin.Logger.LogError(data);
    
    internal static void LogDebug(object data) => Plugin.Logger.LogDebug(data);

    internal static void DisplaySubtitle(string message, AudioSource audioSource = null!, bool ignoreSetting = false)
    {
        MonoSingleton<SubtitleController>.Instance
            .DisplaySubtitle(message, audioSource, ignoreSetting);
    }

    internal static void DisplaySubtitle(string caption, float time, GameObject origin)
    {
        MonoSingleton<SubtitleController>.Instance
            .DisplaySubtitle(caption, time, origin);
    }
}