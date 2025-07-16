using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Azuro.MSMQ;
using Azuro.Crm.Entities;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;

namespace Azuro.Crm.Plugin
{
	//	Receive Sms Event and post it the appropriate Msmq queue

	public class SmsActivityCreateHandler : Azuro.Crm.Integration.APlugin
	{
		public override void Execute(IServiceProvider serviceProvider)
		{
			GetTracingService(serviceProvider).Trace("{0} - Plugin has started.", this);

			Entity entity = GetPrimaryEntity(serviceProvider, "azuro_sms");

			this.DoUpdateExecution(serviceProvider, entity);
		}

		public void DoUpdateExecution(IServiceProvider serviceProvider, Entity entity)
		{
			GetTracingService(serviceProvider).Trace("Entering [DoUpdateExecution]");

			//	TODO: We might need to cache this for optimization in the future
			var cfg = RetrieveSmsConfiguration(serviceProvider);

			//	Get SMS Entity and Test state is correct for sending.
			Sms sms = GetCrmHelper(serviceProvider).GetEntity<Sms>(entity.Id);
			if (sms.Status != SmsStatus.Send)
			{
				GetTracingService(serviceProvider).Trace("Invalid Status for SMS Sending: [{0}]", sms.Status);
				GetTracingService(serviceProvider).Trace("Leaving [DoUpdateExecution]");
				return;
			}

			//QueueHelper.Insert(@".\private$\SmsTriggerQueue", new SmsMessages.SmsTriggerEventMessage { OrganisationId = context.OrganizationId, ActivityId = smsId, });
			InsertQueueItem((cfg != null && !string.IsNullOrEmpty(cfg.SmsTriggerEventQueue)) ? cfg.SmsTriggerEventQueue : @".\private$\SmsTriggerQueue", new SmsMessages.SmsTriggerEventMessage { OrganisationId = GetContext(serviceProvider).OrganizationId, ActivityId = entity.Id, }, typeof(SmsMessages.SmsTriggerEventMessage));

			GetTracingService(serviceProvider).Trace("Updating the Sms Activity Status");
			SetStateRequest setStateRequest = new SetStateRequest();

			setStateRequest.EntityMoniker = new EntityReference(Entities.Sms.LogicalName, entity.Id);
			setStateRequest.State = new OptionSetValue((int)SmsActivityStatus.Scheduled);
			setStateRequest.Status = new OptionSetValue((int)SmsStatusReason.Scheduled);

			var response = GetOrganizationService(serviceProvider).Execute(setStateRequest);

			GetTracingService(serviceProvider).Trace("Leaving [DoUpdateExecution]");
		}

		private void InsertQueueItem(string queueName, object msg, Type msgType)
		{
			//	TODO: Logging in CRM
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

		private SmsConfiguration RetrieveSmsConfiguration(IServiceProvider serviceProvider)
		{
			SmsConfiguration cfg = null;
			GetTracingService(serviceProvider).Trace("Retrieving Configuration");
			var cfglist = GetCrmHelper(serviceProvider).GetEntityList<SmsConfiguration>();
			if (cfglist.Count == 1)
			{
				GetTracingService(serviceProvider).Trace("configuration entity found");
				cfg = cfglist[0];
			}
			else if (cfglist.Count > 1)
			{
				GetTracingService(serviceProvider).Trace("There can only be one configuration entity");
			}

			GetTracingService(serviceProvider).Trace("Configuration - {0}", cfg != null ? "Obtained" : "Not Loaded");
			return cfg;
		}
	}
}
