using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace Training.Docker.CommonLibs.RabbitMQDAL
{
    public class MessagesListener : RabbitMQConnectivityBasicTasks
    {
        private string _userName = String.Empty;
        private string _password = String.Empty;
        private string _hostName = String.Empty;
        private string _port = String.Empty;
        private string _exchangeName = String.Empty;
        private string _queueName = String.Empty;
        private string _messageKey = String.Empty;
        private bool _isInstantiatedCorrectly = false;

        public MessagesListener(string userName, string password, string hostName, string port, string exchangeName, string queueName, string messageKey)
        {
            this._userName = userName;
            this._password = password;
            this._hostName = hostName;
            this._port = port;
            this._exchangeName = exchangeName;
            this._queueName = queueName;
            this._messageKey = messageKey;

            ConnectWithRabbitMQInstance(this._userName, this._password, this._hostName, this._port, out this._conn, out this._channel);
            if ((this._conn == null) || (this._channel == null))
                return;

            ConnectWithQueue(this._channel, this._exchangeName, this._queueName, this._messageKey);
            
            this._isInstantiatedCorrectly = true;
        }

        ~MessagesListener()
        {
            Dispose(false);
        }

        public void StartListeningForMessages(Action<JObject> messageHandler)
        {
            if (this._isInstantiatedCorrectly)
            {
                var consumer = new EventingBasicConsumer(this._channel);
                consumer.Received += (cons, eventArgs) => {
                    MemoryStream ms = null;
                    try
                    {
                        byte[] objectBasedMsg = eventArgs.Body;

                        BinaryFormatter bf = new BinaryFormatter();
                        ms = new MemoryStream(objectBasedMsg);
                        string sMessage = (string)bf.Deserialize(ms);
                        JObject message = JObject.Parse(sMessage);
                        ((EventingBasicConsumer)cons).Model.BasicAck(eventArgs.DeliveryTag, false);
                        messageHandler(message);
                    }
                    finally
                    {
                        if (ms != null)
                            ms.Close();
                    }
                };

                this._channel.BasicConsume(this._queueName, false, consumer);
            }
        }        
    }
}