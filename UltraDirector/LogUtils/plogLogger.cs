using plog;

namespace UltraDirector.LogUtils;

internal sealed class PlogLogger(Logger inner) : ILogger
{
    public PlogLogger(string name) : this(new Logger(name)) { }
    public void LogInfo(object message) => inner.Info(message.ToString());

    public void LogWarning(object message) => inner.Warning(message.ToString());

    public void LogError(object message) => inner.Error(message.ToString());

    public void LogDebug(object message) => inner.Debug(message.ToString());
    public void LogFatal(object message) => inner.Debug(message.ToString());
    public void LogMessage(object message) => inner.Info(message.ToString());
    public static implicit operator PlogLogger(Logger logger) => new(logger);
}