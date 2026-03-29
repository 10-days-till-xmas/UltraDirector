namespace UltraDirector.Scripting.CSharp.AutoScripter;

public sealed class OnCheckpointLoadAutoScriptTrigger(string checkpointName) : AutoScriptTrigger
{
    public override bool OnSceneLoad => false;
    public string CheckpointName { get; private set; } = checkpointName;
}