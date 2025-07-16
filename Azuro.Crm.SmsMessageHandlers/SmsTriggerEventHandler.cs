using System;
using Azuro.Common.Messaging;
using Azuro.Crm.SmsMessages;
using Azuro.Crm.Integration;
using Azuro.Crm.Entities;
using NLog;
using Azuro.Crm.SmsData;
using System.Linq;

namespace Azuro.Crm.SmsMessageHandlers
{
	public class SmsTriggerEventHandler : IMessageHandler<SmsTriggerEventMessage>
	{
		public void ProcessMessage(SmsTriggerEventMessage msg)
		{
			Logger.Trace("Starting ProcessMessage");

			// Save the SMS as early as possible, in case there are CRM or other errors.
			SmsSendLog smsDb = null;
			Logger.Debug("Saving SMS to DB for first time [{0}]", msg.ActivityId);
			SaveSmsMessage(ref smsDb, msg);
			Logger.Debug("After Saving SMS to DB for first time [{0}] - [{1}]", msg.ActivityId, smsDb.ID);

			ICrmHelper helper = CrmHelperFactory.Create(msg.OrganisationId);
			Logger.Trace("Creating CrmHelper for Organisation [{0}]", msg.OrganisationId);

			Logger.Trace("Get Entity from Crm [{0}]", msg.ActivityId);
			var sms = helper.GetEntity<Azuro.Crm.Entities.Sms>(msg.ActivityId);
			if (sms == null)
			{
				Logger.Error("Unable to retrieve Activity from CRM [{0}]", msg.ActivityId);
				smsDb.ProviderStatusMessage = "Invalid ActivityId or ActivityId was not found in CRM.";
				SaveSmsMessage(smsDb);
				return;
			}

			if (sms.Status != SmsStatus.Send)
			{
				Logger.Warn("Invalid Sms Status [{0}] for Sms [{1}]", sms.Status, sms.Id);
				SaveSmsMessage(ref smsDb, sms);
				return;
			}

			Logger.Trace("Entity retrieved from Crm [{0}]", sms);

			if (string.IsNullOrEmpty(sms.MobilePhone))
			{
				Logger.Error("No mobile number set for SMS [{0}]", sms.Id);
				sms.Status = SmsStatus.Failed;
			}
			else
			{
				Logger.Trace("Sending the SMS [{0}] to [{1}]", sms.Message, sms.MobilePhone);

				if (!sms.MobilePhone.StartsWith("0") && !sms.MobilePhone.StartsWith("+"))
					sms.MobilePhone = "0" + sms.MobilePhone;
				if (!sms.StartDate.HasValue || sms.StartDate == DateTime.MinValue)
					sms.StartDate = DateTime.Now;

				var cfg = RetrieveSmsConfiguration(helper);
				// HACK: Hard-coded to bypass out of date solutions
				if (!Enum.IsDefined(typeof(SmsProvider), cfg.SmsProvider)) cfg.SmsProvider = SmsProvider.Clickatell;
				Logger.Trace("Sms Provider [{0}]", cfg.SmsProvider.ToString());
				var smsClient = Azuro.Sms.Common.SmsProviderFactory.Create(cfg.SmsProvider.ToString());
				Logger.Trace("Config: [{0}] - Footer [{1}]", cfg != null, (cfg != null ? cfg.SmsFooterMessage : string.Empty));
				string message = string.Concat(sms.Message, (cfg != null ? cfg.SmsFooterMessage : string.Empty));
				var csms = smsClient.SendMessage(new Sms.Common.SmsMessage { ClientId = smsDb.ID.ToString(), To = sms.MobilePhone, Message = message, NumberOfMessages = cfg.NumberOfMessages });

				if (string.IsNullOrEmpty(csms.ProviderId))
				{
					Logger.Error("SMS failed to send to [{0}] - [{1}]", sms.MobilePhone, csms.ErrorMessage);
					sms.Status = SmsStatus.Failed;
					sms.ActivityStatus = SmsActivityStatus.Canceled;
					sms.StatusReason = SmsStatusReason.Canceled;
					smsDb.ProviderStatusMessage = sms.ProviderErrorMessage = csms.ErrorMessage;
				}
				else
				{
					Logger.Trace("SMS sent to [{0}] with return id [{1}]", sms.MobilePhone, csms.ProviderId, csms.ErrorMessage);
					sms.Status = SmsStatus.Sent;
					sms.StatusReason = SmsStatusReason.Open;
					sms.ActivityStatus = SmsActivityStatus.Open;
					sms.SentDate = DateTime.Now;
					sms.ProviderMessageId = csms.ProviderId;
					sms.Provider = cfg.SmsProvider;
				}
			}

			if (sms.StartDate == null)
				sms.StartDate = DateTime.Now;
			if (sms.DueDate == null)
				sms.DueDate = DateTime.Now;

			Logger.Trace("Saving the Entity to the Db [{0}]", smsDb.ActivityId);
			// Populate Entity
			Logger.Debug("Saving SMS to DB to update Provider Info [{0}] - [{1}] - [{2}]", msg.ActivityId, smsDb.ID, sms.ProviderMessageId);
			SaveSmsMessage(ref smsDb, sms);
			Logger.Debug("After Saving SMS to DB to update Provider Info [{0}] - [{1}] - [{2}]", msg.ActivityId, smsDb.ID, sms.ProviderMessageId);

			Logger.Trace("Updating the Entity [{0}]", sms.Id);
			helper.Update(sms);

			Logger.Trace("Leaving ProcessMessage");
		}

