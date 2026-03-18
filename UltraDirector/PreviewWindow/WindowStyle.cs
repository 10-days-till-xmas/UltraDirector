using System;
using UltraDirector.PreviewWindow.Windows.Interop;

namespace UltraDirector.PreviewWindow;

/// <summary>
/// Specifies the style of a window. These flags can be combined to define the appearance and behavior of windows.
/// </summary>
/// <remarks>
/// Note to developers: The values are the exact same as <see cref="WindowStyles"/>
/// </remarks>
[Flags, Serializable]
public enum WindowStyle : uint
{
    /// <summary>
    /// The window can receive keyboard focus via the Tab key.
    /// </summary>
    TabStop = 1 << 16,

    /// <summary>
    /// Marks the window as the first control in a group for keyboard navigation.
    /// </summary>
    Group = 1 << 17,

    /// <summary>
    /// The window has a minimize button. Requires <see cref="SystemMenu"/> to be set.
    /// </summary>
    MinimizeBox = 1 << 17,

    /// <summary>
    /// The window has a sizing border that allows resizing.
    /// </summary>
    SizeFrame = 1 << 18,

    /// <summary>
    /// The window has a system menu on its title bar. Requires <see cref="Caption"/> to be set.
    /// </summary>
    SystemMenu = 1 << 19,

    /// <summary>
    /// The window has a horizontal scroll bar.
    /// </summary>
    HorizontalScroll = 1 << 20,

    /// <summary>
    /// The window has a dialog-style border. Cannot have a title bar.
    /// </summary>
    DialogFrame = 1 << 22,
    /// <summary>
    /// The window has a thin-line border.
    /// </summary>
    Border = 1 << 23,
    /// <summary>
    /// Excludes the area occupied by child windows when drawing within the parent window.
    /// </summary>
    ClipChildren = 1 << 25,

    /// <summary>
    /// Clips child windows relative to each other during paint operations to prevent overlapping draws.
    /// </summary>
    ClipSiblings = 1 << 26,

    /// <summary>
    /// The window is initially disabled and cannot receive user input.
    /// </summary>
    Disabled = 1 << 27,
    /// <summary>
    /// The window is a child window. Cannot have a menu bar or be used with <see cref="Popup"/>.
    /// </summary>
    Child = 1 << 30,
    /// <summary>
    /// The window has a title bar. Implicitly includes <see cref="Border"/> and <see cref="DialogFrame"/>.
    /// </summary>
    Caption = Border | DialogFrame,

    /// <summary>
    /// The window is initially maximized.
    /// </summary>
    Maximize = 1 << 24,

    /// <summary>
    /// The window has a maximize button. Requires <see cref="SystemMenu"/> to be set.
    /// </summary>
    MaximizeBox = 1 << 16,

    /// <summary>
    /// The window is initially minimized.
    /// </summary>
    Minimize = 1 << 29,



    /// <summary>
    /// The window is an overlapped window with a title bar and border.
    /// </summary>
    Overlapped = 0,

    /// <summary>
    /// A standard overlapped window with title bar, system menu, sizing border, and minimize/maximize buttons.
    /// </summary>
    OverlappedWindow = Overlapped | Caption | SystemMenu | SizeFrame | MinimizeBox | MaximizeBox,

    /// <summary>
    /// The window is a pop-up window. Cannot be used with <see cref="Child"/>.
    /// </summary>
    Popup = 1u << 31,

    /// <summary>
    /// A pop-up window with a border and system menu. Combine with <see cref="Caption"/> for a visible menu.
    /// </summary>
    PopupWindow = Popup | Border | SystemMenu,



    /// <summary>
    /// The window is initially visible.
    /// </summary>
    Visible = 1 << 28,

    /// <summary>
    /// The window has a vertical scroll bar.
    /// </summary>
    VerticalScroll = 1 << 21
}

public static class WindowStyleExtensions
{
    public static bool HasFlagFast(this WindowStyle value, WindowStyle flag)
    {
        return (value & flag) != 0;
    }
}