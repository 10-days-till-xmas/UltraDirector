using System.Diagnostics;
using System.IO;

namespace UltraDirector;

public static class Remux
{
    private const string ffmpegPath = "ffmpeg/ffmpeg.exe";
    public static void ToMp4(string inputPath, string? outputPath = null)
    {
        if (!File.Exists(inputPath))
            throw new FileNotFoundException(inputPath);
        if (Path.GetExtension(inputPath) == ".mp4") return;
        outputPath ??= inputPath;
        outputPath = Path.ChangeExtension(outputPath, ".mp4");
        var process = new Process
        {
            StartInfo =
            {
                FileName = Path.Combine(Plugin.Directory, ffmpegPath),
                Arguments = $"-i {inputPath} -c copy {outputPath}",
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };
        process.OutputDataReceived += static (_, args) => LogInfo(args.Data);
        process.Exited += static (_, _) => LogInfo("Finished processing");
        process.Start();
    }
}