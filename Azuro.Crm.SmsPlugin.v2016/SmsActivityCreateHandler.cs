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

	public class SmsActivityCreateHandler : Integration.APlugin
	{
		protected override void Execute(IServiceProvider serviceProvider, IOrganizationService service, IPluginExecutionContext context)
		{
			GetTracingService(serviceProvider).Trace("{0} - Plugin has started.", this);

			Entity entity = GetPrimaryEntity(serviceProvider, "azuro_sms");

			this.DoUpdateExecution(serviceProvider, service, entity);
		}

		public void DoUpdateExecution(IServiceProvider serviceProvider, IOrganizationService service, Entity entity)
		{
			GetTracingService(serviceProvider).Trace("Entering [DoUpdateExecution]");

			//	TODO: We might need to cache this for optimization in the future
			var cfg = RetrieveSmsConfiguration(serviceProvider, service);

			//	Get SMS Entity and Test state is correct for sending.
			var sms = GetEntity<azuro_sms>(service, entity.Id);
			if (sms.azuro_StatusEnum != azuro_sms_azuro_Status.Send)
			{
				LogTrace(serviceProvider, "Invalid Status for SMS Sending: [{0}]", sms.azuro_StatusEnum);
				LogTrace(serviceProvider, "Leaving [DoUpdateExecution]");
				return;
			}

			//QueueHelper.Insert(@".\private$\SmsTriggerQueue", new SmsMessages.SmsTriggerEventMessage { OrganisationId = context.OrganizationId, ActivityId = smsId, });
			InsertQueueItem(serviceProvider, (cfg != null && !string.IsNullOrEmpty(cfg.azuro_SmsTriggerEventQueue)) ? cfg.azuro_SmsTriggerEventQueue : @".\private$\SmsTriggerQueue", new SmsMessages.SmsTriggerEventMessage { OrganisationId = GetContext(serviceProvider).OrganizationId, ActivityId = entity.Id, }, typeof(SmsMessages.SmsTriggerEventMessage));

			LogTrace(serviceProvider, "Updating the Sms Activity Status");
			SetStateRequest setStateRequest = new SetStateRequest();

			setStateRequest.EntityMoniker = new EntityReference(Entities.azuro_sms.EntityLogicalName, entity.Id);
			setStateRequest.State = new OptionSetValue((int)azuro_smsState.Scheduled);
			setStateRequest.Status = new OptionSetValue((int)azuro_sms_StatusCode.Scheduled);

			var response = GetOrganizationService(serviceProvider).Execute(setStateRequest);

			LogTrace(serviceProvider, "Leaving [DoUpdateExecution]");
		}

		private void InsertQueueItem(IServiceProvider serviceProvider, string queueName, object msg, Type msgType)
		{
			//	TODO: Logging in CRM
			LogTrace(serviceProvider, $"Adding message to MSMQ [{queueName}]");
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
			LogTrace(serviceProvider, $"Successfully added message to MSMQ [{queueName}]");
		}

		private azuro_smsconfiguration RetrieveSmsConfiguration(IServiceProvider serviceProvider, IOrganizationService service)
		{
			azuro_smsconfiguration cfg = null;
			LogTrace(serviceProvider, "Retrieving Configuration");
			var cfglist = GetEntityList<azuro_smsconfiguration>(service);
			if (cfglist.Count == 1)
			{
				LogTrace(serviceProvider, "configuration entity found");
				cfg = cfglist[0];
			}
			else if (cfglist.Count > 1)
			{
				LogTrace(serviceProvider, "There can only be one configuration entity");
			}

			LogTrace(serviceProvider, "Configuration - {0}", cfg != null ? "Obtained" : "Not Loaded");
			return cfg;
		}
	}
}
