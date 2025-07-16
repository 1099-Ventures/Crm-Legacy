using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Integration
{
	public class ConvertIntegerToTwoOptionsActivity : ACrmCodeActivity
	{
		[Input("Integer OptionSet Value")]
		public InArgument<int> InOptionValue { get; set; }

		[Output("Two Options Value")]
		public OutArgument<bool> OutTwoOptions { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			OutTwoOptions.Set(context, Convert.ToBoolean(InOptionValue.Get<int>(context)));
		}
	}
}
