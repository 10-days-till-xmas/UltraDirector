using System;
using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

/// <summary>
/// Represents the extended window class information structure used in Windows interoperability.
/// This structure is used with the RegisterClassEx Win32 API function to define window class attributes.
/// </summary>
/// <remarks>
/// The WNDCLASSEX structure contains the window class attributes that are registered with the system.
/// This is the extended version of WNDCLASS and includes an additional hIconSm field for small icons.
/// The struct uses sequential layout to match the native Windows WNDCLASSEX structure.
/// </remarks>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct WNDCLASSEX
{
    /// <summary>
    /// Gets or sets the size of the structure in bytes. This must be set to the size of WNDCLASSEX before passing to RegisterClassEx.
    /// </summary>
    [MarshalAs(UnmanagedType.U4)]
    public int cbSize;

    /// <summary>
    /// Gets or sets the class style flags. These flags define the appearance and behavior of windows of this class.
    /// Common values include CS_VREDRAW, CS_HREDRAW, CS_DBLCLKS, etc.
    /// </summary>
    [MarshalAs(UnmanagedType.U4)]
    public ClassStyles style;

    /// <summary>
    /// Gets or sets a pointer to the window procedure (WndProc) callback function.
    /// This function processes messages sent to windows of this class.
    /// Note: Use WndProcDelegate, not WndProc directly. Be careful with this field.
    /// </summary>
    public IntPtr lpfnWndProc; // not WndProc -- careful

    /// <summary>
    /// Gets or sets the number of extra bytes to allocate for the window class structure.
    /// This memory is available to the application for class-specific data.
    /// </summary>
    public int cbClsExtra;

    /// <summary>
    /// Gets or sets the number of extra bytes to allocate for each window instance.
    /// This memory is available to the application for window-specific data.
    /// </summary>
    public int cbWndExtra;

    /// <summary>
    /// Gets or sets a handle to the instance of the application that registers the window class.
    /// This is typically obtained from the module instance handle.
    /// </summary>
    public IntPtr hInstance;

    /// <summary>
    /// Gets or sets a handle to the class icon displayed in the title bar and taskbar.
    /// If this field is NULL, the system uses a default icon.
    /// </summary>
    public IntPtr hIcon;

    /// <summary>
    /// Gets or sets a handle to the cursor displayed when the mouse is over a window of this class.
    /// If this field is NULL, the application must set the cursor in response to WM_SETCURSOR messages.
    /// </summary>
    public IntPtr hCursor;

    /// <summary>
    /// Gets or sets a handle to the brush used to paint the background of the window.
    /// If this field is NULL, the application is responsible for painting the window background.
    /// </summary>
    public IntPtr hbrBackground;

    /// <summary>
    /// Gets or sets the name of the menu resource associated with the window class.
    /// This menu is displayed when the user right-clicks on the window.
    /// </summary>
    public string lpszMenuName;

    /// <summary>
    /// Gets or sets the name of the window class.
    /// This name is used when creating windows of this class with CreateWindow or CreateWindowEx.
    /// </summary>
    public string lpszClassName;

    /// <summary>
    /// Gets or sets a handle to a small icon (16x16) associated with the window class.
    /// This icon is displayed in the title bar and taskbar of windows of this class.
    /// If this field is NULL, the system uses a small version of the hIcon.
    /// </summary>
    public IntPtr hIconSm;
}