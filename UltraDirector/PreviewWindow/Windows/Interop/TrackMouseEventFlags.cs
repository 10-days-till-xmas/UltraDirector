using System;

namespace UltraDirector.PreviewWindow.Windows.Interop;

/// <summary>
/// Flags for the TrackMouseEvent function.
/// </summary>
[Flags]
internal enum TrackMouseEventFlags : uint
{
    /// <summary>
    /// The caller wants to cancel a prior tracking request. The caller should also specify the type of tracking that it wants to cancel.
    /// For example, to cancel hover tracking, the caller must pass the TME_CANCEL and TME_HOVER flags.
    /// </summary>
    TME_CANCEL = 0x80000000,

    /// <summary>
    /// The caller wants hover notification. Notification is delivered as a WM_MOUSEHOVER message.
    /// If the caller requests hover tracking while hover tracking is already active, the hover timer will be reset.
    /// </summary>
    TME_HOVER = 0x00000001,

    /// <summary>
    /// The caller wants leave notification. Notification is delivered as a WM_MOUSELEAVE message.
    /// If the mouse is not over the specified window or area, a leave notification is generated immediately and no further tracking is performed.
    /// </summary>
    TME_LEAVE = 0x00000002,

    /// <summary>
    /// The caller wants hover and leave notification for the nonclient areas.
    /// Notification is delivered as WM_NCMOUSEHOVER and WM_NCMOUSELEAVE messages.
    /// </summary>
    TME_NONCLIENT = 0x00000010,

    /// <summary>
    /// The function fills in the structure instead of treating it as a tracking request.
    /// The structure is filled such that had that structure been passed to TrackMouseEvent, it would generate the current tracking.
    /// </summary>
    TME_QUERY = 0x40000000
}

