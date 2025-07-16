using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;
using Azuro.Crm.Online.Integration;
using Microsoft.Crm.Sdk.Messages;

namespace Azuro.Crm.Online.Workflow
{
	public class ShareRecordActivity : ACrmCodeActivity
	{
		[Input("Parent Account (For Comparison to Determine whether to Share Record)")]
		[RequiredArgument]
		[ReferenceTarget("account")]
		public InArgument<EntityReference> ParentAccountParam { get; set; }

		[Input("Team (Team to Share with)")]
		[RequiredArgument]
		[ReferenceTarget("team")]
		public InArgument<EntityReference> TeamParam { get; set; }

		[Input("Account (To Check - Only 1 of Account / Contact / Case should be supplied)")]
		[ReferenceTarget("account")]
		public InArgument<EntityReference> AccountParam { get; set; }

		[Input("Contact (To Check - Only 1 of Account / Contact / Case should be supplied)")]
		[ReferenceTarget("contact")]
		public InArgument<EntityReference> ContactParam { get; set; }

		[Input("Case (To Check - Only 1 of Account / Contact / Case should be supplied)")]
		[ReferenceTarget("incident")]
		public InArgument<EntityReference> IncidentParam { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			IOrganizationService service = GetOrganizationService(context);

			var accountLookup = AccountParam.Get<EntityReference>(context);
			var contactLookup = ContactParam.Get<EntityReference>(context);
			var incidentLookup = IncidentParam.Get<EntityReference>(context);
			var teamLookup = TeamParam.Get<EntityReference>(context);
			var rootAccount = ParentAccountParam.Get<EntityReference>(context);

			if (accountLookup != null && contactLookup != null
				|| accountLookup != null && incidentLookup != null
				|| contactLookup != null && incidentLookup != null)
				throw new InvalidPluginExecutionException("Only one record should be set for sharing");

			if (incidentLookup != null)
			{
				CheckIncident(service, rootAccount, teamLookup, accountLookup, contactLookup, incidentLookup);
			}
			else if (contactLookup != null)
			{
				CheckContact(service, contactLookup, rootAccount, teamLookup, accountLookup, incidentLookup);
			}
			else if (accountLookup != null)
			{
				CheckAccount(service, rootAccount, teamLookup, null, accountLookup, contactLookup, incidentLookup, 1);
			}
		}

		private void SharePassedRecord(IOrganizationService service, EntityReference record, EntityReference team, AccessRights accessMask)
		{
			if (record.LogicalName == "incident")
				accessMask = AccessRights.ReadAccess | AccessRights.WriteAccess;

			var grantAccessRequest = new GrantAccessRequest
			{
				PrincipalAccess = new PrincipalAccess
				{
					AccessMask = accessMask,
					Principal = team,
				},
				Target = record,
			};
			service.Execute(grantAccessRequest);
		}

		private void CheckAccount(IOrganizationService service, EntityReference rootAccount, EntityReference teamLookup, EntityReference parent, EntityReference accountLookup, EntityReference contactLookup, EntityReference incidentLookup, int depth)
		{
			const int maxDepth = 5;
			if (depth > maxDepth)
				return;

			if (accountLookup.Id == rootAccount.Id)
			{
				SharePassedRecord(service, accountLookup, teamLookup, AccessRights.ReadAccess);
			}
			else if (parent != null)
			{
				if (parent.Id == rootAccount.Id)
				{
					var RefToShare = (incidentLookup != null) ? incidentLookup : (contactLookup != null) ? contactLookup : accountLookup;
					SharePassedRecord(service, RefToShare, teamLookup, AccessRights.ReadAccess);
				}
				else
				{
					Entity account = service.Retrieve(parent.LogicalName, parent.Id, new ColumnSet("accountid", "parentaccountid"));
					parent = account.GetAttributeValue<EntityReference>("parentaccountid");
					if (parent != null)
						CheckAccount(service, rootAccount, teamLookup, parent, accountLookup, contactLookup, incidentLookup, depth + 1);
				}
			}
			else
			{
				Entity account = service.Retrieve(accountLookup.LogicalName, accountLookup.Id, new ColumnSet("accountid", "parentaccountid"));
				parent = account.GetAttributeValue<EntityReference>("parentaccountid");
				if (parent != null)
					CheckAccount(service, rootAccount, teamLookup, parent, accountLookup, contactLookup, incidentLookup, depth + 1);
			}
		}

		private void CheckContact(IOrganizationService service, EntityReference rootAccount, EntityReference teamLookup, EntityReference accountLookup, EntityReference contactLookup, EntityReference incidentLookup)
		{
			Entity contact = service.Retrieve(contactLookup.LogicalName, contactLookup.Id, new ColumnSet("contactid", "parentcustomerid"));
			EntityReference parentRef = contact.GetAttributeValue<EntityReference>("parentcustomerid");
			if (parentRef != null)
			{
				if (parentRef.Id == rootAccount.Id)
				{
					var RefToShare = (incidentLookup != null) ? incidentLookup : contactLookup;
					SharePassedRecord(service, RefToShare, teamLookup, AccessRights.ReadAccess);
				}
				else
				{
					CheckAccount(service, rootAccount, teamLookup, null, accountLookup, contactLookup, incidentLookup, 1);
				}
			}
		}

		private void CheckIncident(IOrganizationService service, EntityReference rootAccount, EntityReference teamLookup, EntityReference accountLookup, EntityReference contactLookup, EntityReference incidentLookup)
		{
			Entity incident = service.Retrieve(incidentLookup.LogicalName, incidentLookup.Id, new ColumnSet("incidentid", "customerid"));
			EntityReference parentRef = incident.GetAttributeValue<EntityReference>("customerid");
			if (parentRef != null)
			{
				if (parentRef.Id == rootAccount.Id)
				{
					SharePassedRecord(service, incidentLookup, teamLookup, AccessRights.ReadAccess | AccessRights.WriteAccess);
				}
				else if (parentRef.LogicalName == "account")
				{
					CheckAccount(service, rootAccount, teamLookup, null, accountLookup, contactLookup, incidentLookup, 1);
				}
				else if (parentRef.LogicalName == "contact")
				{
					CheckContact(service, parentRef, rootAccount, teamLookup, accountLookup, incidentLookup);
				}
			}
		}
	}
}
