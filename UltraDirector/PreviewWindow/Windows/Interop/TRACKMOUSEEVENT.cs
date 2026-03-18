using System;
using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

/// <summary>
/// Used by the TrackMouseEvent function to track when the mouse pointer leaves a window or hovers over a window for a specified amount of time.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct TRACKMOUSEEVENT
{
    /// <summary>
    /// The size of the TRACKMOUSEEVENT structure, in bytes.
    /// </summary>
    public uint cbSize;

    /// <summary>
    /// The services requested. Can be a combination of TrackMouseEventFlags values.
    /// </summary>
    public TrackMouseEventFlags dwFlags;

    /// <summary>
    /// A handle to the window to track.
    /// </summary>
    public IntPtr hwndTrack;

    /// <summary>
    /// The hover time-out (if TME_HOVER was specified). Can be HOVER_DEFAULT to use the system default hover time-out.
    /// </summary>
    public uint dwHoverTime;


    /// <summary>
    /// Use the system default hover time-out.
    /// </summary>
    public const uint HOVER_DEFAULT = 0xFFFFFFFF;
}

