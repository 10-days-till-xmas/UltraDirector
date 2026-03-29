using System.Collections;
using Microsoft.CodeAnalysis.Scripting;
using UltraDirector.Scripting.CSharp.AutoScripter;

namespace UltraDirector.Scripting.CSharp.Tests;
#pragma warning disable xUnit1045
public sealed class CoroutineScriptTests
{
    public static readonly TheoryData<string, object?[]> ExampleScripts = new()
    {
        { // simple case
            """
            yield return 1;
            yield return 2;
            yield return null;
            """, [1, 2, null]
        },
        { // with usings
            """
            using System;
            yield return 1;
            yield return 2;
            yield return null;
            """, [1, 2, null]
        },
        { // with global usings
            """
            global using System;
            yield return 1;
            yield return 2;
            yield return null;
            """, [1, 2, null]
        },
        { // with static usings
            """
            using static System.Console;
            yield return 1;
            yield return 2;
            yield return null;
            """, [1, 2, null]
        },
        { // with weird logic
            """
            using System.Collections.Generic;
            var list = new List<int> { 1, 2, 3 };
            yield return list;
            list.Add(4);
            """, [new List<int> { 1, 2, 3, 4 }]
        },
        { // with comments
            """
            using System.Collections.Generic; // comment
            using System;
            yield return 1;
            yield return 2;
            yield return null;
            """, [1, 2, null]
        }
    };

    public static readonly TheoryData<string> InvalidScripts = new()
    {
        """
        using System.Threading.Tasks;
        await Task.Yield();
        yield return 1;
        """,
        """
        using System.Collections;
        IEnumerator ___CreateScriptEnumerator()
        {
            yield return 42;
        }
        yield return 1;
        """,
        """
        return 123;
        """,
        """
        // comment-only script should fail after wrapping because iterator has no yield/return path
        """
    };

    [Theory]
    [MemberData(nameof(ExampleScripts))]
    public async Task CreateAsync_ReturnsExpected(string code, object?[] expectedReturn)
    {
        var scriptEnumerator = await CoroutineScript.CreateAsync(code, TestContext.Current.CancellationToken);
        var results = Drain(scriptEnumerator.GetNewEnumerator());
        Assert.Equal(expectedReturn, results);
    }

    [Theory]
    [MemberData(nameof(ExampleScripts))]
    public async Task CoroutineScript_CanReturnMultipleTimes(string code, object?[] expectedReturn)
    {
        var script = (await CoroutineScript.CreateAsync(code, TestContext.Current.CancellationToken));
        var scriptEnumerator = script.GetNewEnumerator();
        var results = Drain(scriptEnumerator);
        Assert.Equal(expectedReturn, results);
        scriptEnumerator = script.GetNewEnumerator();
        results = Drain(scriptEnumerator);
        Assert.Equal(expectedReturn, results);
    }

    [Theory]
    [MemberData(nameof(InvalidScripts))]
    public async Task CreateAsync_ThrowsCompilationError_ForInvalidScripts(string code)
    {
        await Assert.ThrowsAsync<CompilationErrorException>(
            () => CoroutineScript.CreateAsync(code, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_DefersExceptionsUntilEnumeration()
    {
        const string code =
            """
            using System;
            throw new InvalidOperationException("boom");
            yield break;
            """;

        var scriptCoroutine = (await CoroutineScript.CreateAsync(code, TestContext.Current.CancellationToken)).GetNewEnumerator();

        var exception = Assert.Throws<InvalidOperationException>(() => scriptCoroutine.MoveNext());
        Assert.Equal("boom", exception.Message);
    }

    [Fact]
    public async Task Reset_RecreatesStateMachine_AndStartsFromBeginning()
    {
        const string code =
            """
            var count = 0;
            yield return count++;
            yield return count++;
            """;

        var script = (await CoroutineScript.CreateAsync(code, TestContext.Current.CancellationToken));
        var scriptCoroutine = script.GetNewEnumerator();

        var firstPass = Drain(scriptCoroutine);
        Assert.Equal([0, 1], firstPass);

        scriptCoroutine = script.GetNewEnumerator();

        var secondPass = Drain(scriptCoroutine);
        Assert.Equal([0, 1], secondPass);
    }

    [Fact]
    public async Task CreateFromFileAsync_ThrowsForMissingFile()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), $"ud-script-{Guid.NewGuid():N}.csx");

        await Assert.ThrowsAsync<FileNotFoundException>(
            () => CoroutineScript.CreateFromFileAsync(missingPath, TestContext.Current.CancellationToken));
    }

    public static int someNumber = 0;
    [Fact]
    public async Task CoroutineScript_CanAccessApplicationState()
    {
        const string code =
            """
            using UltraDirector.Scripting.CSharp.Tests;
            CoroutineScriptTests.someNumber += 10;
            yield return CoroutineScriptTests.someNumber;
            yield return CoroutineScriptTests.someNumber;
            yield return CoroutineScriptTests.someNumber;
            CoroutineScriptTests.someNumber -= 5;
            """;

        var script = await CoroutineScript.CreateAsync(code, TestContext.Current.CancellationToken);
        var scriptEnumerator = script.GetNewEnumerator();
        var results = new List<object?>();
        while (scriptEnumerator.MoveNext())
        {
            results.Add(scriptEnumerator.Current);
            someNumber--;
        }
        Assert.Equal([10, 9, 8], results);
        Assert.Equal(2, someNumber);
        scriptEnumerator = script.GetNewEnumerator();
        results.Clear();
        while (scriptEnumerator.MoveNext())
        {
            results.Add(scriptEnumerator.Current);
            someNumber--;
        }
        Assert.Equal([12, 11, 10], results);
        Assert.Equal(4, someNumber);
    }


    private static List<object?> Drain(IEnumerator coroutine)
    {
        var results = new List<object?>();
        while (coroutine.MoveNext())
            results.Add(coroutine.Current);
        return results;
    }
}