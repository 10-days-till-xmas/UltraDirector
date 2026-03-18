#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UltraDirector.PreviewWindow.Windows.Interop;

namespace UltraDirector.PreviewWindow.Windows;

internal sealed unsafe class Win32Window
{
    private readonly IntPtr hInstance;
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly WndProc WinProc;
    private WNDCLASSEX wc;
    private IntPtr hwnd;
    private readonly IntPtr hCursor;
    private bool _isMouseOver;

    // Texture data
    private byte[]? _textureData;
    private int _textureWidth;
    private int _textureHeight;
    private GCHandle _textureHandle;

    private const string WindowClassName = "UnityWindowClass";

    private bool _closedEventRaised;

    public bool IsClosed { get; private set; }
    public event Action? Closed;
    public Win32Window()
    {
        hInstance = User32.GetModuleHandle(null!);
        WinProc = WindowProc;
        hCursor = User32.LoadCursor(IntPtr.Zero, User32.IDC_ARROW);
        wc = new WNDCLASSEX
        {
            cbSize = sizeof(WNDCLASSEX),
            style = ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw,
            lpfnWndProc = Marshal.GetFunctionPointerForDelegate(WinProc),
            cbClsExtra = 0,
            cbWndExtra = 0,
            hInstance = hInstance,
            hIcon = (nint)1,
            hCursor = hCursor,
            hbrBackground = IntPtr.Zero,
            lpszMenuName = null!,
            lpszClassName = WindowClassName,
            hIconSm = IntPtr.Zero
        };
    }

    /// <summary>
    /// Sets the texture data to be displayed in the window.
    /// </summary>
    /// <param name="bgra32Data">The texture data in BGRA32 format (Blue, Green, Red, Alpha - 4 bytes per pixel).</param>
    /// <param name="width">The width of the texture in pixels.</param>
    /// <param name="height">The height of the texture in pixels.</param>
    /// <remarks>
    /// The byte array must be in BGRA format, not RGBA. If your source is RGBA, swap the R and B channels before calling this method.
    /// Call <see cref="Invalidate"/> after setting the texture to trigger a repaint.
    /// </remarks>
    public void SetTexture(byte[] bgra32Data, int width, int height)
    {
        if (IsInvalid) throw new InvalidOperationException("Window is not created or has been closed.");
        // Free previous handle if exists
        if (_textureHandle.IsAllocated)
            _textureHandle.Free();

        _textureData = bgra32Data;
        _textureWidth = width;
        _textureHeight = height;

        // Pin the array to prevent GC from moving it
        _textureHandle = GCHandle.Alloc(_textureData, GCHandleType.Pinned);
    }

    /// <summary>
    /// Invalidates the entire client area of the window, causing it to be repainted.
    /// </summary>
    public void Invalidate()
    {
        if (IsInvalid)
            return;

        User32.InvalidateRect(hwnd, IntPtr.Zero, false);
    }

    public bool IsWindowCreated => hwnd != IntPtr.Zero && !IsClosed && User32.IsWindow(hwnd);
    private bool IsInvalid => !IsWindowCreated;
    public void CreateWindow(WindowConfig config)
    {
        var atom = User32.RegisterClassEx(ref wc);
        if (atom == 0)
        {
            var lastError = Marshal.GetLastWin32Error();
            throw new Win32Exception(lastError);
        }
        hwnd = User32.CreateWindowEx(
            dwExStyle: 0,
            lpClassName: WindowClassName,
            lpWindowName: config.WindowTitle,
            dwStyle: (WindowStyles)config.Style,
            x: config.X, y: config.Y,
            nWidth: config.Width, nHeight: config.Height,
            hWndParent: IntPtr.Zero,
            hMenu: IntPtr.Zero,
            hInstance: hInstance,
            lpParam: IntPtr.Zero);

        if (hwnd == IntPtr.Zero)
        {
            var lastError = Marshal.GetLastWin32Error();
            throw new Win32Exception(lastError);
        }

        User32.ShowWindow(hwnd, ShowWindowCommand.Show);
        User32.UpdateWindow(hwnd);
    }

