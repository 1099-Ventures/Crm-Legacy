using Azuro.Common;
using Azuro.MSMQ;
using NLog;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
namespace Azuro.Crm.Integration.Nable.Web
{
    public partial class NotificationProcessor : System.Web.UI.Page
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private string QueueName
        {
            get { return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["QueueName"]) ? ConfigurationManager.AppSettings["QueueName"] : @".\private$\nablenotifications"; }
        }

        private string LogfilePath
        {
            get { return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogfilePath"]) ? ConfigurationManager.AppSettings["LogfilePath"] : @"c:\inetpub\wwwroot\NAbleNotifications"; }
        }

        private bool EnableTracing
        {
            get { return !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Trace"]) ? bool.Parse(ConfigurationManager.AppSettings["Trace"]) : false; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.RequestType == "POST" && Request.InputStream != null && Request.InputStream.Length > 0)
                {
                    //	Process in-bound notification from N-Able
                    Logger.Trace("Starting Message Parsing");
                    Azuro.Crm.Integration.Nable.Entities.Notification notification = null;
                    if (EnableTracing)
                    {
                        using (var reader = new System.IO.StreamReader(Request.InputStream))
                        {
                            var xml = reader.ReadToEnd();
                            using (var writer = new System.IO.StreamWriter(System.IO.File.OpenWrite(System.IO.Path.Combine(LogfilePath, string.Format("nable_{0}.xml", DateTime.Now.ToString("yyyyMMdd_HHmmssf"))))))
                            {
                                writer.WriteLine(xml);
                                notification = Azuro.Crm.Integration.Nable.Entities.Notification.Deserialize(xml);
                            }
                        }
                    }
                    else
                        notification = Azuro.Crm.Integration.Nable.Entities.Notification.Deserialize(Request.InputStream);

                    QueueHelper.Insert(QueueName, notification, typeof(Azuro.Crm.Integration.Nable.Entities.Notification));

                    Logger.Trace("Finished Inserting Message into Queue");
                }
                else
                {
                    lblMessage.Text = "Listening for new incoming messages........";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception in Process Message");
            }
        }
    }
}