using System;
using Microsoft.Extensions.Logging;

namespace Training.Docker.FromCommandToQueryPartNotificationService.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private static object _lock = new Object();
        private static FileLogger _fileLogger = null;

        public FileLoggerProvider(FileLoggerConfig config)
        {
            lock(_lock)
            {
                if (_fileLogger == null)
                    _fileLogger = new FileLogger(config);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _fileLogger;
        }

        public void Dispose()
        {
            _fileLogger = null;
        }
    }
}