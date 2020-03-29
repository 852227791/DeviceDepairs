using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;

namespace Common
{
    public class ActiveMqHelper
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly NMSConnectionFactory _factory;
       
        public ActiveMqHelper()
        {
            _factory=new NMSConnectionFactory(new Uri(ConfigurationManager.ConnectionStrings["Activemq"].ConnectionString));
            _userName=ConfigurationManager.AppSettings["SendUserName"];
            _password=ConfigurationManager.AppSettings["SendPassword"];
           
        }

        public ActiveMqHelper(string conn, string userName, string password)
        {
            _factory=new NMSConnectionFactory(new Uri(conn));
            _userName=userName;
            _password=password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="messageBody"></param>
        /// <param name="properties"></param>
        /// <param name="pereisent"></param>
        /// <param name="isquery"></param>
        public void PublishMessage(string name, string messageBody, Dictionary<string, string> properties = null, bool pereisent = false, bool isquery = false)
        {
            using (var publisherConn = _factory.CreateConnection(_userName, _password))
            {
                publisherConn.Start();
                using (var session = publisherConn.CreateSession())
                {
                    var prod = session.CreateProducer(new ActiveMQTopic(name));

                    var message = prod.CreateTextMessage();

                    message.Text=messageBody;

                    if (properties!=null)
                    {
                        //设置消息对象的属性，这个很重要哦，是Queue的过滤条件，也是P2P消息的唯一指定属性
                        foreach (var property in properties)
                        {
                            message.Properties.SetString(property.Key, property.Value);
                        }
                    }

                    //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否长链，MsgPriority消息优先级别，发送最小单位，当然还有其他重载
                    prod.Send(message, pereisent? MsgDeliveryMode.Persistent:MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                }

            }
        }
    }
}
