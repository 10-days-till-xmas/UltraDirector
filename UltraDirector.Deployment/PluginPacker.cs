using System.Xml.Linq;
using Path = System.IO.Path;

namespace UltraDirector.Deployment;

public sealed class PluginPacker(PluginInfo pluginInfo)
{
    private DirectoryPath projectDir => pluginInfo.projectDir;
    public static readonly DirectoryPath TempDir = DirectoryPath.FromString(Path.GetTempPath()).Combine($"Cake_{Guid.NewGuid().ToString()[..4]}/");
    private readonly DirectoryPath tempDir = TempDir.Combine(pluginInfo.pluginProject.Name);
    private FilePath csprojFilePath => pluginInfo.csprojFilePath;

    public async Task Pack()
    {
        if (!DirectoryExists(tempDir)) throw new DirectoryNotFoundException($"Directory {TempDir} does not exist");
        var iconFile = projectDir.CombineWithFilePath("icon.png");
        if (!FileExists(iconFile)) throw new FileNotFoundException($"Icon file not found at {iconFile}");
        var readmeFile = projectDir.CombineWithFilePath("README.md");
        if (!FileExists(readmeFile)) throw new FileNotFoundException($"Readme file not found at {readmeFile}");
        if (HasBom(readmeFile.FullPath)) Warning($"Readme file {readmeFile} has a BOM, which may cause issues with some parsers. Consider removing the BOM.");
        CopyFileToDirectory(iconFile, tempDir);
        CopyFileToDirectory(readmeFile, tempDir);
        SerializeJsonToPrettyFile(tempDir.CombineWithFilePath("manifest.json"), await ReadCsproj());
        Context.SerializeJsonToPrettyFile(tempDir.CombineWithFilePath("manifest.json"), await ReadCsproj());
        var packageDir = (DirectoryPath)"./packages/";
        CreateDirectory(packageDir);
        var package = packageDir.CombineWithFilePath($"{pluginInfo.pluginProject.Name}.zip");
        Zip(tempDir, package);
        Information("Packed plugin {0} to {1}",
            pluginInfo.pluginProject.Name,
            MakeAbsolute(package).FullPath);
    }

    private async Task<JObject> ReadCsproj(CancellationToken ct = default)
    {
        await using var csprojFile = Context.FileSystem.GetFile(csprojFilePath).OpenRead();
        var doc = await XDocument.LoadAsync(csprojFile, LoadOptions.None, ct);
        var ns = doc.Root!.Name.Namespace;
        var imports = doc.Descendants(ns + "Import");
        // merge imports
        foreach (var importPath in imports.Select(static import => import.Attribute("Project")?.Value)
                                          .OfType<string>()
                                          .Select(projPath => Path.GetFullPath(projPath, csprojFilePath.GetDirectory().FullPath)))
        {
            await using var importFile = Context.FileSystem.GetFile(importPath).OpenRead();
            var importDoc = await XDocument.LoadAsync(importFile, LoadOptions.None, ct);
            doc.Root.Add(importDoc.Root!.Elements());
        }
        var properties = doc.Root.Descendants(ns + "PropertyGroup")
                            .SelectMany(pg => pg.Elements())
                            .GroupBy(e => e.Name.LocalName)
                            .ToDictionary(g => g.Key, g => g.First().Value); // could be optimized but idc
        var name = (properties.GetValueOrDefault("Product") ??
                    properties.GetValueOrDefault("AssemblyName") ??
                    properties.GetValueOrDefault("RootNamespace") ??
                    throw new Exception("Name of plugin could not be determined"));

        var version = properties.GetValueOrDefault("Version", "0.1.0");
        var description = properties.GetValueOrDefault("Description", "");

        var dependencies = doc.Root.Descendants(ns + "ItemGroup")
                              .SelectMany(pg => pg.Elements("ThunderDependency"))
                              .Select(e => e.Attribute("Include")?.Value)
                              .Where(static p => !string.IsNullOrWhiteSpace(p));
        var manifest = new JObject
        {
            { "name", name },
            { "description", description },
            { "version_number", version },
            { "website_url", ""},
            { "dependencies", new JArray(dependencies) }
        };
        return manifest;
    }

    private static bool HasBom(string filePath)
    {
        var buffer = new byte[4];
        using var fs = Context.FileSystem.GetFile(filePath).OpenRead();

        var read = fs.Read(buffer, 0, buffer.Length);
        return buffer.AsSpan()[..read] switch
        {
            [0xEF, 0xBB, 0xBF] => true, // UTF-8 BOM: EF BB BF
            [0xFF, 0xFE] or [0xFE, 0xFF]  => true, // UTF-16 LE/BE BOM: FF FE / FE FF
            [0xFF, 0xFE, 0x00, 0x00] or [0x00, 0x00, 0xFE, 0xFF] => true, // UTF-32 LE/BE BOM: FF FE 00 00 / 00 00 FE FF
            _ => false
        };
    }
}