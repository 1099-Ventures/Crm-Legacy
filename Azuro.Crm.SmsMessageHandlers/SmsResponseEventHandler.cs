using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common.Messaging;
using Azuro.Crm.SmsMessages;
using Azuro.Crm.Integration;
using Azuro.Data;
using NLog;
using Azuro.Crm.SmsData;
using System.Threading;

namespace Azuro.Crm.SmsMessageHandlers
{
	public class SmsResponseEventHandler : IMessageHandler<SmsResponseMessage>
	{
		public void ProcessMessage(SmsResponseMessage msg)
		{
			Logger.Trace("[{0}] Entering [{1}:ProcessMessage] - ProviderMsgId - [{2}]", Thread.CurrentThread.ManagedThreadId, this, msg.ProviderMsgId);

			SmsSendLog smsDb = null;
			int retryCount = 0;
			if (!int.TryParse(msg.ClientMsgId, out int clientMsgId))
				throw new ArgumentOutOfRangeException(string.Format("[{0}] Unable to Parse ClientMsgId [{1}] with ProviderId [{2}]", Thread.CurrentThread.ManagedThreadId, msg.ClientMsgId, msg.ProviderId));

			while (smsDb == null && retryCount++ < 5)
			{
				try
				{
					using (var ctx = new AzuroSMSEntities())
					{
						smsDb = (from sms in ctx.SmsSendLogs
								 where sms.ID == clientMsgId
									 && string.Compare(sms.ProviderId.Trim(), msg.ProviderMsgId.Trim(), true) == 0
								 select sms).FirstOrDefault();

						if (smsDb != null)
						{
							Logger.Trace("[{0}] Found Db object with ID = [{1}] && ProviderMsgId = [{2}]", Thread.CurrentThread.ManagedThreadId, clientMsgId, msg.ProviderMsgId);
							ICrmHelper helper = CrmHelperFactory.Create(smsDb.OrganizationId);

							Entities.SmsStatus status = Entities.SmsStatus.Send;
							switch (msg.Status)
							{
								case MessageStatus.Success:
									smsDb.DateDelivered = DateTime.Now;
									status = Entities.SmsStatus.Delivered;
									break;

								case MessageStatus.Failed:
									status = Entities.SmsStatus.Failed;
									break;

								case MessageStatus.Processing:
									status = Entities.SmsStatus.Sent;
									break;

								case MessageStatus.Unknown:
								default:
									status = Entities.SmsStatus.Send;
									break;
							}

							var sms = helper.GetEntity<Entities.Sms>(smsDb.ActivityId);
							// TODO: Sms Provider value below is hard-coded
							sms.Status = status;
							sms.ProviderErrorMessage = msg.ProviderStatusMessage;

							Logger.Trace("[{0}] Updating Sms Activity with status [{1}]", Thread.CurrentThread.ManagedThreadId, sms.Status);
							helper.Update<Azuro.Crm.Entities.Sms>(sms);

							smsDb.ProviderStatus = msg.ProviderStatus;
							smsDb.ProviderStatusMessage = msg.ProviderStatusMessage;

							Logger.Trace("[{0}] Updating database with status [{1}]", Thread.CurrentThread.ManagedThreadId, smsDb.ProviderStatus);
							ctx.SaveChanges();

							// Update Status Fields on Failed or Delivered. Must be
							// done after update to avoid validation errors.
							if (sms.Status == Entities.SmsStatus.Delivered || sms.Status == Entities.SmsStatus.Failed)
							{
								Logger.Trace("[{0}] Closing Crm Activity status", Thread.CurrentThread.ManagedThreadId);
								sms.ActivityStatus = Entities.SmsActivityStatus.Completed;
								sms.StatusReason = Entities.SmsStatusReason.Completed;
							}

							helper.SetStatus(sms);
						}
						else
						{
							Logger.Error("[{0}] - Unable to retrieve DB entry with ID = [{1}] && ProviderMsgId = [{2}]", Thread.CurrentThread.ManagedThreadId, retryCount, clientMsgId, msg.ProviderMsgId);
							Thread.Sleep(retryCount * 1000);
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error("[{0}] {1} in {2}.ProcessMessage : [SmsId:{3}|ProviderId:{4}] - [{5}]", Thread.CurrentThread.ManagedThreadId, ex.GetType(), this, clientMsgId, msg.ProviderMsgId, ex.Message);
				}
			}

			if (retryCount >= 5)
			{
				Logger.Error("[{0}] FAILED to retrieve DB entry with ID = [{1}] && ProviderMsgId = [{2}] after [{3}] retries.", Thread.CurrentThread.ManagedThreadId, retryCount, msg.ClientMsgId, msg.ProviderMsgId, retryCount);
			}
		}

		private static Logger Logger = LogManager.GetCurrentClassLogger();
	}
}