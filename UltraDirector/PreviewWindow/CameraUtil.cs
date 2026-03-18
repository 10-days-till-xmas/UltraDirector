using System;

namespace UltraDirector.PreviewWindow;

public static class CameraUtil
{
    /// <summary>
    /// Converts pixel data from RGBA format to BGRA format by swapping R and B channels.
    /// Also flips the image vertically since Unity's coordinate system is bottom-up.
    /// </summary>
    public static void ConvertRgbaToBgra(ref Span<byte> rgba, ref byte[] bgra, int width, int height)
    {
        var rowSize = width * 4;

        for (var y = 0; y < height; y++)
        {
            // Flip vertically: read from bottom, write to top
            var srcRow = (height - 1 - y) * rowSize;
            var dstRow = y * rowSize;

            for (var x = 0; x < width; x++)
            {
                var srcIdx = srcRow + (x * 4);
                var dstIdx = dstRow + (x * 4);
                // perhaps i could do this more efficiently by using SIMD instructions, but for now this is straightforward and easy to understand

                // Swap R and B, keep G and A in place
                bgra[dstIdx + 0] = rgba[srcIdx + 2]; // B <- R
                bgra[dstIdx + 1] = rgba[srcIdx + 1]; // G <- G
                bgra[dstIdx + 2] = rgba[srcIdx + 0]; // R <- B
                bgra[dstIdx + 3] = rgba[srcIdx + 3]; // A <- A
            }
        }
    }
}