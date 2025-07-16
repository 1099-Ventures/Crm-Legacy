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
	public class ExtractTimeComponentActivity : ACrmCodeActivity
	{
		[Input("Date from which time component is taken (Input)")]
		public InArgument<DateTime> DateComponent { get; set; }

		[Output("Time Component (Output)")]
		public OutArgument<string> TimeComponent { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			Trace(context, "ExtractTimeComponentActivity.OnExecute");

			var time = DateComponent.Get(context).ToLocalTime().ToString("HH:mm:ss");

			TimeComponent.Set(context, time);

			Trace(context, "Leaving ExtractTimeComponentActivity.OnExecute");
		}
	}
}
