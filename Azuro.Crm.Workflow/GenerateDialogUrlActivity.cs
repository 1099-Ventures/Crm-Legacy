using Azuro.Crm.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Workflow
{
	public class GenerateDialogUrlActivity : ACrmCodeActivity
	{
		[Input("Source Entity Url (Related to Dialog) (Input)")]
		public InArgument<string> ObjectReferenceUrl { get; set; }

		[Input("Dialog Process Name (Input)")]
		public InArgument<string> DialogName { get; set; }

		[Output("Url (Output)")]
		public OutArgument<string> DialogUrl { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			//http://crmserver/AdventureWorksCycle/cs/dialog/rundialog.aspx?DialogId=9F53D2D8-AC54-46A6-A190-F23DE6677C65&EntityName=contact&ObjectId=41D1884E-B4B6-DF11-BF5E-00155DB05986 

			var url = ObjectReferenceUrl.Get(context);
			var dialogname = DialogName.Get(context);
			if (string.IsNullOrEmpty(url))
				throw new InvalidPluginExecutionException("No input Url provided.");

			if (string.IsNullOrEmpty(dialogname))
				throw new InvalidPluginExecutionException("No dialog specified.");

			//	Check the type of object?
			var entityNameIdx = url.IndexOf("EntityName=");
			var entityNameLength = "EntityName=".Length;
			var entityName = url.Substring(entityNameIdx + entityNameLength, url.IndexOf('&', entityNameIdx + 1) - entityNameIdx - entityNameLength);
			var entityId = url.Substring(url.IndexOf("ObjectId=") + "ObjectId=".Length);

			//	Get the dialog id?
			var crmHelper = GetCrmHelper(context);
			var dialog = crmHelper.GetEntity("workflow", "name", dialogname);
			if (dialog == null)
				throw new NullReferenceException(string.Format("Dialog with name [{0}] could not be found.", dialogname));
			var dialogId = dialog.Attributes["workflowid"];

			//	Build the Url
			var dialogUrl = string.Format("{0}/cs/dialog/rundialog.aspx?DialogId={1}&EntityName={2}&ObjectId={3}", crmHelper.GetApplicationUrl(GetWorkflowContext(context).OrganizationId), dialogId, entityName, entityId);
			DialogUrl.Set(context, dialogUrl);
		}
	}
}
