using Azuro.Crm.Online.Integration;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Workflow.Activities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Online.Workflow
{
	public class GetRecordId : ACrmCodeActivity
	{
		[Output("Record Id")]
		public OutArgument<string> RecordId { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			var wf = GetWorkflowContext(context);

			RecordId.Set(context, wf.PrimaryEntityId.ToString());
		}
	}
}