		private static Logger Logger = LogManager.GetCurrentClassLogger();

		private SmsSendLog FetchSms(SmsSendLog sms)
		{
			using (var ctx = new AzuroSMSEntities())
			{
				return (from smses in ctx.SmsSendLogs
						where smses.ActivityId == sms.ActivityId
						select smses).FirstOrDefault();
			}
		}

		private Entities.SmsConfiguration RetrieveSmsConfiguration(ICrmHelper helper)
		{
			Logger.Trace("Retrieve Sms Configuration Object");

			var cfglist = helper.GetEntityList<SmsConfiguration>();
			if (cfglist.Count == 1)
				return cfglist[0];

			// HACK: Return default cfg
			return new SmsConfiguration { SmsProvider = SmsProvider.Clickatell };
		}

		private void SaveSmsMessage(ref SmsSendLog smsDb, SmsTriggerEventMessage msg)
		{
			Logger.Trace("Saving the Db entry - First");
			if (smsDb == null) smsDb = new SmsSendLog { DateCreated = DateTime.Now, DateChanged = DateTime.Now };

			// Populate Entity
			smsDb.ActivityId = msg.ActivityId;
			smsDb.OrganizationId = msg.OrganisationId;

			var smstmp = FetchSms(smsDb);
			if (smstmp != null)
				smsDb.ID = smstmp.ID;

			if (smsDb.OrganizationId == null || smsDb.ActivityId == null)
				Logger.Warn("Org - {0} | Act - {1} is null", smsDb.OrganizationId, smsDb.ActivityId);
			else
				Logger.Trace("Org - {0} | Act - {1} ", smsDb.OrganizationId, smsDb.ActivityId);

			SaveSmsMessage(smsDb);
		}

		private void SaveSmsMessage(ref SmsSendLog smsDb, Azuro.Crm.Entities.Sms sms)
		{
			Logger.Trace("Saving the Db entry - Second");
			if (smsDb == null) smsDb = new SmsSendLog();

			// Populate Entity
			smsDb.MobileNumber = sms.MobilePhone;
			smsDb.Provider = sms.Provider.ToString();
			smsDb.ProviderId = sms.ProviderMessageId;
			smsDb.ActivityId = sms.Id;
			smsDb.DateSent = sms.SentDate;
			smsDb.Message = sms.Message;

			if (smsDb.OrganizationId == null || smsDb.ActivityId == null)
				Logger.Warn("Org - {0} | Act - {1} is null", smsDb.OrganizationId, smsDb.ActivityId);
			else
				Logger.Trace("Org - {0} | Act - {1} ", smsDb.OrganizationId, smsDb.ActivityId);

			SaveSmsMessage(smsDb);
		}

		private void SaveSmsMessage(SmsSendLog sms)
		{
			Logger.Trace("Creating the Sql Object");
			using (var ctx = new AzuroSMSEntities())
			{
				if (sms.ID == 0)
				{
					Logger.Debug("Adding a new SMS Entry to the DB");
					ctx.SmsSendLogs.Add(sms);
				}
				else
				{
					//var smsUpdate = (from smses in ctx.SmsSendLogs
					//             where smses.ID == sms.ID
					//             select smses).FirstOrDefault();
					Logger.Debug("Updating the SMS Entry on the DB [{0}]", sms.ID);
					ctx.Entry(sms).State = System.Data.Entity.EntityState.Modified;
				}

				ctx.SaveChanges();
			}

			Logger.Debug("Successfully Saved to the Database with ID [{0}]", sms.ID);
		}
	}
}