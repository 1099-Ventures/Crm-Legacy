using Azuro.Crm.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace Azuro.Crm.Workflow
{
	public class IncidentCloseActivity : ACrmCodeActivity
	{
		[Input("Entity Reference (Case) (Input)")]
		[ReferenceTarget("incident")]
		public InArgument<EntityReference> CaseReference { get; set; }

		[Input("Case Resolution Description")]
		public InArgument<string> ResolutionDescription { get; set; }

		[Input("Case Resolution Time")]
		public InArgument<int> ResolutionTime { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			var caseId = CaseReference.Get(context).Id;
			if (caseId == Guid.Empty)
				throw new InvalidPluginExecutionException("No case provided as input");

			var resolution = ResolutionDescription.Get(context);
			var time = ResolutionTime.Get(context);

			Trace(context, "Resolution Description: {0}", resolution);
			Trace(context, "Resolution Time: {0}", time);

			var crmHelper = GetCrmHelper(context);

			var incidentResolution = crmHelper.GetEntity<Entities.CaseResolution>("incidentid", caseId);
			if (incidentResolution != null)
			{
				Trace(context, "Resolution Found: {0} : {1}, {2}, {3}", true, incidentResolution.ActualDurationMinutes, incidentResolution.StateCode, incidentResolution.TimeSpent);
				UpdateIncidentResolution(crmHelper, resolution, time, incidentResolution);
			}
			else
			{
				Trace(context, "Resolution Found: {0} ", false);
				// if incident resolution is null, entity cannot be updated so just return
			}
		}

		private static void UpdateIncidentResolution(ICrmHelper crmHelper, string resolution, int time, Entities.CaseResolution incidentResolution)
		{
			var statecode = incidentResolution.StateCode;
			var statuscode = incidentResolution.StatusCode;
			var restoreStatus = false;
			if (incidentResolution.StateCode != 0)	//	Case is closed, reopen, change and change back
			{
				incidentResolution.StateCode = 0;
				incidentResolution.StatusCode = 1;
				crmHelper.SetStatus(incidentResolution);
				restoreStatus = true;
			}

			incidentResolution.Description = resolution;

			incidentResolution.TimeSpent = (int)time;

			crmHelper.Update(incidentResolution);

			if (restoreStatus)
			{
				incidentResolution.StateCode = statecode;
				incidentResolution.StatusCode = statuscode;
				crmHelper.SetStatus(incidentResolution);
			}
		}
	}
}
