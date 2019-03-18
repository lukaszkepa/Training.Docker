using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Training.Docker.CommonLibs.RabbitMQDAL
{
    public class RabbitMQConnectivityBasicTasks : IDisposable
    {
        protected IConnection _conn = null;
        protected IModel _channel = null;
        private bool _isDisposed = false;

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

        protected void ConnectWithRabbitMQInstance(string userName, string password, string hostname, string port, out IConnection conn, out IModel channel)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.UserName = userName;
            connectionFactory.Password = password;
            connectionFactory.HostName = hostname;
            connectionFactory.Port = Int32.Parse(port);
            conn = connectionFactory.CreateConnection();
            channel = conn.CreateModel();
        }

        protected void ConnectWithExchange(IModel channel, string exchangeName)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        }

        protected void ConnectWithQueue(IModel channel, string exchangeName, string queueName, string messageKey)
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchangeName, messageKey);
        }         
    }
}