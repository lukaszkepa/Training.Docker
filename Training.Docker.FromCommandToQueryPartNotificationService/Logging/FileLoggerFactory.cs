using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Training.Docker.FromCommandToQueryPartNotificationService.Logging
{
    public class FileLoggerFactory : ILoggerFactory
    {
        private static object _lock = new Object();
        private static FileLoggerProvider _fileLoggerProvider = null;
        private FileLoggerConfig _config = null;

        public FileLoggerFactory(IOptions<FileLoggerConfig> config)
        {
            this._config = config.Value;
            lock(_lock)
            {
                if (_fileLoggerProvider == null)
                    _fileLoggerProvider = new FileLoggerProvider(this._config);
            }
        }

        public void AddProvider(ILoggerProvider provider)
        {
            return;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _fileLoggerProvider.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            _fileLoggerProvider.Dispose();
        }
    }
}