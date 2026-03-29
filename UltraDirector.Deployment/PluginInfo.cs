namespace UltraDirector.Deployment;

public sealed class PluginInfo
{
    public readonly SolutionProject pluginProject;
    public readonly SolutionProject? testProject;
    public readonly string configuration;
    public readonly DirectoryPath projectDir;
    public readonly FilePath csprojFilePath;
    public PluginInfo(SolutionProject pluginProject, string configuration, SolutionProject? testProject = null)
    {
        this.pluginProject = pluginProject;
        this.configuration = configuration;
        this.testProject = testProject;
        csprojFilePath = FilePath.FromString(pluginProject.Path.FullPath);
        projectDir = csprojFilePath.GetDirectory();
    }
}