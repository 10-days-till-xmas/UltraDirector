using System;
using JetBrains.Annotations;

namespace UltraDirector.CameraLogic;
[PublicAPI]
[Flags]
public enum GameObjectLayer
{
    Default = 0,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,
    Unused3 = 1 << 3,
    Water = 1 << 4,
    UI = 1 << 5,
    Unused6 = 1 << 6,
    Unused7 = 1 << 7,
    Environment = 1 << 8,
    Gib = 1 << 9,
    Limb = 1 << 10,
    BigCorpse = 1 << 11,
    EnemyTrigger = 1 << 12,
    AlwaysOnTop = 1 << 13,
    Projectile = 1 << 14,
    Invincible = 1 << 15,
    Invisible = 1 << 16,
    BrokenGlass = 1 << 17,
    PlayerOnly = 1 << 18,
    VirtualScreen = 1 << 19,
    GroundCheck = 1 << 20,
    EnemyWall = 1 << 21,
    Item = 1 << 22,
    Explosion = 1 << 23,
    Outdoors = 1 << 24,
    OutdoorsNonsolid = 1 << 25,
    Armor = 1 << 26,
    GibLit = 1 << 27,
    VirtualRender = 1 << 28,
    SandboxGrabbable = 1 << 29,
    Portal = 1 << 30,
    Unused31 = 1 << 31
}

public static class GameObjectLayersExtensions
{
    public static bool HasFlagFast(this GameObjectLayer value, GameObjectLayer flag)
    {
        return (value & flag) != 0;
    }
}