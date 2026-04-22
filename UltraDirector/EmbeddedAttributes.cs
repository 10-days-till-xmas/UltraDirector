// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
#pragma warning disable CS9113 // Parameter is unread.
using System;
using Microsoft.CodeAnalysis;
namespace System.Runtime.CompilerServices
{
    [Embedded]
    internal sealed class IsExternalInit;

    [Embedded]
    internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute;

    [Embedded]
    internal sealed class RequiredMemberAttribute : Attribute;
}

namespace Microsoft.CodeAnalysis
{
    [Embedded]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct)]
    internal sealed class EmbeddedAttribute : Attribute;
}