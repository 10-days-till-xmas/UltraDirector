using System;
using UnityEngine;

namespace UltraDirector.PreviewWindow;
// ReSharper disable Unity.RedundantSerializeFieldAttribute
[Serializable]
public class WindowConfig
{
    [field: SerializeField]
    public WindowStyle Style { get; set; } = WindowStyle.OverlappedWindow;

    [field: SerializeField]
    public string WindowTitle { get; set; } = "Unity Window";

    [field: SerializeField, Range(0, int.MaxValue)]
    public int Width { get; set => field = IntVerify(value); } = 800;

    [field: SerializeField, Range(0, int.MaxValue)]
    public int Height { get; set => field = IntVerify(value); } = 600;

    [field: SerializeField, Range(0, int.MaxValue)]
    public int X { get; set => field = IntVerify(value); } = 100;

    [field: SerializeField, Range(0, int.MaxValue)]
    public int Y { get; set => field = IntVerify(value); } = 100;

    private static int IntVerify(int value) =>
        value >= 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value),
                  "Value must be non-negative.");
}