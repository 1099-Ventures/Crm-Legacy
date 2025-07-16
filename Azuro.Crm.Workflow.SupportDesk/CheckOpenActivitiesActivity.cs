using Azuro.Crm.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Workflow
{
	public class CheckOpenActivitiesActivity : ACrmCodeActivity
	{
		[Input("Entity Reference (Case) (Input)")]
		[ReferenceTarget("incident")]
		public InArgument<EntityReference> CaseReference { get; set; }

		[Output("Case Has Open Activities")]
		public OutArgument<bool> HasOpenActivities { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			var caseId = CaseReference.Get(context).Id;
			if (caseId == Guid.Empty)
				throw new InvalidPluginExecutionException("No case provided as input");

			//	Scan for all open activities relating to this case
			var caseHelper = new CaseHelper(GetCrmHelper(context));
			var activities = caseHelper.RetrieveOpenActivitiesForCase(caseId);

			//	set output to true if activities found
			var hasOpenActivities = activities != null && activities.Entities != null && activities.Entities.Count > 0;
			HasOpenActivities.Set(context, hasOpenActivities);
		}
	}
}
