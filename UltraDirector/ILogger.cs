namespace UltraDirector;

public interface ILogger
{
    void LogInfo(object message);
    void LogWarning(object message);
    void LogError(object message);
    void LogDebug(object message);
    void LogFatal(object message);
    void LogMessage(object message);
}