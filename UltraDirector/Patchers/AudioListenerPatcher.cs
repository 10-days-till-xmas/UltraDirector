using HarmonyLib;
using UltraDirector.CameraLogic.Input.Audio;
using UnityEngine;

namespace UltraDirector.Patchers;
[HarmonyPatch(typeof(AudioListener))]
public static class AudioListenerPatcher
{
    private static AudioCompReader _audioCompReader = null!;
    [HarmonyPostfix]
    [HarmonyPatch(MethodType.Constructor)]
    public static void AttachAudioReader(AudioListener __instance)
    {
        _audioCompReader = __instance.gameObject.AddComponent<AudioCompReader>();
    }

    extension(AudioListener)
    {
        public static AudioCompReader AudioReader => _audioCompReader;
    }
}