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
        private string _userName = String.Empty;
        private string _password = String.Empty;
        private string _hostName = String.Empty;
        private string _port = String.Empty;
        private string _exchangeName = String.Empty;
        private bool _isInstantiatedCorrectly = false;        

        public MessagesPublisher(string userName, string password, string hostName, string port, string exchangeName)
        {
            this._userName = userName;
            this._password = password;
            this._hostName = hostName;
            this._port = port;
            this._exchangeName = exchangeName;

            ConnectWithRabbitMQInstance(this._userName, this._password, this._hostName, this._port, out this._conn, out this._channel);
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
            if (this._isInstantiatedCorrectly)
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