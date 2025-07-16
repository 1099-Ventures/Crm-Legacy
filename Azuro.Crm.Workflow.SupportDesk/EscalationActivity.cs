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
	public class EscalationActivity : ACrmCodeActivity
	{
		[Input("Entity Reference (Case) (Input)")]
		[ReferenceTarget("incident")]
		public InArgument<EntityReference> CaseReference { get; set; }

		[Output("Valid Escalation (Output)")]
		public OutArgument<bool> ValidEscalation { get; set; }

		[Output("Escalation Date (Output)")]
		public OutArgument<DateTime> EscalationDate { get; set; }

		[Output("Escalation Reason (Output)")]
		public OutArgument<string> EscalationReason { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			Trace(context, "Escalation Activity.OnExecute");
			if (CaseReference.Get(context).Id == Guid.Empty)
				throw new InvalidPluginExecutionException("No case provided as input");

			ValidEscalation.Set(context, false);
			var caseId = CaseReference.Get(context).Id;
			var result = CheckForEscalations(context, caseId);
			ValidEscalation.Set(context, result);
		}

		private bool CheckForEscalations(CodeActivityContext context, Guid caseId)
		{
			var crmHelper = GetCrmHelper(context);

			//	Retrieve case
			var caseItem = crmHelper.GetEntity<Entities.Case>(caseId);
			Trace(context, "Retrieved case: {0}", caseItem.TicketNumber);

			//	Confirm case is in valid state
			//	TODO: Make this configurable
			if (caseItem.StateCode != 0)
			{
				return false;
			}

			if (caseItem.StatusCode == 2 || caseItem.StatusCode == 334070003)
			{
				return false;
			}

			Entities.EscalationGroup escalationGroup = null;
			List<Entities.EscalationCondition> escalations = null;

			//	Retrieve contract
			if (CrmEntityReference.IsValid(caseItem.ContractId))
			{
				var contract = crmHelper.GetEntity<Entities.Contract>(caseItem.ContractId.ReferencedEntityId);
				Trace(context, "Retrieved contract: {0}", contract != null ? contract.ContractNumber : "Not Found");
				Trace(context, "Retrieving escalation group: {0}", contract.EscalationGroup);

				if (CrmEntityReference.IsValid(contract.EscalationGroup))
				{
					escalationGroup = crmHelper.GetEntity<Entities.EscalationGroup>(contract.EscalationGroup.ReferencedEntityId);
					Trace(context, "Retrieved escalation group: {0}", escalationGroup != null ? escalationGroup.Name : "Not Found");
					escalations = crmHelper.GetEntityList<Entities.EscalationCondition>(new List<KeyValuePair<string, object>> 
																							{
																								new KeyValuePair<string, object>("azuro_escalationgroupid", contract.EscalationGroup.ReferencedEntityId), 
																								new KeyValuePair<string, object>("azuro_currentseverity", caseItem.Severity), 
																							});

					var escalationReason = string.Format("Retrieved contract escalations : {0}", escalations != null ? escalations.Count.ToString() : "Not Found");
					Trace(context, escalationReason);
					EscalationReason.Set(context, escalationReason);
					return ProcessEscalation(context, crmHelper, caseItem, contract, escalationGroup, escalations);
				}
			}

			Trace(context, "Retrieving default EscalationGroup");
			escalationGroup = crmHelper.GetEntity<Entities.EscalationGroup>("azuro_isdefault", true);
			if (escalationGroup != null)
			{
				Trace(context, "Retrieved escalation group: {0}", escalationGroup.Name);
				escalations = crmHelper.GetEntityList<Entities.EscalationCondition>(new List<KeyValuePair<string, object>> 
																							{
																								new KeyValuePair<string, object>("azuro_escalationgroupid", escalationGroup.Id), 
																								new KeyValuePair<string, object>("azuro_currentseverity", caseItem.Severity), 
																							});
				var escalationReason = string.Format("Retrieved default escalations : {0}", escalations != null ? escalations.Count.ToString() : "Not Found");
				Trace(context, escalationReason);
				EscalationReason.Set(context, escalationReason);
				return ProcessEscalation(context, crmHelper, caseItem, null, escalationGroup, escalations);
			}
			else
			{
				Trace(context, "Escalation group Not Found!");
				throw new InvalidPluginExecutionException("No escalation group defined, and no or multiple default group(s) defined.");
			}
		}

		private bool ProcessEscalation(CodeActivityContext context, ICrmHelper crmHelper, Entities.Case caseItem, Entities.Contract contract, Entities.EscalationGroup escalationGroup, List<Entities.EscalationCondition> escalations)
		{
			Trace(context, "Enter ProcessEscalation");
			if (escalations.Count == 1)
			{
				var escalation = escalations[0];

				if (!escalation.ApplicableToAllCaseOrigins.GetValueOrDefault(true) && !CaseOriginHelper.IsValidOrigin(crmHelper, caseItem.CaseOriginCode, escalation.ApplicableCaseOrigins))
				{
					EscalationReason.Set(context, string.Format("Case Origin Not Valid: {0}", caseItem.CaseOriginCode));
					return false;
				}

				Entities.ContractTemplate contractTemplate = null;
				if (contract == null)
				{
					contractTemplate = crmHelper.GetEntity<Entities.ContractTemplate>(escalationGroup.DefaultContractTemplate.ReferencedEntityId);	//default from where?
				}
				else
				{
					contractTemplate = crmHelper.GetEntity<Entities.ContractTemplate>(contract.ContractTemplateId.ReferencedEntityId);
				}

				//	Check contract template calendar
				var calendar = new Entities.SupportCalendar(contractTemplate.EffectivityCalendar);

				//	Calculate next escalation date
				var escalateMinutes = escalationGroup.DefaultEscalationCondition ? escalations[0].MeanTimeToRepair : escalations[0].MeanTimeToRespond;
				var oldDate = caseItem.EscalationDate.HasValue && caseItem.EscalationDate != DateTime.MinValue && caseItem.EscalationDate != caseItem.CreatedOn ? caseItem.EscalationDate : caseItem.CreatedOn;
				var escalationDate = calendar.AddMinutes(oldDate.Value.ToLocalTime(), escalateMinutes);

				Trace(context, "Calculated escalation date: [{0}]", escalationDate);

				caseItem.FollowupBy = escalationDate;
				caseItem.NextSeverity = escalations[0].EscalationSeverity;
				//caseItem.EscalationDate = DateTime.Now;
				//caseItem.Severity = escalations[0].EscalationSeverity;

				var note = new Entities.Note
				{
					NoteText = string.Format("Case Escalation Changed \n\r From : {0} \n\r To : {1} \n\r Changed Severity: {2}", oldDate.Value.ToLocalTime(), escalationDate.ToLocalTime(), escalations[0].Name),
					ObjectId = new CrmEntityReference(Azuro.Crm.Entities.Case.LogicalName, caseItem.Title, caseItem.Id),
					Subject = "Escalation Changed",
					OwnerId = caseItem.OwnerId,
				};

				crmHelper.Update(caseItem);
				crmHelper.Insert(note);

				EscalationDate.Set(context, escalationDate);
				EscalationReason.Set(context, string.Format("Case Escalation Date Set: {0} | Next Severity: {1}", escalationDate, escalations[0].EscalationSeverity));

				return true;
			}

			EscalationReason.Set(context, string.Format("Escalation Count not Valid: {0}", escalations.Count));

			return false;
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void TestEscalations(Guid organizationId, Guid caseId)
		{
			//_crmHelper = CrmHelperFactory.Create(organizationId);
			//CheckForEscalations(caseId);
		}
	}
}