    private IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch ((WM)msg)
        {
            case WM.MOUSEMOVE:
                if (!_isMouseOver)
                {
                    _isMouseOver = true;

                    // Start tracking mouse leave
                    var tme = new TRACKMOUSEEVENT
                    {
                        cbSize = (uint)Marshal.SizeOf<TRACKMOUSEEVENT>(),
                        dwFlags = TrackMouseEventFlags.TME_LEAVE,
                        hwndTrack = hWnd,
                        dwHoverTime = 0
                    };
                    User32.TrackMouseEvent(ref tme);

                    // Force cursor to be visible - keep incrementing until visible
                    while (User32.ShowCursor(true) < 0) ;
                }
                break;

            case WM.MOUSELEAVE:
                if (_isMouseOver)
                {
                    _isMouseOver = false;

                    // Restore cursor visibility state (decrement what we incremented)
                    while (User32.ShowCursor(false) >= 0) { }
                }
                break;

            case WM.SETCURSOR:
                // Check if we're in the client area (LOWORD of lParam == HTCLIENT)
                if ((lParam.ToInt64() & 0xFFFF) == (int)HitTestResult.HTCLIENT)
                {
                    User32.SetCursor(hCursor);
                    return new IntPtr(1); // Return TRUE to indicate we handled it
                }
                break;

            case WM.PAINT:
                OnPaint(hWnd);
                return IntPtr.Zero;

            case WM.CLOSE:
                IsClosed = true;
                RaiseClosedOnce();
                User32.DestroyWindow(hWnd);
                return IntPtr.Zero;

            case WM.DESTROY:
                IsClosed = true;
                RaiseClosedOnce();

                // Restore cursor visibility before destroying
                if (_isMouseOver)
                {
                    while (User32.ShowCursor(false) >= 0) { }
                }
                // Free the texture handle when window is destroyed
                if (_textureHandle.IsAllocated)
                    _textureHandle.Free();

                hwnd = IntPtr.Zero;
                User32.PostQuitMessage(0);
                return IntPtr.Zero;
        }

        return User32.DefWindowProc(hWnd, msg, wParam, lParam);
    }

    private void RaiseClosedOnce()
    {
        if (_closedEventRaised)
            return;

        _closedEventRaised = true;
        Closed?.Invoke();
    }

    private void OnPaint(IntPtr hWnd)
    {
        var hdc = User32.BeginPaint(hWnd, out var paintstruct);

        if (_textureData != null && _textureWidth > 0 && _textureHeight > 0 && _textureHandle.IsAllocated)
        {
            User32.GetClientRect(hWnd, out var clientRect);

            var bmi = new BITMAPINFO
            {
                bmiHeader = new BITMAPINFOHEADER
                {
                    biSize = sizeof(BITMAPINFOHEADER),
                    biWidth = _textureWidth,
                    biHeight = -_textureHeight, // Negative for top-down DIB
                    biPlanes = 1,
                    biBitCount = 32, // BGRA
                    biCompression = BITMAPINFOHEADER.BI_RGB,
                    biSizeImage = 0,
                    biXPelsPerMeter = 0,
                    biYPelsPerMeter = 0,
                    biClrUsed = 0,
                    biClrImportant = 0
                }
            };

            // Blit the texture to the window, stretching to fit the client area
            Gdi32.StretchDIBits(
                hdc,
                0, 0, clientRect.Width, clientRect.Height, // Destination
                0, 0, _textureWidth, _textureHeight,       // Source
                _textureHandle.AddrOfPinnedObject(),
                ref bmi,
                Gdi32.DIB_RGB_COLORS,
                Gdi32.SRCCOPY);
        }
        else
        {
            // Fill with a default background color if no texture is set
            User32.GetClientRect(hWnd, out var rect);
            var brush = Gdi32.CreateSolidBrush(0x00404040); // Dark gray
            User32.FillRect(hdc, ref rect, brush);
            Gdi32.DeleteObject(brush);
        }

        User32.EndPaint(hWnd, ref paintstruct);
    }

    public void DestroyWindow() => User32.DestroyWindow(hwnd);

    public void HideWindow()
    {
        if (IsInvalid) return;
        User32.ShowWindow(hwnd, ShowWindowCommand.Hide);
    }
}