using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UltraDirector.Utils;

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> enumerable)
    {
        public async Task ForEachAsync(Func<T, Task> func)
        {
            await Task.WhenAll(
                enumerable.Select(func)
            );
        }

    }
}