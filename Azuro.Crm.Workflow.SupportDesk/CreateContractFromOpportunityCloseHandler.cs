using Azuro.Crm.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Workflow
{
	public class CreateContractFromOpportunityCloseHandler : APlugin
	{
		public override void Execute(IServiceProvider serviceProvider)
		{
			GetTracingService(serviceProvider).Trace("Entering CreateContractFromOpportunityCloseHandler");

			if (GetContext(serviceProvider).PrimaryEntityName != Opportunity.EntityLogicalName)
				return;
			if (GetContext(serviceProvider).MessageName != "Close")
				return;

			var opportunity = GetPrimaryEntity(serviceProvider, "opportunity");

			/*	TODO: Finish the plugin
			//	Create Contract based on Opportunity
			var contract = new Entities.Contract
									{
										Title = opportunity.Attributes["title"],
										BillingCustomerId = opportunity.Attributes["parentaccountid"],
										BillingAccountId = opportunity.Attributes["parentaccountid"],
										BillingContactId = opportunity.Attributes["parentcontactid"],
										AccountId = opportunity.Attributes["parentaccountid"],
										ContactId = opportunity.Attributes["parentcontactid"],
										ActiveOn = opportunity.Attributes["expectedstartdate"],
										BillingStartOn = opportunity.Attributes["expectedstartdate"],
										BillingEndOn = opportunity.Attributes["expectedenddate"],
										ExpiresOn = opportunity.Attributes["expectedenddate"],
										BillingFrequencyCode = 1, //	Monthly
										ContractManager = opportunity.Attributes["parentcontactid"],
										ContractTemplateId = Guid.Empty,	//	TODO: Figure out how to choose the contract type? Put it on opportunity?
									};

			CrmHelper.Insert(contract);

			//	Create Contract Lines based on Opportunity Products
			var opportunityProducts = CrmHelper.GetEntityList<Entities.OpportunityProduct>(new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("opportunityid", opportunity.Id), });
			foreach (var op in opportunityProducts)
			{
				var contractLine = new Entities.ContractLine
										{
											ContractId = contract.Id,
											ProductId = op.ProductId,
										};

				CrmHelper.Insert(contractLine);
			}
			*/
		}
	}
}
