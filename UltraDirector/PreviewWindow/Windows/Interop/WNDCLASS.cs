using System;
using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

[StructLayout(LayoutKind.Sequential)]
// ReSharper disable once UnusedType.Global
internal struct WNDCLASS
{
    public ClassStyles style;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    public WndProc lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public IntPtr hInstance;
    public IntPtr hIcon;
    public IntPtr hCursor;
    public IntPtr hbrBackground;
    [MarshalAs(UnmanagedType.LPTStr)]
    public string lpszMenuName;
    [MarshalAs(UnmanagedType.LPTStr)]
    public string lpszClassName;
}