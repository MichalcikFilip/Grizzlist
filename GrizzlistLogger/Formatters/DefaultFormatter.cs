using System;

namespace Grizzlist.Logger.Formatters
{
    class DefaultFormatter : ILogFormatter
    {
        public string Format(LogLevel level, string message, object source, DateTime time)
        {
            return $"{time} {FormatLogLevel(level)} [{source?.GetType().Name}]: {message}";
        }

        private string FormatLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return "DBG";
                case LogLevel.Info:
                    return "INF";
                case LogLevel.Warning:
                    return "WAR";
                case LogLevel.Error:
                    return "ERR";
                default:
                    return string.Empty;
            }
        }
    }
}
