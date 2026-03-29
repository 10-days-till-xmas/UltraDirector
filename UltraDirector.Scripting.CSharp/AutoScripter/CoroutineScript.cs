using System;
using System.Collections;
using System.Threading;
using Microsoft.CodeAnalysis.Scripting;

namespace UltraDirector.Scripting.CSharp.AutoScripter;

public sealed partial class CoroutineScript
{
    private readonly Func<IEnumerator> _factory;
    private CoroutineScript(Func<IEnumerator> factory)
    {
        _factory = factory;
    }
    /// <summary>
    /// Returns a new clean enumerator, 
    /// </summary>
    /// <remarks>Note that IEnumerators instantiated via this do not support <see cref="IEnumerator.Reset"/></remarks>
    /// <returns></returns>
    public IEnumerator GetNewEnumerator() => _factory();
}