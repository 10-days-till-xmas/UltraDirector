using System;
using BepInEx.Logging;
using plog.Handlers;
using plog.Models;
using Logger = plog.Logger;

namespace UltraDirector.LogUtils;

public sealed class BepinexPlogHandler(ManualLogSource logger) : ILogHandler
{
    public Log HandleRecord(Logger source, Log log)
    {
        var prefix = source.Tag?.Name ?? "";
        var message = log.Message;
        switch (log.Level)
        {
            case Level.Debug:
                logger.LogDebug($"[{prefix}] {message}");
                break;
            case Level.Info:
                logger.LogInfo($"[{prefix}] {message}");
                break;
            case Level.Warning:
                logger.LogWarning($"[{prefix}] {message}");
                break;
            case Level.Error:
                logger.LogError($"[{prefix}] {message}");
                break;
            case Level.Exception:
                logger.LogFatal($"[{prefix}] {message}");
                break;
            case Level.Off:
                logger.Log(LogLevel.None, $"[{prefix}] {message}");
                break;
            case Level.Fine:
                logger.LogDebug($"{prefix} Level.Fine: {message}");
                break;
            case Level.CommandLine:
                logger.LogDebug($"{prefix} Level.CommandLine: {message}");
                break;
            case Level.Config:
                logger.LogDebug($"{prefix} Level.Config: {message}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return log;
    }
}