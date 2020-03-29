using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BLL;
using System.Configuration;
using Common;

namespace FeeServices
{
    public partial class FeeMessageService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(FeeMessageService));
        public FeeMessageService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
          
            try
            {
                IConnectionFactory factory = new NMSConnectionFactory(new Uri(OtherHelper.GetAppSettingsValue("MessageCentreURL")));
                var connection = factory.CreateConnection(OtherHelper.GetAppSettingsValue("UserName"), OtherHelper.GetAppSettingsValue("Password"));
                connection.ClientId = OtherHelper.GetAppSettingsValue("ClientID");

                connection.Start();
                var session = connection.CreateSession();
                var verifyconsumer = session.CreateDurableConsumer(new ActiveMQTopic(OtherHelper.GetAppSettingsValue("MessageName")), "financeConsumer.USERS.Orientation.Verifyed", null, false);
                verifyconsumer.Listener += consumer_Listener;
                log.Info("服务已启动");
            }
            catch (Exception ex)
            {
                log.Error("启动失败", ex);
            }
        }

        private void consumer_Listener(IMessage message)
        {
            var textmessage = (ITextMessage)message;

            Console.WriteLine(textmessage.Text);
            try
            {
                //接收消息
                Console.WriteLine(textmessage.Text);
                if (textmessage != null)
                {
                    string result = sEnrollBLL.EnrollReview(textmessage.Text);
                    if (string.IsNullOrEmpty(result))
                    {
                        result = "操作成功!";
                    }
                    log.Info("" + result + "=>>" + textmessage.Text);
                }
            }
            catch (Exception ex)
            {
                log.Error(textmessage.Text, ex);
            }
        }


        protected override void OnStop()
        {
            log.Info("服务已停止");
        }
    }
}
