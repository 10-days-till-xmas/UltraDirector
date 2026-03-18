using System.Runtime.InteropServices;

namespace UltraDirector.PreviewWindow.Windows.Interop;

/// <summary>
/// Contains information about the dimensions and color format of a device-independent bitmap (DIB).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct BITMAPINFO
{
    /// <summary>
    /// A BITMAPINFOHEADER structure that contains information about the dimensions and color format of the DIB.
    /// </summary>
    public BITMAPINFOHEADER bmiHeader;
}

/// <summary>
/// Contains information about the dimensions and color format of a device-independent bitmap (DIB).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct BITMAPINFOHEADER
{
    /// <summary>
    /// The number of bytes required by the structure. Must be set to <c>sizeof(BITMAPINFOHEADER)</c>.
    /// </summary>
    public int biSize;

    /// <summary>
    /// The width of the bitmap, in pixels.
    /// </summary>
    public int biWidth;

    /// <summary>
    /// The height of the bitmap, in pixels. If positive, the bitmap is bottom-up. If negative, the bitmap is top-down.
    /// </summary>
    public int biHeight;

    /// <summary>
    /// The number of planes for the target device. Must be set to 1.
    /// </summary>
    public short biPlanes;

    /// <summary>
    /// The number of bits per pixel. For uncompressed formats, common values are 24 (RGB) or 32 (RGBA/BGRA).
    /// </summary>
    public short biBitCount;

    /// <summary>
    /// The type of compression. Use BI_RGB (0) for uncompressed bitmaps.
    /// </summary>
    public int biCompression;

    /// <summary>
    /// The size, in bytes, of the image. Can be set to 0 for BI_RGB bitmaps.
    /// </summary>
    public int biSizeImage;

    /// <summary>
    /// The horizontal resolution, in pixels-per-meter, of the target device.
    /// </summary>
    public int biXPelsPerMeter;

    /// <summary>
    /// The vertical resolution, in pixels-per-meter, of the target device.
    /// </summary>
    public int biYPelsPerMeter;

    /// <summary>
    /// The number of color indexes in the color table that are actually used by the bitmap.
    /// </summary>
    public int biClrUsed;

    /// <summary>
    /// The number of color indexes that are required for displaying the bitmap. If zero, all colors are required.
    /// </summary>
    public int biClrImportant;

    /// <summary>
    /// Compression type: uncompressed RGB.
    /// </summary>
    public const int BI_RGB = 0;
}
