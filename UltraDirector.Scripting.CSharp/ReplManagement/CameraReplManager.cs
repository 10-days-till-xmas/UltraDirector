using System;
using GameConsole;
using JetBrains.Annotations;
using UltraDirector.CameraLogic;
using UltraDirector.LogUtils;
using Logger = plog.Logger;

namespace UltraDirector.Scripting.CSharp.ReplManagement;

[Serializable]
[ConfigureSingleton(SingletonFlags.DestroyDuplicates)]
public sealed class CameraReplManager : MonoSingleton<CameraReplManager>, IConsoleLogger
{
    public bool IsActive { get; private set; }

    [PublicAPI]
    public UKCamera? SelectedCamera { get; set; }
    public Logger Log => new("Camera C# REPL");

    public CSharpRepl Repl { get; private set; } = null!;

    private void Awake()
    {
        Repl = gameObject.GetOrAddComponent<CSharpRepl>();
        Log.RegisterToConsole();
    }
    
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void EvaluateExpression(string expression)
    {
        Repl.RunLine(expression);
    }
}