using Grizzlist.Logger.Formatters;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Grizzlist.Logger
{
    public static class Log
    {
        private static LogLevel _minLogLevel = LogLevel.Debug;
        private static string _path = string.Empty;
        private static string _file = string.Empty;
        private static ILogFormatter _fomatter = null;

        public static void Configure(LogLevel minLogLevel, string path, string file)
        {
            Configure(minLogLevel, path, file, new DefaultFormatter());
        }

        public static void Configure(LogLevel minLogLevel, string path, string file, ILogFormatter formatter)
        {
            _minLogLevel = minLogLevel;
            _path = path;
            _file = file;
            _fomatter = formatter;
        }

        public static async void CreateLog(LogLevel level, string message, object source)
        {
            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(_path) && !string.IsNullOrEmpty(_file) && _fomatter != null && level >= _minLogLevel)
                {
                    try
                    {
                        lock (_path)
                        {
                            DirectoryInfo directory = Directory.CreateDirectory(_path);
                            File.AppendAllLines($"{directory.FullName}\\{_file}", new string[] { _fomatter.Format(level, message, source, DateTime.Now) });
                        }
                    }
                    catch { }
                }
            });
        }

        public static void Debug(string message, object source)
        {
            CreateLog(LogLevel.Debug, message, source);
        }

        public static void Info(string message, object source)
        {
            CreateLog(LogLevel.Info, message, source);
        }

        public static void Warning(string message, object source)
        {
            CreateLog(LogLevel.Warning, message, source);
        }

        public static void Error(string message, object source)
        {
            CreateLog(LogLevel.Error, message, source);
        }
    }
}
