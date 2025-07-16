using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Integration
{
	public abstract class ASetStatusActivity : ACrmCodeActivity
	{
		private const string InputEntityLogicalName = "Set the input entity's name in your derived class. (ReferenceTarget or AttributeTarget)";

		[Input("Replace with your Entity Type Description")]
		[ReferenceTarget(InputEntityLogicalName)]
		public abstract InArgument<EntityReference> InEntity { get; set; }

		[Input("New StateCode")]
		[AttributeTarget(InputEntityLogicalName, "statecode")]
		public abstract InArgument<OptionSetValue> InStateCode { get; set; }

		[Input("New StatusCode")]
		[AttributeTarget(InputEntityLogicalName, "statuscode")]
		public abstract InArgument<OptionSetValue> InStatusCode { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			if (!IsValid(context, InEntity))
				throw new ArgumentException("The entity supplied as an input is not a valid reference.");

			var reference = InEntity.Get(context);
			var stateCode = InStateCode.Get(context);
			var statusCode = InStatusCode.Get(context);
			var setStateRequest = new Microsoft.Crm.Sdk.Messages.SetStateRequest
			{
				EntityMoniker = reference,
				State = stateCode,
				Status = statusCode,
			};

			var ssResponse = GetOrganizationService(context).Execute(setStateRequest);
		}
	}

	public class SetContactStatus : ASetStatusActivity
	{
		private const string InputEntityLogicalName = "contact";

		[Input("Random Entity Status to Set")]
		[ReferenceTarget(InputEntityLogicalName)]
		public override InArgument<EntityReference> InEntity { get; set; }

		[Input("New StateCode")]
		[AttributeTarget(InputEntityLogicalName, "statecode")]
		public override InArgument<OptionSetValue> InStateCode { get; set; }

		[Input("New StatusCode")]
		[AttributeTarget(InputEntityLogicalName, "statuscode")]
		public override InArgument<OptionSetValue> InStatusCode { get; set; }
	}

	public class SetAccountStatus : ASetStatusActivity
	{
		private const string InputEntityLogicalName = "account";

		[Input("Random Entity Status to Set")]
		[ReferenceTarget(InputEntityLogicalName)]
		public override InArgument<EntityReference> InEntity { get; set; }

		[Input("New StateCode")]
		[AttributeTarget(InputEntityLogicalName, "statecode")]
		public override InArgument<OptionSetValue> InStateCode { get; set; }

		[Input("New StatusCode")]
		[AttributeTarget(InputEntityLogicalName, "statuscode")]
		public override InArgument<OptionSetValue> InStatusCode { get; set; }
	}
}
