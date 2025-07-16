using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

[assembly: SecurityCritical]
namespace Azuro.Crm.KaseyaPluginHelper
{
	[SecurityCritical]
	public class MSMQHelper
	{
		[SecurityCritical]
		public void InsertQueueItem(string queueName, object msg, Type msgType)
		{
			
			using (System.Messaging.MessageQueue mq = new System.Messaging.MessageQueue(queueName, System.Messaging.QueueAccessMode.Send))
			{
				
				using (System.Messaging.MessageQueueTransaction mqt = new System.Messaging.MessageQueueTransaction())
				{
					
					System.Messaging.Message m = null;
					if (msgType != null)
						m = new System.Messaging.Message(msg, new System.Messaging.XmlMessageFormatter(new Type[] { msgType }));
					else
						m = new System.Messaging.Message(msg);

					mqt.Begin();
					mq.Send(m, mqt);
					mqt.Commit();
					mq.Close();

					
				}
			}
		}
	}
}
