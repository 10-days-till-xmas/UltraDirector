using BepInEx.Logging;
using UnityEngine;
// ReSharper disable once CheckNamespace
namespace UltraDirector;

public sealed partial class Plugin
{
    public static class LogHelper
    {
        internal static void LogInfo(object data) => Logger.LogInfo(data);

        internal static void LogWarning(object data) => Logger.LogWarning(data);

        internal static void LogError(object data) => Logger.LogError(data);

        internal static void LogDebug(object data) => Logger.LogDebug(data);

        internal static void DisplaySubtitle(string message, AudioSource audioSource = null!,
            bool ignoreSetting = false)
        {
            SubtitleController.Instance!.DisplaySubtitle(message, audioSource, ignoreSetting);
        }

        internal static void DisplaySubtitle(string caption, float time, GameObject origin)
        {
            SubtitleController.Instance!.DisplaySubtitle(caption, time, origin);
        }
    }
}
