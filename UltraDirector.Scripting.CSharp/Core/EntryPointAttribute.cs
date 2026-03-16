using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using MonoMod.Utils;

namespace UltraDirector.Scripting.CSharp.Core;
/// <summary>
/// Marks a static method as the entry point for a script.
/// The method must return an IEnumerator and can be either public or non-public.
/// The script runner will look for the first method marked with this attribute and execute it as a coroutine when the script is loaded.
/// </summary>
[PublicAPI]
public class EntryPointAttribute : Attribute
{
    private const BindingFlags staticBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    public static Func<IEnumerator>? GetEntryPoint(Assembly assembly) =>
        assembly
           .GetExportedTypes()
           .SelectMany(static type => type.GetMethods(staticBindingFlags))
           .Where(static method => method.GetCustomAttribute<EntryPointAttribute>() != null)
           .FirstOrDefault(static method => method.ReturnType == typeof(IEnumerator))
          ?.CreateDelegate<Func<IEnumerator>>();
    public static IEnumerable<Func<IEnumerator>> GetEntryPoints(Assembly assembly) =>
        assembly
           .GetExportedTypes()
           .SelectMany(static type => type.GetMethods(staticBindingFlags))
           .Where(static method => method.GetCustomAttribute<EntryPointAttribute>() != null)
           .Where(static method => method.ReturnType == typeof(IEnumerator))
           .Select(static method => method.CreateDelegate<Func<IEnumerator>>());
}