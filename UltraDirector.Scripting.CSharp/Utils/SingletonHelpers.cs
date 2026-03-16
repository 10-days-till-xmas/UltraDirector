using UnityEngine;

namespace UltraDirector.Scripting.CSharp.Utils;

public static class SingletonHelpers
{
    public static T GetSingleton<T>() where T : MonoSingleton<T>
    {
        Plugin.Logger.LogInfo("GetSingleton<T>()");
        if (MonoSingleton<T>.Instance == null) return Object.FindObjectOfType<T>();
        Plugin.Logger.LogInfo("MonoSingleton<T>.Instance != null");
        return MonoSingleton<T>.Instance;
    }
}