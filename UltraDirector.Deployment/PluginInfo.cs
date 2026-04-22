namespace UltraDirector.Deployment;

public sealed class PluginInfo
{
    public readonly SolutionProject pluginProject;
    public readonly SolutionProject? testProject;
    public readonly DirectoryPath projectDir;
    public readonly FilePath csprojFilePath;
    public PluginInfo(SolutionProject pluginProject, SolutionProject? testProject = null)
    {
        this.pluginProject = pluginProject;
        this.testProject = testProject;
        csprojFilePath = FilePath.FromString(pluginProject.Path.FullPath);
        projectDir = csprojFilePath.GetDirectory();
    }
}