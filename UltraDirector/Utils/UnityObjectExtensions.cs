using UnityEngine;

namespace UltraDirector.Utils;

public static class UnityObjectExtensions
{
    extension(Object obj)
    {
        public void Destroy()
        {
            if (obj != null) Object.Destroy(obj);
        }

        public void DestroyImmediate()
        {
            if (obj != null) Object.DestroyImmediate(obj);
        }
    }
}