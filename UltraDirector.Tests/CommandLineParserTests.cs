using JetBrains.Annotations;
using UltraDirector.CameraLogic.ConsoleCommands;

namespace UltraDirector.Tests;

public sealed class CommandLineParserTests
{
    [PublicAPI]
    public static TheoryData<string, string[]> commandLineData = new()
    {
        { "camera list", ["camera", "list"] },
        { "camera setDepth 5.0", ["camera", "setDepth", "5.0"] },
        { """
          camera get "My Camera"
          """, ["camera", "get", "My Camera"]},
        { @"command arg1 arg\\arg2", ["command", "arg1", @"arg\arg2"] },
        { """
          command "arg with spaces"
          """, ["command", "arg with spaces"] },
        { """
          command "arg with \"quotes\""
          """, ["command", "arg with \"quotes\""] },
    };


    [Theory]
    [MemberData(nameof(commandLineData))]
    public void TestParsing(string input, string[] expected)
    {
        var result = CommandLineParser.Parse(input);
        Assert.Equal(expected.Length, result.Length);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ThrowsIfUnclosedQuotes()
    {
        const string input = @"camera get ""My Camera";
        Assert.Throws<ArgumentException>(() => CommandLineParser.Parse(input));
    }
}