using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.MSMQ;
using Microsoft.Crm.Sdk;

namespace Azuro.Crm.Plugin
{
	//	Receive Sms Event and post it the appropriate Msmq queue

	//	TODO: Remove forward defines for compile purposes
	internal class Entity { public string LogicalName { get; private set; } }
	internal interface IOrganizationServiceFactory { IOrganizationService CreateOrganizationService(Guid userId); }
	internal interface IOrganizationService { }

	public class OnUpdateHandler : IPlugin
	{
		public void Execute(IPluginExecutionContext context)
		{
			Entity entity;

			if (context.InputParameters != null)
			{
				if (context.InputParameters.Contains("Target") &&
					context.InputParameters["Target"] is Entity)
				{
					// Obtain the target business entity from the input parameters.
					entity = (Entity)context.InputParameters["Target"];

					// Verify that the entity represents a contact.
					if (entity.LogicalName != "new_sms") { return; }

					var id = (Guid)context.OutputParameters["id"];
					this.DoUpdateExecution(context, id);
				}
			}
		}

		public void DoUpdateExecution(IPluginExecutionContext context, Guid smsId)
		{
			QueueHelper.Insert(@".\private$\SmsTriggerQueue", new SmsMessages.SmsTriggerEventMessage { OrganisationId = context.OrganizationId, ActivityId = smsId, }, typeof(SmsMessages.SmsTriggerEventMessage));
		}
	}
}
