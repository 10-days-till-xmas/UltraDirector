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
            CameraGet_,
            CameraSet_,
            Window
            #if DEBUG
          , Branch("debug",
                Branch("prefs",
                    Leaf<string>("setSpawnKey", SetSpawnCameraKey),
                    Leaf("listKeyCodes", ListKeyCodes)
                    )
                )
            #endif
            );
    }
}