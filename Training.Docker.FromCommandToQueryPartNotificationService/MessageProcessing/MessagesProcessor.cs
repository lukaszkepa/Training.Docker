using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Training.Docker.CommonLibs.RabbitMQDAL;

namespace Training.Docker.FromCommandToQueryPartNotificationService.MessageProcessing
{
    public class MessagesProcessor : IMessagesProcessor, IHostedService
    {
        private ILogger _logger = null;
        private IOptions<RabbitMQConfig> _rabbitMQConfig = null;
        private MessagesListener _messagesListener = null;
        public MessagesProcessor(ILogger<MessagesProcessor> logger, IOptions<RabbitMQConfig> rabbitMQConfig)
        {
            this._logger = logger;
            this._rabbitMQConfig = rabbitMQConfig;
        }

        public void ProcessMessageFromMessageBroker()
        {
            this._messagesListener = new MessagesListener(this._rabbitMQConfig.Value.UserName, 
                this._rabbitMQConfig.Value.Password, 
                this._rabbitMQConfig.Value.HostName, 
                this._rabbitMQConfig.Value.Port, 
                this._rabbitMQConfig.Value.ExchangeName, 
                this._rabbitMQConfig.Value.QueueName,
                this._rabbitMQConfig.Value.MessageKey);
            this._messagesListener.StartListeningForMessages((json) => Console.WriteLine(json.ToString()));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("Starting");
            ProcessMessageFromMessageBroker();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("Stopping");
            this._messagesListener.Dispose();
            return Task.CompletedTask;
        }
    }
}