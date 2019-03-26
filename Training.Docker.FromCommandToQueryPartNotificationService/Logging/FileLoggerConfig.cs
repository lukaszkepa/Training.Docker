using Microsoft.Extensions.Logging;

namespace Training.Docker.FromCommandToQueryPartNotificationService.Logging
{
    public class FileLoggerConfig
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public int EventId { get; set; } = 0;

        public string LogFile { get; set; } = string.Empty;
    }
}