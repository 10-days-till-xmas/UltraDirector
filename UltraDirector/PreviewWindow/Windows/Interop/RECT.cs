using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

[StructLayout(LayoutKind.Sequential)]
internal struct RECT(int left, int top, int right, int bottom)
    : IEquatable<RECT>, IEquatable<Rectangle>
{
    public int Left = left;
    public int Top = top;
    public int Right = right;
    public int Bottom = bottom;

    public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

    public int X
    {
        get => Left;
        set { Right -= (Left - value); Left = value; }
    }

    public int Y
    {
        get => Top;
        set { Bottom -= (Top - value); Top = value; }
    }

    public int Height
    {
        get => Bottom - Top;
        set => Bottom = value + Top;
    }

    public int Width
    {
        get => Right - Left;
        set => Right = value + Left;
    }

    public Point Location
    {
        get => new(Left, Top);
        set { X = value.X; Y = value.Y; }
    }

    public Size Size
    {
        get => new(Width, Height);
        set { Width = value.Width; Height = value.Height; }
    }

    public static implicit operator Rectangle(RECT r) => new(r.Left, r.Top, r.Width, r.Height);

    public static implicit operator RECT(Rectangle r) => new(r);

    public static bool operator ==(RECT r1, RECT r2) => r1.Equals(r2);

    public static bool operator !=(RECT r1, RECT r2) => !r1.Equals(r2);

    public bool Equals(RECT r) => r.Left == Left
                               && r.Top == Top
                               && r.Right == Right
                               && r.Bottom == Bottom;

    public bool Equals(Rectangle other) => other.Left == Left
                                        && other.Top == Top
                                        && other.Right == Right
                                        && other.Bottom == Bottom;

    public override bool Equals(object? obj) =>
        obj switch
        {
            RECT r      => Equals(r),
            Rectangle r => Equals(r),
            _           => false
        };

    // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => string.Format(System.Globalization.CultureInfo.CurrentCulture,
        "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
}