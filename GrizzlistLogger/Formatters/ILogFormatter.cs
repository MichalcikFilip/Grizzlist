using System;

namespace Grizzlist.Logger.Formatters
{
    public interface ILogFormatter
    {
        string Format(LogLevel level, string message, object source, DateTime time);
    }
}
