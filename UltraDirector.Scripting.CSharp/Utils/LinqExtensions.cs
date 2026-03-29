using System.Collections.Generic;
using System.Linq;

namespace UltraDirector.Scripting.CSharp.Utils;

public static class LinqExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable) where T : class =>
        enumerable.Where(static e => e is not null)!;
}