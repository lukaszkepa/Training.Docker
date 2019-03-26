using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace Training.Docker.CommonLibs.RabbitMQDAL
{
    public class MessagesPublisher : RabbitMQConnectivityBasicTasks
    {
        private string _connectionString;
        private string _exchangeName = String.Empty;
        private bool _isInstantiatedCorrectly = false;        

        public MessagesPublisher(string connectionString, string exchangeName)
        {
            this._connectionString = connectionString;
            this._exchangeName = exchangeName;

            ConnectWithRabbitMQInstance(this._connectionString, out this._conn, out this._channel);
            if ((this._conn == null) || (this._channel == null))
                return;

            ConnectWithExchange(this._channel, this._exchangeName);

            this._isInstantiatedCorrectly = true;            
        }

        ~MessagesPublisher()
        {
            Dispose(false);
        }

        public void SendMessageToRabbitMQ(JObject message, string messageKey)
        {
            if ((this._isInstantiatedCorrectly) && (!this._isDisposed))
            {
                MemoryStream ms = null;
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    ms = new MemoryStream();
                    bf.Serialize(ms, message.ToString());
                    byte[] objectBasedMsg = ms.ToArray();

                    this._channel.BasicPublish(this._exchangeName, messageKey, null, objectBasedMsg);
                }
                finally
                {
                    if (ms != null)
                        ms.Close();
                }
            }
        }         
    }
}