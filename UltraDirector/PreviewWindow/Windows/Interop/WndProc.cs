using System;

namespace UltraDirector.PreviewWindow.Windows.Interop;

internal delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);