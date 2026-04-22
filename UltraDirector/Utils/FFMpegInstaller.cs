using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UltraDirector.Utils;

public static class FFMpegInstaller
{
    private const string DownloadUrl = "https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-n7.1-latest-win64-gpl-shared-7.1.zip";
    public static readonly string FFMpegBinDir = Path.Combine(Plugin.Directory, @"ffmpeg\");

    private static readonly HashSet<string> _dllNames =
    [
        "avcodec-61", "avformat-61", "avutil-59", "swresample-5", "swscale-8"
    ];

    public static bool CheckPresence() =>
        Directory.EnumerateFiles(FFMpegBinDir, "*.dll")
                 .Select(Path.GetFileNameWithoutExtension)
                 .ToHashSet()
                 .IsSupersetOf(_dllNames);

    public static async Task InstallAsync()
    {
        Directory.CreateDirectory(FFMpegBinDir);
        
        LogInfo("Begin GET");
        using (var http = new HttpClient())
        using (var response = await http.GetAsync(DownloadUrl))
        {
            response.EnsureSuccessStatusCode();
            LogInfo("GET complete. Beginning download to file");
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
            await zip.Entries
                     .Where(e => !string.IsNullOrEmpty(e.Name) && e.FullName.Contains("/bin/"))
                     .ForEachAsync(async entry =>
                      {
                          var destinationPath = Path.Combine(FFMpegBinDir, entry.Name);
                          if (File.Exists(destinationPath)) return;
                          await using var entryStream = entry.Open();
                          await using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
                          await entryStream.CopyToAsync(fileStream);
                      });
        }
        LogInfo("Ffmpeg installation complete.");
    }
}