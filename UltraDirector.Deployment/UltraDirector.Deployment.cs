using UltraDirector.Deployment;

#region Read Arguments
var target = Argument("target", "Default");

var configuration = Argument("configuration", "Release");
#endregion

var slnFile = FilePath.FromString("./UltraDirector.slnx");
var slnParsed = ParseSolution(slnFile);

var projects = slnParsed.Projects.ToDictionary(p => p.Name);
var plugins = projects.Values
                      .Where(static p => p.Type is "{PLUGIN}")
                      .Select(p =>
                       {
                           var testProj = projects.GetValueOrDefault(p.Name + ".Tests");
                           return new PluginInfo(p, configuration, testProj);
                       })
                      .ToList();

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Setup(_ =>
{
    CreateDirectory(PluginPacker.tempDir);
    Information($"Created temp directory at {PluginPacker.tempDir}");
});
Teardown(_ =>
{
    DeleteDirectory(PluginPacker.tempDir, new DeleteDirectorySettings()
    {
        Force = true, Recursive = true
    });
    Information($"Deleted temp directory at {PluginPacker.tempDir}");
});

Task("Clean")
   .IsDependeeOf("Default")
   .WithCriteria(c => HasArgument("rebuild"))
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
            DotNetBuild(plugin.pluginProject.Path.FullPath, new DotNetBuildSettings
            {
                Configuration = configuration,
                OutputDirectory = PluginPacker.tempDir.Combine(name),
                ArgumentCustomization = args => args.Append("/p:SuppressDeployment=true")
            });
        });

    Task(testTask)
       .IsDependentOn(buildTask)
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

Task("Default").Does(() => {});

RunTarget(target);