using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Security;
using Azuro.Crm.KaseyaIntegration;

namespace Azuro.Crm.KaseyaPlugin
{

	public class KaseyaTicketUpdateHandler : Azuro.Crm.Integration.APlugin
	{
		public override void Execute(IServiceProvider serviceProvider)
		{
			Entity entity;

			GetTracingService(serviceProvider).Trace("service has now been init");

			try
			{
				entity = (Entity)GetContext(serviceProvider).InputParameters["Target"];
				if (entity.LogicalName != "incident")
				{
					GetTracingService(serviceProvider).Trace("Entity not an incident.");
					return;
				}
			}
			catch (InvalidPluginExecutionException exc)
			{
				GetTracingService(serviceProvider).Trace("Error occured retrieving entity, {0}", exc.Message);
				throw new InvalidPluginExecutionException("Error occured retrieving entity," + exc.Message);
			}

			ColumnSet attributes = new ColumnSet(new string[] { "incidentid", "ticketnumber", "statuscode", "statecode" });
			entity = GetOrganizationService(serviceProvider).Retrieve(entity.LogicalName, entity.Id, attributes);

			//Check if it is Kaseya Ticket
			string ticketID = entity["ticketnumber"].ToString();
			if ((ticketID.Substring(0, ticketID.IndexOf('-'))) != "kaseyaticket")
			{
				GetTracingService(serviceProvider).Trace("Not a Kaseya ticket, ID must start with 'kaseyaticket' not '{0}'", ticketID.Substring(0, ticketID.IndexOf('-')));
				return;
			}

			SendTicketToMQ(serviceProvider, entity);
			//RemoveCachedData();
		}

		//public void RemoveCachedData()
		//{
		//	Context = null;
		//	ServiceProvider = null;
		//	TracingService = null;
		//}

		protected void SendTicketToMQ(IServiceProvider serviceProvider, Entity entity)
		{
			GetTracingService(serviceProvider).Trace("In the [SendEntityToMQ]");
			KaseyaCase kaseyaCase = new KaseyaCase();
			kaseyaCase.Id = entity.Id;
			kaseyaCase.statuscode = (CaseStatus)((OptionSetValue)entity["statuscode"]).Value;
			kaseyaCase.ticketnumber = entity["ticketnumber"].ToString();
			GetTracingService(serviceProvider).Trace("Found Status code");

			if (kaseyaCase.statuscode.ToString() != CaseStatus.Canceled.ToString())
			{
				if ((kaseyaCase.statuscode.ToString() != CaseStatus.Resolved.ToString()))
				{
					if (kaseyaCase.statuscode.ToString() != CaseStatus.Closed.ToString())
					{
						GetTracingService(serviceProvider).Trace("The Case is not yet resolves, {0}.", kaseyaCase.statuscode);
						GetTracingService(serviceProvider).Trace("Leaving [SendEntityToMQ]");
						return;
					}

				}
			}

			GetTracingService(serviceProvider).Trace("Sending Ticket to MSMQ.");
			InsertQueueItem(serviceProvider, @".\private$\KaseyaTicketTriggerQueue", new KaseyaTicketTriggerEventMessage { OrganisationId = GetContext(serviceProvider).OrganizationId, Incidentid = entity.Id, ticketnumber = kaseyaCase.ticketnumber }, typeof(KaseyaTicketTriggerEventMessage));
			GetTracingService(serviceProvider).Trace("Done[SendEntityToMQ]");
		}

		//protected ITracingService GetTracingService(IServiceProvider serviceProvider)
		//{
		//	ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
		//	if (tracingService == null)
		//		throw new InvalidPluginExecutionException("Failed to retrieve the Tracing Service.");

		//	return tracingService;
		//}

		//protected IOrganizationService GetOrganizationService(IServiceProvider serviceProvider)
		//{
		//	IOrganizationServiceFactory orgSvcFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
		//	if (orgSvcFactory == null)
		//		throw new InvalidPluginExecutionException("Failed to retrieve the Organization Service Factory.");
		//	IOrganizationService organizationService = orgSvcFactory.CreateOrganizationService(Context.UserId);
		//	if (organizationService == null)
		//		throw new InvalidPluginExecutionException("Failed to retrieve the Organization Service.");

		//	return organizationService;
		//}

		public void InsertQueueItem(IServiceProvider serviceProvider, string queueName, object msg, Type msgType)
		{
			GetTracingService(serviceProvider).Trace("In the [InsertQueueItem]");

			using (System.Messaging.MessageQueue mq = new System.Messaging.MessageQueue(queueName, System.Messaging.QueueAccessMode.Send))
			{
				GetTracingService(serviceProvider).Trace("MessageQueueu item created");
				using (System.Messaging.MessageQueueTransaction mqt = new System.Messaging.MessageQueueTransaction())
				{
					GetTracingService(serviceProvider).Trace("Transection Object created");
					System.Messaging.Message m = null;
					if (msgType != null)
						m = new System.Messaging.Message(msg, new System.Messaging.XmlMessageFormatter(new Type[] { msgType }));
					else
						m = new System.Messaging.Message(msg);

					mqt.Begin();
					GetTracingService(serviceProvider).Trace("Transection Began");
					mq.Send(m, mqt);
					GetTracingService(serviceProvider).Trace("Message send");
					mqt.Commit();
					GetTracingService(serviceProvider).Trace("Transection commited");
					mq.Close();
					GetTracingService(serviceProvider).Trace("Done");
				}
			}
		}
	}
}
