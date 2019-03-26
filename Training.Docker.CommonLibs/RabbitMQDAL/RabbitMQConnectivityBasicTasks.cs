using System;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Training.Docker.CommonLibs.RabbitMQDAL
{
    public class RabbitMQConnectivityBasicTasks : IDisposable
    {
        protected IConnection _conn = null;
        protected IModel _channel = null;
        protected bool _isDisposed = false;

        protected void Dispose(bool isExpliciteInvoked)
        {
            if (!this._isDisposed)
            {
                if (this._channel != null)
                    this._channel.Close();

                if (this._conn != null)
                    this._conn.Close();

                if (isExpliciteInvoked)
                    GC.SuppressFinalize(this);

                this._isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void ConnectWithRabbitMQInstance(string connectionString, out IConnection conn, out IModel channel)
        {
            var policy = Policy.Handle<BrokerUnreachableException>()
                .WaitAndRetry(3, attempt => TimeSpan.FromSeconds(attempt * 5));

            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            conn = policy.Execute(() => connectionFactory.CreateConnection());
            channel = conn.CreateModel();
        }

        protected void ConnectWithExchange(IModel channel, string exchangeName)
        {
            if (!String.IsNullOrEmpty(exchangeName))
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        }

        protected void ConnectWithQueue(IModel channel, string exchangeName, string queueName, string messageKey)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, messageKey);            
        }                 
    }
}