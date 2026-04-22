using UltraDirector.Deployment;

#region Read Arguments
var target = Argument("target", "Default");

var configuration = Argument("configuration", "Release");
#endregion

#region Gather Plugins
var slnFile = FilePath.FromString("./UltraDirector.slnx");
var slnParsed = ParseSolution(slnFile);

var projects = slnParsed.Projects.ToDictionary(p => p.Name);
var plugins = projects.Values
                      .Where(static p => p.Type is "{PLUGIN}")
                      .Select(p =>
                       {
                           var testProj = projects.GetValueOrDefault(p.Name + ".Tests");
                           return new PluginInfo(p, testProj);
                       })
                      .ToList();
#endregion

#region Define Tasks

Setup(static _ =>
{
    CreateDirectory(PluginPacker.TempDir);
    Information($"Created temp directory at {PluginPacker.TempDir}");
});
Teardown(static _ =>
{
    DeleteDirectory(PluginPacker.TempDir, new DeleteDirectorySettings()
    {
        Force = true, Recursive = true
    });
    Information($"Deleted temp directory at {PluginPacker.TempDir}");
});

Task("Clean")
   .IsDependeeOf("Default")
   .WithCriteria(static _ => HasArgument("rebuild"))
   .Does(() => CleanDirectory($"./**/bin/{configuration}"));

foreach (var plugin in plugins)
{
    var name = plugin.pluginProject.Name;
    var buildTask = $"Build_{name}";
    var testTask = $"Test_{name}";
    var packTask = $"Pack_{name}";

    Task(buildTask)
       .IsDependentOn("Clean")
       .IsDependeeOf("Default")
       .Does(_ =>
        {
            var outDir = PluginPacker.TempDir.Combine(name);
            DotNetBuild(plugin.pluginProject.Path.FullPath, new DotNetBuildSettings
            {
                Configuration = configuration,
                OutputDirectory = outDir,
                ArgumentCustomization = args => args.Append("/p:SuppressDeployment=true")
            });
            var unusedFiles = GetFiles(outDir.FullPath + "/*", new GlobberSettings()
                {
                    FilePredicate =
                        f => f.Path.GetFilenameWithoutExtension().ToString() != plugin.pluginProject.Path.GetFilenameWithoutExtension()
                });
            DeleteFiles(unusedFiles);
        });

    Task(testTask)
       .IsDependentOn(buildTask)
       .IsDependeeOf("Default")
       .WithCriteria(() => plugin.testProject != null, "Plugin does not have a test project")
       .Does(() =>
        {
            Information("Building test project {0}", plugin.testProject!.Name);
            DotNetRun(plugin.testProject!.Path.FullPath, new DotNetRunSettings()
            {
                Configuration = configuration
            });
        });

    Task(packTask)
       .IsDependentOn(testTask)
       .IsDependeeOf("Default")
       .Does(async Task () => await new PluginPacker(plugin).Pack());
}

Task("Default").Does(static () => {});
#endregion

RunTarget(target);