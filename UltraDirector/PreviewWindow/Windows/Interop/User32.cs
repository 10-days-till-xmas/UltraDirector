using System;
using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

internal static class User32
{
    private const string USER32_DLL = "user32.dll";

    /// <summary>
    /// Represents the window procedure callback function signature.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="msg">The message identifier. It can be any integer value.</param>
    /// <param name="wParam">Additional message information. The exact meaning depends on the message code.</param>
    /// <param name="lParam">Additional message information. The exact meaning depends on the message code.</param>
    /// <returns>The return value is the result of the message processing and depends on the message sent. If you process the message, you should return zero. If you do not handle the message, you should call DefWindowProc and return its result.</returns>
    /// <remarks>
    /// A window procedure is a function that processes messages sent to a window. Each window class is associated with a window procedure.
    /// The window procedure must not make any blocking calls, such as Wait or SendMessage to a window in another process.
    /// </remarks>
    public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// The CreateWindowEx function creates an overlapped, pop-up, or child window with an extended window style; otherwise, this function is identical to the CreateWindow function.
    /// </summary>
    /// <param name="dwExStyle">Specifies the extended window style of the window being created.</param>
    /// <param name="lpClassName">Pointer to a null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero. If lpClassName is a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, provided that the module that registers the class is also the module that creates the window. The class name can also be any of the predefined system class names.</param>
    /// <param name="lpWindowName">Pointer to a null-terminated string that specifies the window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar. When using CreateWindow to create controls, such as buttons, check boxes, and static controls, use lpWindowName to specify the text of the control. When creating a static control with the SS_ICON style, use lpWindowName to specify the icon name or identifier. To specify an identifier, use the syntax "#num". </param>
    /// <param name="dwStyle">Specifies the style of the window being created. This parameter can be a combination of window styles, plus the control styles indicated in the Remarks section.</param>
    /// <param name="x">Specifies the initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates. For a child window, x is the x-coordinate of the upper-left corner of the window relative to the upper-left corner of the parent window's client area. If x is set to CW_USEDEFAULT, the system selects the default position for the window's upper-left corner and ignores the y parameter. CW_USEDEFAULT is valid only for overlapped windows; if it is specified for a pop-up or child window, the x and y parameters are set to zero.</param>
    /// <param name="y">Specifies the initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates. For a child window, y is the initial y-coordinate of the upper-left corner of the child window relative to the upper-left corner of the parent window's client area. For a list box y is the initial y-coordinate of the upper-left corner of the list box's client area relative to the upper-left corner of the parent window's client area.
    /// <para>If an overlapped window is created with the WS_VISIBLE style bit set and the x parameter is set to CW_USEDEFAULT, then the y parameter determines how the window is shown. If the y parameter is CW_USEDEFAULT, then the window manager calls ShowWindow with the SW_SHOW flag after the window has been created. If the y parameter is some other value, then the window manager calls ShowWindow with that value as the nCmdShow parameter.</para></param>
    /// <param name="nWidth">Specifies the width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT. If nWidth is CW_USEDEFAULT, the system selects a default width and height for the window; the default width extends from the initial x-coordinates to the right edge of the screen; the default height extends from the initial y-coordinate to the top of the icon area. CW_USEDEFAULT is valid only for overlapped windows; if CW_USEDEFAULT is specified for a pop-up or child window, the nWidth and nHeight parameter are set to zero.</param>
    /// <param name="nHeight">Specifies the height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param> <param name="hWndParent">Handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.
    /// <para>Windows 2000/XP: To create a message-only window, supply HWND_MESSAGE or a handle to an existing message-only window.</para></param>
    /// <param name="hMenu">Handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used. For a child window, hMenu specifies the child-window identifier, an integer value used by a dialog box control to notify its parent about events. The application determines the child-window identifier; it must be unique for all child windows with the same parent window.</param>
    /// <param name="hInstance">Handle to the instance of the module to be associated with the window.</param> <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.
    /// <para>If an application calls CreateWindow to create a MDI client window, lpParam should point to a CLIENTCREATESTRUCT structure. If an MDI client window calls CreateWindow to create an MDI child window, lpParam should point to a MDICREATESTRUCT structure. lpParam may be NULL if no additional data is needed.</para></param>
    /// <returns>If the function succeeds, the return value is a handle to the new window.
    /// <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
    /// <para>This function typically fails for one of the following reasons:</para>
    /// <list type="">
    /// <item>an invalid parameter value</item>
    /// <item>the system class was registered by a different module</item>
    /// <item>The WH_CBT hook is installed and returns a failure code</item>
    /// <item>if one of the controls in the dialog template is not registered, or its window window procedure fails WM_CREATE or WM_NCCREATE</item>
    /// </list></returns>
    [DllImport(USER32_DLL, SetLastError=true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateWindowEx(
       WindowStylesEx dwExStyle,
       string lpClassName,
       string lpWindowName,
       WindowStyles dwStyle,
       int x,
       int y,
       int nWidth,
       int nHeight,
       IntPtr hWndParent,
       IntPtr hMenu,
       IntPtr hInstance,
       IntPtr lpParam);

    /// <summary>
    /// Registers a window class for subsequent use in calls to the CreateWindow or CreateWindowEx function.
    /// </summary>
    /// <param name="lpwcx">A reference to a WNDCLASSEX structure. You must fill the structure with the appropriate class attributes before passing it to the function.</param>
    /// <returns>If the function succeeds, the return value is a class atom that uniquely identifies the class being registered. This atom can only be used by the CreateWindow and CreateWindowEx functions to create windows of this class.
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
    /// <remarks>
    /// Before calling CreateWindow, you must call RegisterClassEx to register the window class. The system maintains a global list of registered window classes.
    /// Window class names are case-insensitive.
    /// </remarks>
    [DllImport(USER32_DLL, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

    /// <summary>
    /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
    /// </summary>
    /// <param name="hWnd">A handle to the window procedure that received the message.</param>
    /// <param name="uMsg">The message identifier.</param>
    /// <param name="wParam">Additional message information. The content of this parameter depends on the value of the uMsg parameter.</param>
    /// <param name="lParam">Additional message information. The content of this parameter depends on the value of the uMsg parameter.</param>
    /// <returns>The return value is the result of the message processing and depends on the message sent.</returns>
    /// <remarks>
    /// When a window procedure does not handle a message, it should call DefWindowProc to ensure that the message is processed correctly.
    /// </remarks>
    [DllImport(USER32_DLL, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Sets the specified window's show state.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="nCmdShow">Controls how the window is to be shown. This parameter is ignored the first time an application calls ShowWindow if the program that launched the application provides a STARTUPINFO structure. Otherwise, the first time ShowWindow is called, the value should be the value obtained by the WinMain function in its nCmdShow parameter. In subsequent calls, this parameter can be one of the ShowWindowCommand values.</param>
    /// <returns>If the window was previously visible, the return value is nonzero. If the window was previously hidden, the return value is zero.</returns>
    /// <remarks>
    /// To perform certain special effects, use AnimateWindow.
    /// The first time an application calls ShowWindow, it should use the WinMain function's nCmdShow parameter as the nCmdShow parameter. Subsequent calls to ShowWindow must use one of the values in the ShowWindowCommand enumeration, instead of the one specified by the WinMain function's nCmdShow parameter.
    /// </remarks>
    [DllImport(USER32_DLL, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport(USER32_DLL, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool ShowWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.I4)] ShowWindowCommand nCmdShow);

    /// <summary>
    /// Updates the client area of the specified window by sending a WM_PAINT message to the window if any part of the window is marked invalid (the update region is not empty).
    /// </summary>
    /// <param name="hWnd">Handle to the window to be updated.</param>
    /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
    /// <remarks>
    /// The UpdateWindow function sends a WM_PAINT message directly, bypassing the application queue. If the update region is empty, UpdateWindow does not send a WM_PAINT message.
    /// </remarks>
    [DllImport(USER32_DLL, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool UpdateWindow(IntPtr hWnd);

    /// <summary>
    /// Retrieves a module handle for the specified module.
    /// </summary>
    /// <param name="lpModuleName">The name of the loaded module (either a .dll or .exe file). If the file name extension is omitted, the default library extension .dll is appended. The file name string can include a trailing point (.) to indicate that the module name has no extension. The string does not include the file extension. To get the handle of modules that were loaded by the system, such as those listed in the Device Drivers section of the registry under HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services, specify NULL for lpModuleName and use the returned handle in subsequent API calls to reference the system driver.
    /// <para>If this parameter is NULL, the function returns a handle to the file used to create the calling process.</para></param>
    /// <returns>If the function succeeds, the return value is a handle to the specified module. If the function fails, the return value is NULL. To get extended error information, call GetLastError.</returns>
    /// <remarks>
    /// The returned handle is not global or inheritable. It cannot be duplicated or used by another process.
    /// GetModuleHandle increments the reference count of the DLL by one on each successful call but does not increment the reference count if the module is already loaded in the process. Each successful call must eventually be matched by a call to FreeLibrary.
    /// </remarks>
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    /// <summary>
    /// Prepares the specified window for painting and fills a PAINTSTRUCT structure with information about the painting.
    /// </summary>
    /// <param name="hWnd">Handle to the window to be repainted.</param>
    /// <param name="lpPaint">Pointer to the PAINTSTRUCT structure that will receive painting information.</param>
    /// <returns>If the function succeeds, the return value is the handle to a display device context for the specified window.</returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern IntPtr BeginPaint(IntPtr hWnd, out PAINTSTRUCT lpPaint);

    /// <summary>
    /// Marks the end of painting in the specified window. This function is required for each call to the BeginPaint function.
    /// </summary>
    /// <param name="hWnd">Handle to the window that has been repainted.</param>
    /// <param name="lpPaint">Pointer to a PAINTSTRUCT structure that contains the painting information retrieved by BeginPaint.</param>
    /// <returns>The return value is always nonzero.</returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

    /// <summary>
    /// Adds a rectangle to the specified window's update region. The update region represents the portion of the window's client area that must be redrawn.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose update region has changed.</param>
    /// <param name="lpRect">A pointer to a RECT structure that contains the client coordinates of the rectangle to be added to the update region. If this parameter is NULL, the entire client area is added to the update region.</param>
    /// <param name="bErase">Specifies whether the background within the update region is to be erased when the update region is processed.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

    /// <summary>
    /// Fills a rectangle by using the specified brush. This function includes the left and top borders but excludes the right and bottom borders of the rectangle.
    /// </summary>
    /// <param name="hDC">A handle to the device context.</param>
    /// <param name="lprc">A pointer to a RECT structure that contains the logical coordinates of the rectangle to be filled.</param>
    /// <param name="hbr">A handle to the brush used to fill the rectangle.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern int FillRect(IntPtr hDC, ref RECT lprc, IntPtr hbr);

    /// <summary>
    /// Indicates to the system that a thread has made a request to terminate (quit). It is typically used in response to a WM_DESTROY message.
    /// </summary>
    /// <param name="nExitCode">The application exit code. This value is used as the wParam parameter of the WM_QUIT message.</param>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern void PostQuitMessage(int nExitCode);

    /// <summary>
    /// Retrieves the dimensions of the bounding rectangle of the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="lpRect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    /// <summary>
    /// Loads the specified cursor resource from the executable (.EXE) file associated with an application instance.
    /// </summary>
    /// <param name="hInstance">A handle to an instance of the module whose executable file contains the cursor to be loaded. To load a predefined system cursor, set this parameter to IntPtr.Zero.</param>
    /// <param name="lpCursorName">The cursor resource identifier. To load a predefined system cursor, use one of the IDC_ constants.</param>
    /// <returns>If the function succeeds, the return value is the handle to the newly loaded cursor. If the function fails, the return value is NULL.</returns>
    [DllImport(USER32_DLL, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

    /// <summary>
    /// Sets the cursor shape.
    /// </summary>
    /// <param name="hCursor">A handle to the cursor. The cursor must have been created by the CreateCursor function or loaded by the LoadCursor or LoadImage function.</param>
    /// <returns>The return value is the handle to the previous cursor if there was one. If there was no previous cursor, the return value is NULL.</returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern IntPtr SetCursor(IntPtr hCursor);

    /// <summary>
    /// Displays or hides the cursor.
    /// </summary>
    /// <param name="bShow">If true, the display count is incremented by one. If false, the display count is decremented by one.</param>
    /// <returns>The return value specifies the new display counter.</returns>
    /// <remarks>
    /// This function sets an internal display counter that determines whether the cursor should be displayed.
    /// The cursor is displayed only if the display count is greater than or equal to 0.
    /// </remarks>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern int ShowCursor(bool bShow);

    /// <summary>
    /// Posts messages when the mouse pointer leaves a window or hovers over a window for a specified amount of time.
    /// </summary>
    /// <param name="lpEventTrack">A pointer to a TRACKMOUSEEVENT structure that contains tracking information.</param>
    /// <returns>If the function succeeds, the return value is nonzero.</returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

    /// <summary>
    /// Determines whether the specified window handle identifies an existing window.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be tested</param>
    /// <returns>If the window handle identifies an existing window, returns <see langword="true"/></returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern bool IsWindow([In] IntPtr hWnd);

    /// <summary>
    /// Destroys the specified window. The function sends WM_DESTROY and WM_NCDESTROY messages to the window to deactivate it and remove the keyboard focus from it. The function also destroys the window's menu, flushes the thread message queue, destroys timers, removes clipboard ownership, and breaks the clipboard viewer chain (if the window is at the top of the viewer chain). If the specified window is a parent or owner window, DestroyWindow automatically destroys the associated child or owned windows.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be destroyed.</param>
    /// <returns>If the function succeeds, the return value is <see langword="true"/></returns>
    [DllImport(USER32_DLL, SetLastError = true)]
    public static extern bool DestroyWindow([In] IntPtr hWnd);

    /// <summary>
    /// Standard arrow cursor.
    /// </summary>
    public static readonly IntPtr IDC_ARROW = new((int)Win32_IDC_Constants.IDC_ARROW);

    /// <summary>
    /// I-beam cursor (text selection).
    /// </summary>
    public static readonly IntPtr IDC_IBEAM = new((int)Win32_IDC_Constants.IDC_IBEAM);

    /// <summary>
    /// Hourglass/wait cursor.
    /// </summary>
    public static readonly IntPtr IDC_WAIT = new((int)Win32_IDC_Constants.IDC_WAIT);

    /// <summary>
    /// Crosshair cursor.
    /// </summary>
    public static readonly IntPtr IDC_CROSS = new((int)Win32_IDC_Constants.IDC_CROSS);

    /// <summary>
    /// Hand cursor.
    /// </summary>
    public static readonly IntPtr IDC_HAND = new(32649);
}
