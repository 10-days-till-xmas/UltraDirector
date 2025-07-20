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
            Leaf<string>("spawn", SpawnCamera),
            Leaf<string>("remove", RemoveCamera),
            Branch("get",
                Leaf<string>("summary", GetCameraSummary),
                Leaf<string>("depth", GetCameraDepth),
                Leaf<string>("rectSize", GetCameraRectSize),
                Leaf<string>("rectPosition", GetCameraRectPosition)
                ),
            Branch("set",
                Leaf<string, float>("depth", SetCameraDepth),
                Leaf<string, float, float>("rectSize", SetCameraRectSize),
                Leaf<string, float, float>("rectPosition", SetCameraRectPosition)
                )
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