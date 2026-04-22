using GameConsole;
using plog;

namespace UltraDirector.LogUtils;

internal static class PloggerExtensions
{
    extension(Logger logger)
    {
        public void LogInfo(string message)
        {
            logger.Info(message);
            Plugin.LogHelper.LogInfo(message);
        }
        public void LogWarning(string message)
        {
            logger.Warning(message);
            Plugin.LogHelper.LogWarning(message);
        }

        public void RegisterToConsole()
        {
            logger.NotifyParent = false;
            logger.AddHandler(Console.Instance!);
        }
    }
}