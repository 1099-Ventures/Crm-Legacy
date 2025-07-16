using Azuro.Crm.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Azuro.Crm.Workflow
{
	public class AutoNumberingActivity : ACrmCodeActivity
	{
		[Input("Dialog Process Name (Input)")]
		[ReferenceTarget("contact")]
		public InArgument<EntityReference> ContactInput { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			if (IsValid(context, ContactInput))
				DoContactAutoNumber(context);
		}

		private void DoContactAutoNumber(CodeActivityContext context)
		{
			var crmHelper = GetCrmHelper(context);
			var contact = crmHelper.GetEntity<Entities.Contact>(ContactInput.Get(context).Id);

			//	Fetch numbering entity
			var number = crmHelper.GetEntity("new_numbering", "new_name", Entities.Contact.LogicalName);
			if (number == null)
			{
				//	Create new numbering entity
				number = new CrmEntity();
				number.LogicalName = "new_numbering";
				number.Attributes["new_name"] = Entities.Contact.LogicalName;
				number.Attributes["new_lastnumber"] = 1;
				crmHelper.Insert(number);
			}
			else
			{
				var ii = (int)number.Attributes["new_lastnumber"];
				number.Attributes["new_lastnumber"] = ++ii;
				crmHelper.Update(number);
			}
		}
	}
}
