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
	public class CloseAllOpenActivitiesActivity : ACrmCodeActivity
	{
		[Input("Entity Reference (Case) (Input)")]
		[ReferenceTarget("incident")]
		public InArgument<EntityReference> CaseReference { get; set; }

		[Output("Number of Activities Closed")]
		public OutArgument<int> NumberOfActivitiesClosed { get; set; }

		[Output("Success")]
		public OutArgument<bool> Success { get; set; }

		[Output("Error Information")]
		public OutArgument<string> ErrorMessage { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			var caseId = CaseReference.Get(context).Id;
			if (caseId == Guid.Empty)
				throw new InvalidPluginExecutionException("No case provided as input");

			//	Scan for all open activities relating to this case
			var crmHelper = GetCrmHelper(context);
			var caseHelper = new CaseHelper(crmHelper);
			var activities = caseHelper.RetrieveOpenActivitiesForCase(caseId);
			var numOfActivities = 0;
			List<string> errorMessages = new List<string>();
			foreach (Entity a in activities.Entities)
			{
				try // try close all activities, and log the ones that fail.
				{
					if (a.LogicalName == "activitypointer")
					{
						if (a.Attributes.Contains("activityid") & a.Attributes.Contains("activitytypecode"))
						{
							string activitySchemaName = a.GetAttributeValue<string>("activitytypecode");

							try
							{
								switch (activitySchemaName.ToLower())
								{
									case "task":
										var resultTask = crmHelper.Execute<Microsoft.Crm.Sdk.Messages.SetStateRequest, Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest
										{
											EntityMoniker = new EntityReference(activitySchemaName, a.Id),
											State = new OptionSetValue(1),
											Status = new OptionSetValue(5)
										});
										break;
									case "call":
									case "phonecall":
										var resultCall = crmHelper.Execute<Microsoft.Crm.Sdk.Messages.SetStateRequest, Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest
										{
											EntityMoniker = new EntityReference(activitySchemaName, a.Id),
											State = new OptionSetValue(1),
											Status = new OptionSetValue(2)
										});
										break;
									case "appointment":
										var resultAppointment = crmHelper.Execute<Microsoft.Crm.Sdk.Messages.SetStateRequest, Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest
										{
											EntityMoniker = new EntityReference(activitySchemaName, a.Id),
											State = new OptionSetValue(1),
											Status = new OptionSetValue(3)
										});
										break;
									case "email":
									case "e-mail":
									case "fax":
									case "activity":
										var result = crmHelper.Execute<Microsoft.Crm.Sdk.Messages.SetStateRequest, Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest
										{
											EntityMoniker = new EntityReference(activitySchemaName, a.Id),
											State = new OptionSetValue(1),
											Status = new OptionSetValue(2)
										});
										break;
									default:
										var resultDefault = crmHelper.Execute<Microsoft.Crm.Sdk.Messages.SetStateRequest, Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest
										{
											EntityMoniker = new EntityReference(activitySchemaName, a.Id),
											State = new OptionSetValue(1),
											Status = new OptionSetValue(2)
										});
										break;
								}



							}
							catch (Exception)
							{
								//failed to close so try cancelling the acivity
								var result = crmHelper.Execute<Microsoft.Crm.Sdk.Messages.SetStateRequest, Microsoft.Crm.Sdk.Messages.SetStateResponse>(new Microsoft.Crm.Sdk.Messages.SetStateRequest
								{
									EntityMoniker = new EntityReference(activitySchemaName, a.Id),
									State = new OptionSetValue(2),
									Status = new OptionSetValue(-1)
								});
							}
						}
					}

					numOfActivities++;
				}
				catch (Exception ex)
				{
					errorMessages.Add(string.Format("{0} - {1} : {2} - {3}", a.LogicalName, a.Id, ex.Source, ex.Message));
				}
			}

			NumberOfActivitiesClosed.Set(context, numOfActivities);
			if (errorMessages.Count > 0)
			{
				Success.Set(context, false);
				var errorMessage = new StringBuilder();
				errorMessages.ForEach(s => errorMessage.AppendLine(s));
				ErrorMessage.Set(context, errorMessage.ToString());
			}
			else
			{
				Success.Set(context, true);
				ErrorMessage.Set(context, string.Empty);
			}
		}
	}
}
