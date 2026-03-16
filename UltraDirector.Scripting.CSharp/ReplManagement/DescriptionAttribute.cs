using System;

namespace UltraDirector.Scripting.CSharp.ReplManagement;

public sealed class DescriptionAttribute(string description) : Attribute
{
    internal static readonly string DefaultDescription = "<No description provided>";
    public string Description { get; } = description;
}