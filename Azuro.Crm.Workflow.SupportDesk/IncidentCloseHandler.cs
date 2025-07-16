using Azuro.Crm.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Workflow
{
	public class IncidentCloseHandler : APlugin
	{
		public override void Execute(IServiceProvider serviceProvider)
		{
			GetTracingService(serviceProvider).Trace("{0} - Plugin has started.", this);

			var context = GetContext(serviceProvider);
			if (context.PrimaryEntityName != Azuro.Crm.Entities.Case.LogicalName)
				throw new Microsoft.Xrm.Sdk.InvalidPluginExecutionException(string.Format("Context.PrimaryEntityName: [{0}]", context.PrimaryEntityName));
			if (context.MessageName != "Close")
				throw new Microsoft.Xrm.Sdk.InvalidPluginExecutionException(string.Format("Context.MessageName: [{0}]", context.MessageName));

#if DEBUG
			foreach (var p in context.InputParameters)
			{
				GetTracingService(serviceProvider).Trace("{0} - {1}", p.Key, p.Value);
				if (p.Value is Microsoft.Xrm.Sdk.Entity)
				{
					foreach (var a in (((Microsoft.Xrm.Sdk.Entity)p.Value).Attributes))
						GetTracingService(serviceProvider).Trace("\t{0}: {1} - {2}", ((Microsoft.Xrm.Sdk.Entity)p.Value).LogicalName, a.Key, a.Value);
				}
			}
#endif

			var incidentResolution = (Microsoft.Xrm.Sdk.Entity)context.InputParameters["IncidentResolution"];
			GetTracingService(serviceProvider).Trace("Retrieving the incident: {0}", context.PrimaryEntityId);

			var incidentid = ((Microsoft.Xrm.Sdk.EntityReference)incidentResolution["incidentid"]).Id;

			var status = (Microsoft.Xrm.Sdk.OptionSetValue)context.InputParameters["Status"];
			GetTracingService(serviceProvider).Trace("Status: {0}", status.Value);

			var timespent = NotifyTimeSpentOnIncident(serviceProvider, incidentid);

			// Temporarily set the timespent to 15, in order to close the case. A later activity must then update it with the correct time.
			GetTracingService(serviceProvider).Trace("Setting time to {0}.", timespent);
			incidentResolution["timespent"] = timespent == 0 ? 15 : timespent;
		}

		private long NotifyTimeSpentOnIncident(IServiceProvider serviceProvider, Guid incidentid)
		{
			// Calculate the total number of minutes spent on an incident. 
			var calculateRequestTime = new Microsoft.Crm.Sdk.Messages.CalculateTotalTimeIncidentRequest
			{
				IncidentId = incidentid
			};
			var response =
				(Microsoft.Crm.Sdk.Messages.CalculateTotalTimeIncidentResponse)GetOrganizationService(serviceProvider).Execute(calculateRequestTime);

			GetTracingService(serviceProvider).Trace("{0} minutes have been spent on the incident.", response.TotalTime);

			return response.TotalTime;
		}
	}
}
