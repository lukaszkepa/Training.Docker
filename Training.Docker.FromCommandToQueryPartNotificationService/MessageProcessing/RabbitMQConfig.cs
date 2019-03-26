using System;

namespace Training.Docker.FromCommandToQueryPartNotificationService.MessageProcessing
{
    public class RabbitMQConfig
    {
        public string ConnectionString { get; set; } = String.Empty;
        public string ExchangeName { get; set; } = String.Empty;
        public string QueueName { get; set; } = String.Empty;
        public string MessageKey { get; set; } = String.Empty;        
    }
}