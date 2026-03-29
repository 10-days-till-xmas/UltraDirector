namespace UltraDirector.Scripting.CSharp.AutoScripter;

public abstract class AutoScriptTrigger
{
    public abstract bool OnSceneLoad { get; }
    public bool OnCheckpointLoad => !OnSceneLoad;
}