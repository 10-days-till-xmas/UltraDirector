using System;
using System.Collections;
using UnityEngine;

namespace UltraDirector.Utils;

public static class CoroutineHelper
{
    public static IEnumerator DoAfterDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}