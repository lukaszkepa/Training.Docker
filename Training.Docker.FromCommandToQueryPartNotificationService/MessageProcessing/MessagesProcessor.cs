using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Training.Docker.Models;
using Training.Docker.CommonLibs.RabbitMQDAL;

namespace Training.Docker.FromCommandToQueryPartNotificationService.MessageProcessing
{
    public class MessagesProcessor : IMessagesProcessor, IHostedService
    {
        private ILogger _logger = null;
        private IOptions<RabbitMQConfig> _rabbitMQConfig = null;
        private IOptions<MongoDBConfig> _mongoDBConfig = null;
        private IOptions<SqlServerDBConfig> _sqlServerDBConfig = null;
        private MessagesListener _messagesListener = null;
        public MessagesProcessor(ILogger<MessagesProcessor> logger,
            IOptions<RabbitMQConfig> rabbitMQConfig,
            IOptions<MongoDBConfig> mongoDBConfig,
            IOptions<SqlServerDBConfig> sqlServerDBConfig)
        {
            this._logger = logger;
            this._rabbitMQConfig = rabbitMQConfig;
            this._mongoDBConfig = mongoDBConfig;
            this._sqlServerDBConfig = sqlServerDBConfig;
        }

        public void ProcessMessageFromMessageBroker()
        {
            this._messagesListener = new MessagesListener(this._rabbitMQConfig.Value.ConnectionString,
                this._rabbitMQConfig.Value.ExchangeName, 
                this._rabbitMQConfig.Value.QueueName,
                this._rabbitMQConfig.Value.MessageKey);
            this._messagesListener.StartListeningForMessages((json) => DoMessageProcessing(json));
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

        private void DoMessageProcessing(JObject json)
        {
            CustomerOrderAdded customerOrderAdded = new CustomerOrderAdded {
                RequestId = Guid.Parse(json["RequestId"].ToString()),
                CustomerOrderId = json["CustomerOrderId"].ToString()
            };
            CommonLibs.MongoDbDAL.Repository<Customer> repositoryMongoDB = new Training.Docker.CommonLibs.MongoDbDAL.Repository<Customer>(
                this._mongoDBConfig.Value.ConnectionString,
                this._mongoDBConfig.Value.DatabaseName,
                CommonLibs.MongoDbDAL.Collections.Orders);
            Customer customer = repositoryMongoDB.GetAsync(customerOrderAdded.CustomerOrderId).Result;
            Decimal totalPrice = 0.0M;
            foreach(var orderDetail in customer.Order.OrderDetails)
                totalPrice += ((Decimal)orderDetail.UnitPrice) * ((int)orderDetail.Amount); 
            Training.Docker.CommonLibs.SqlServerDBDAL.RepositoryADO repositorySqlDB = new Training.Docker.CommonLibs.SqlServerDBDAL.RepositoryADO(
                this._sqlServerDBConfig.Value.ConnectionString);
            repositorySqlDB.InsertCustomerOrderAggregatedData(customer.Name, customer.Order.OrderPlacementDate, totalPrice);
        }
    }
}