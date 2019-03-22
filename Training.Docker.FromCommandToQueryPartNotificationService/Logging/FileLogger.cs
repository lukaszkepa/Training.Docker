using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Training.Docker.FromCommandToQueryPartNotificationService.Logging
{
    public class FileLogger : ILogger
    {
        private static object _lock = new Object();
        private FileLoggerConfig _config = null;
        public FileLogger(FileLoggerConfig config)
        {
            this._config = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            lock (_lock)
            {
                if (_config.EventId == 0 || _config.EventId == eventId.Id)
                {
                    FileStream fs = null;
                    StreamWriter sw = null;
                    try
                    {
                        fs = new FileStream(this._config.LogFile, FileMode.Append);
                        sw = new StreamWriter(fs);
                        sw.WriteLine(String.Format("{0} - {1} - {2}", DateTime.Now, logLevel.ToString(), formatter(state, exception)));
                    }
                    finally
                    {
                        if (sw != null)
                            sw.Close();
                        if (fs != null)
                            fs.Close();
                    }
                }
            }
        }
    }
}