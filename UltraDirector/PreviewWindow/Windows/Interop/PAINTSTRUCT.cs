using System;
using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

[StructLayout(LayoutKind.Sequential)]
internal struct PAINTSTRUCT
{
    public IntPtr hdc;
    public bool fErase;
    public RECT rcPaint;
    public bool fRestore;
    public bool fIncUpdate;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    // ReSharper disable once CollectionNeverQueried.Global
    public byte[] rgbReserved;
}