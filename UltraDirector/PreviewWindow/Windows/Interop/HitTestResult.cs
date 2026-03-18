namespace UltraDirector.PreviewWindow.Windows.Interop;

/// <summary>
/// Hit test return values for WM_NCHITTEST and MOUSEHOOKSTRUCT Mouse Position Codes.
/// </summary>
internal enum HitTestResult
{
    /// <summary>
    /// On the screen background or on a dividing line between windows.
    /// </summary>
    HTNOWHERE = 0,

    /// <summary>
    /// In a client area.
    /// </summary>
    HTCLIENT = 1,

    /// <summary>
    /// In a title bar.
    /// </summary>
    HTCAPTION = 2,

    /// <summary>
    /// In a window menu or in a Close button in a child window.
    /// </summary>
    HTSYSMENU = 3,

    /// <summary>
    /// In a size box (same as HTGROWBOX).
    /// </summary>
    HTGROWBOX = 4,

    /// <summary>
    /// In a size box (same as HTGROWBOX).
    /// </summary>
    HTSIZE = 4,

    /// <summary>
    /// In a menu.
    /// </summary>
    HTMENU = 5,

    /// <summary>
    /// In a horizontal scroll bar.
    /// </summary>
    HTHSCROLL = 6,

    /// <summary>
    /// In the vertical scroll bar.
    /// </summary>
    HTVSCROLL = 7,

    /// <summary>
    /// In a Minimize button.
    /// </summary>
    HTMINBUTTON = 8,

    /// <summary>
    /// In a Maximize button.
    /// </summary>
    HTMAXBUTTON = 9,

    /// <summary>
    /// In the left border of a resizable window.
    /// </summary>
    HTLEFT = 10,

    /// <summary>
    /// In the right border of a resizable window.
    /// </summary>
    HTRIGHT = 11,

    /// <summary>
    /// In the upper-horizontal border of a window.
    /// </summary>
    HTTOP = 12,

    /// <summary>
    /// In the upper-left corner of a window border.
    /// </summary>
    HTTOPLEFT = 13,

    /// <summary>
    /// In the upper-right corner of a window border.
    /// </summary>
    HTTOPRIGHT = 14,

    /// <summary>
    /// In the lower-horizontal border of a resizable window.
    /// </summary>
    HTBOTTOM = 15,

    /// <summary>
    /// In the lower-left corner of a border of a resizable window.
    /// </summary>
    HTBOTTOMLEFT = 16,

    /// <summary>
    /// In the lower-right corner of a border of a resizable window.
    /// </summary>
    HTBOTTOMRIGHT = 17,

    /// <summary>
    /// In the border of a window that does not have a sizing border.
    /// </summary>
    HTBORDER = 18,

    /// <summary>
    /// In a Close button.
    /// </summary>
    HTCLOSE = 20,

    /// <summary>
    /// In a Help button.
    /// </summary>
    HTHELP = 21
}

