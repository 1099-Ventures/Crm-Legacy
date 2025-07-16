using Azuro.Crm.Entities;
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
	public class CalculateEscalationDateActivity : ACrmCodeActivity
	{
		[Input("Support Contract Template (Input)")]
		[ReferenceTarget("contracttemplate")]
		[RequiredArgument]
		public InArgument<EntityReference> ContractTemplateReference { get; set; }

		[Input("Number of Minutes to Add")]
		[RequiredArgument]
		public InArgument<int> Minutes { get; set; }

		[Output("Wait Escalation DateTime (Output)")]
		public OutArgument<DateTime> EscalationDate { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			//	TODO: Update IsValid to take any InArgument?

			var crmHelper = GetCrmHelper(context);
			if (IsValid(context, ContractTemplateReference) /*&&IsValid(Minutes)*/)
			{
				var templateReference = this.ContractTemplateReference.Get(context);
				var template = crmHelper.GetEntity<Entities.ContractTemplate>(templateReference.Id);

				SupportCalendar calendar = new SupportCalendar(template.EffectivityCalendar);

				var minutes = Minutes.Get(context);
				minutes = minutes <= 0 ? 60 : minutes;

				var date = calendar.AddMinutes(DateTime.Now, minutes);

				EscalationDate.Set(context, date);
			}
			else
				throw new InvalidPluginExecutionException("Contract Template must be supplied.");
		}
	}
}
