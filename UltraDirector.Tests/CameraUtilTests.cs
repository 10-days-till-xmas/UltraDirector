using UltraDirector.PreviewWindow;

namespace UltraDirector.Tests;

public sealed class CameraUtilTests
{
    public static TheoryData<byte[], int, int, byte[]> ConvertRgbaToBgraTestData => new()
    {
        // 2x2 image with distinct 0/255-heavy colors
        {
            [
                // Row 1 (top)
                255, 0, 0, 255, // Red
                0, 255, 0, 255, // Green
                // Row 2 (bottom)
                0, 0, 255, 255,   // Blue
                255, 255, 0, 255  // Yellow
            ],
            2,
            2,
            [
                // Row 1 (top) <- source bottom row, RGBA->BGRA
                255, 0, 0, 255,   // Blue
                0, 255, 255, 255, // Yellow
                // Row 2 (bottom) <- source top row
                0, 0, 255, 255,   // Red
                0, 255, 0, 255    // Green
            ]
        },

        // 1x1 with non-binary channels
        {
            [128, 64, 32, 200],
            1,
            1,
            [32, 64, 128, 200]
        },

        // 3x1 row, mixed values (flip does nothing for height=1)
        {
            [
                10, 20, 30, 40,
                50, 60, 70, 80,
                90, 100, 110, 120
            ],
            3,
            1,
            [
                30, 20, 10, 40,
                70, 60, 50, 80,
                110, 100, 90, 120
            ]
        },

        // 1x3 column, tests vertical flip clearly with varied channels
        {
            [
                // Row 1 (top)
                1, 2, 3, 4,
                // Row 2
                11, 22, 33, 44,
                // Row 3 (bottom)
                101, 102, 103, 104
            ],
            1,
            3,
            [
                // top <- old bottom
                103, 102, 101, 104,
                // middle
                33, 22, 11, 44,
                // bottom <- old top
                3, 2, 1, 4
            ]
        },

        // 2x3 with mixed + some 0/255 values
        {
            [
                // Row 1 (top)
                0, 1, 2, 3, 4, 5, 6, 7,
                // Row 2
                8, 9, 10, 11, 12, 13, 14, 15,
                // Row 3 (bottom)
                250, 251, 252, 253, 254, 255, 0, 1
            ],
            2,
            3,
            [
                // Row 1 <- old Row 3
                252, 251, 250, 253, 0, 255, 254, 1,
                // Row 2 <- old Row 2
                10, 9, 8, 11, 14, 13, 12, 15,
                // Row 3 <- old Row 1
                2, 1, 0, 3, 6, 5, 4, 7
            ]
        },

        // 4x1 includes alpha extremes and non-extremes
        {
            [
                255, 128, 64, 0,
                0, 64, 128, 255,
                12, 34, 56, 78,
                200, 150, 100, 50
            ],
            4,
            1,
            [
                64, 128, 255, 0,
                128, 64, 0, 255,
                56, 34, 12, 78,
                100, 150, 200, 50
            ]
        }
    };

    [Theory]
    [MemberData(nameof(ConvertRgbaToBgraTestData))]
    public void ConvertRgbaToBgra_ConvertsAndFlipsCorrectly(byte[] rgbaInput, int width, int height, byte[] expectedBgra)
    {
        Span<byte> rgba = rgbaInput;
        var actual = new byte[width * height * 4];

        CameraUtil.ConvertRgbaToBgra(ref rgba, ref actual, width, height);

        Assert.Equal(expectedBgra, actual);
    }
}