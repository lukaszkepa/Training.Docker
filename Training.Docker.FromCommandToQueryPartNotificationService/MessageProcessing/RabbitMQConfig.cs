using System;

namespace Training.Docker.FromCommandToQueryPartNotificationService.MessageProcessing
{
    public class RabbitMQConfig
    {
        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string HostName { get; set; } = String.Empty;
        public string Port { get; set; } = String.Empty;
        public string ExchangeName { get; set; } = String.Empty;
        public string QueueName { get; set; } = String.Empty;
        public string MessageKey { get; set; } = String.Empty;        
    }
}