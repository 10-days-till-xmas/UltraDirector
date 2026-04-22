using GameConsole;
using GameConsole.CommandTree;
using Logger = plog.Logger;

namespace UltraDirector.CameraLogic.ConsoleCommands;

public sealed partial class CameraRootCommand(Console con) : CommandRoot(con), IConsoleLogger
{
    public override string Name => "camera";
    public override string Description => "Controls cameras in the scene";
    public Logger Log => new(Name);

    protected override Branch BuildTree(Console con)
    {
        return Branch("camera",
            Leaf("list", ListCameras),
            Leaf<string>("spawn", SpawnCamera), // <id>
            Leaf<string>("remove", RemoveCamera), // <id>
            Leaf<string>("start-recording", StartRecording),
            Leaf<string, float>("start-recording-t", StartRecordingWithTime),
            Leaf<string>("stop-recording", StopRecording),
            Leaf<string>("remux", RemuxVideo),
            CameraGet_,
            CameraSet_,
            Window
            #if DEBUG
          , Debug
            #endif
            );
    }
}