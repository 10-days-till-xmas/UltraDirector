using System;

namespace UltraDirector.Utils;

public static class ObjectExtensions
{
    extension<T>(T? obj)
    {
        public void IfNotNullDo(Action<T> action)
        {
            if (obj is not null)
                action(obj);
        }
    }
}