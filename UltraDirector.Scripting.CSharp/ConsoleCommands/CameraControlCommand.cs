using GameConsole;
using GameConsole.CommandTree;
using plog;

namespace UltraDirector.Scripting.CSharp.ConsoleCommands;

public sealed partial class CameraControlCommand(Console con) : CommandRoot(con), IConsoleLogger
{
    public override string Name => "camera-cs";
    public override string Description => "Controls the UltraDirector cameras using C# scripting";
    public Logger Log => new(Name);

    protected override Branch BuildTree(Console con)
    {
        return Branch("camera-cs",
            Leaf<string>("select", SelectCamera),
            Leaf("repl", ActivateRepl));
    }
}