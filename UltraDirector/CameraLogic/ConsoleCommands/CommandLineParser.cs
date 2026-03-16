using System;
using System.Collections.Generic;
using System.Text;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public static class CommandLineParser
{
    /// <summary>
    /// Splits a command-line string into an array of arguments,
    /// similar to how Windows processes command-line input.
    /// </summary>
    /// <param name="input">The raw command-line string.</param>
    /// <param name="isComplete">If the input string is completed or not</param>
    /// <returns>An array of arguments.</returns>
    public static string[] Parse(string input, bool isComplete = true)
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];

        var args = new List<string>();
        var currentArg = new StringBuilder();
        var inQuotes = false;
        var escapeNext = false;

        foreach (var c in input)
        {
            if (escapeNext)
            {
                currentArg.Append(c);
                escapeNext = false;
                continue;
            }
            switch (c)
            {
                case '\\':
                    escapeNext = true;
                    break;
                case '"':
                    inQuotes = !inQuotes;
                    break;
                default:
                    if (!char.IsWhiteSpace(c) || inQuotes)
                        currentArg.Append(c);
                    else if (currentArg.Length > 0)
                    {
                        args.Add(currentArg.ToString());
                        currentArg.Clear();
                    }
                    break;
            }
        }
        if (inQuotes && isComplete) throw new ArgumentException("Unterminated quotes in command line.");
        if (currentArg.Length > 0)
            args.Add(currentArg.ToString());
        return args.ToArray();
    }
}