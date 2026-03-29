using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace UltraDirector.Scripting.CSharp.Hooking;

public sealed class Hook : IDisposable
{
    public enum HookType
    {
        Post,
        Pre
    }

    private class HookInfo(MethodInfo target, HookType hookType = HookType.Post ) : IEquatable<HookInfo>
    {
        public MethodInfo TargetMethod { get; } = target;
        public HookType HookType { get; } = hookType;

        public bool Equals(HookInfo? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return TargetMethod.Equals(other.TargetMethod) && HookType == other.HookType;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not Hook) return false;
            return Equals((HookInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TargetMethod.GetHashCode() * 397) ^ (int)HookType;
            }
        }

        public static bool operator ==(HookInfo? left, HookInfo? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(HookInfo? left, HookInfo? right)
        {
            return !Equals(left, right);
        }
    }
    public event Action? OnTrigger = null;
    private static readonly Harmony harmony = new("UltraDirector.Scripting.CSharp.Hooking");
    private readonly MethodInfo ActivateTriggerMethod;
    private readonly MethodInfo TargetMethod;
    private Hook(HookInfo hookInfo)
    {
        ActivateTriggerMethod = ((Delegate)ActivateTrigger).Method;
        TargetMethod = hookInfo.TargetMethod;
        switch (hookInfo.HookType)
        {
            case HookType.Pre:
                harmony.Patch(TargetMethod, prefix: new HarmonyMethod(ActivateTriggerMethod));
                break;
            case HookType.Post:
                harmony.Patch(TargetMethod, postfix: new HarmonyMethod(ActivateTriggerMethod));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hookInfo), hookInfo.HookType,
                    "Unknown hook type");
        }
        _cache.Add(hookInfo, this);
    }

    private void ActivateTrigger() => OnTrigger?.Invoke();

    public void Dispose() => harmony.Unpatch(TargetMethod, ActivateTriggerMethod);
    private static readonly Dictionary<HookInfo, Hook> _cache = new();

    public static Hook Create(MethodInfo target, HookType hookType = HookType.Post )
    {
        var hookInfo = new HookInfo(target, hookType);
        if (_cache.TryGetValue(hookInfo, out var hook)) return hook;
        return new Hook(hookInfo);
    }
}