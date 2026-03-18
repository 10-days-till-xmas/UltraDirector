using System;
using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

/// <summary>
/// Contains P/Invoke declarations for GDI32.dll functions used for graphics operations.
/// </summary>
internal static class Gdi32
{
    private const string GDI32_DLL = "gdi32.dll";

    /// <summary>
    /// Copies the color data for a rectangle of pixels in a DIB, JPEG, or PNG image to the specified destination rectangle.
    /// </summary>
    /// <param name="hdc">A handle to the destination device context.</param>
    /// <param name="xDest">The x-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
    /// <param name="yDest">The y-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
    /// <param name="DestWidth">The width, in logical units, of the destination rectangle.</param>
    /// <param name="DestHeight">The height, in logical units, of the destination rectangle.</param>
    /// <param name="xSrc">The x-coordinate, in pixels, of the source rectangle in the image.</param>
    /// <param name="ySrc">The y-coordinate, in pixels, of the source rectangle in the image.</param>
    /// <param name="SrcWidth">The width, in pixels, of the source rectangle in the image.</param>
    /// <param name="SrcHeight">The height, in pixels, of the source rectangle in the image.</param>
    /// <param name="lpBits">A pointer to the image bits, which are stored as an array of bytes.</param>
    /// <param name="lpbmi">A pointer to a BITMAPINFO structure that contains information about the DIB.</param>
    /// <param name="iUsage">Specifies whether the bmiColors member of the BITMAPINFO structure was provided. Use DIB_RGB_COLORS (0).</param>
    /// <param name="rop">A raster-operation code. Use SRCCOPY (0x00CC0020) to copy source directly to destination.</param>
    /// <returns>If the function succeeds, the return value is the number of scan lines copied. If the function fails, the return value is zero.</returns>
    [DllImport(GDI32_DLL)]
    public static extern int StretchDIBits(
        IntPtr hdc,
        int xDest, int yDest, int DestWidth, int DestHeight,
        int xSrc, int ySrc, int SrcWidth, int SrcHeight,
        IntPtr lpBits,
        ref BITMAPINFO lpbmi,
        uint iUsage,
        uint rop);

    /// <summary>
    /// Creates a logical brush that has the specified solid color.
    /// </summary>
    /// <param name="crColor">The color of the brush in BGR format (0x00BBGGRR).</param>
    /// <returns>If the function succeeds, the return value identifies a logical brush. If the function fails, the return value is NULL.</returns>
    [DllImport(GDI32_DLL)]
    public static extern IntPtr CreateSolidBrush(uint crColor);

    /// <summary>
    /// Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object.
    /// </summary>
    /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
    /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
    [DllImport(GDI32_DLL)]
    public static extern bool DeleteObject(IntPtr hObject);

    /// <summary>
    /// Raster operation code for SRCCOPY - copies the source rectangle directly to the destination rectangle.
    /// </summary>
    public const uint SRCCOPY = 0x00CC0020;

    /// <summary>
    /// Specifies that the color table contains literal RGB values.
    /// </summary>
    public const uint DIB_RGB_COLORS = 0;
}

