using Azuro.Crm.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Integration
{
	public class CaseHelper
	{
		private ICrmHelper CrmHelper { get; set; }
		public CaseHelper(ICrmHelper helper)
		{
			CrmHelper = helper;
		}

		public bool CreateCase(Entities.Account account, Entities.Contact contact, Entities.Case caseItem, CrmEntityReference owner)
		{
			//	Get support contract and line
			if (account != null)
			{
				GetSupportContract(account, caseItem);
				caseItem.AccountId = new CrmEntityReference(Entities.Account.LogicalName, account.Name, account.Id);
			}

			caseItem.OwnerId = owner;
			var statusCode = CrmHelper.GetOptionSetValueForText(Entities.Case.LogicalName, "statuscode", "New");
			if (statusCode.HasValue)
				caseItem.StatusCode = statusCode.Value;
			caseItem.CustomerId = (caseItem.AccountId != null) ? caseItem.AccountId : caseItem.ContactId;
			if (contact != null)
				caseItem.ResponsibleContactId = new CrmEntityReference("contact", "contactid", contact.Id); ;
			if (caseItem.CustomerId != null)
				CrmHelper.Insert(caseItem);
			else
				return false;

			return true;
		}

		public Microsoft.Xrm.Sdk.EntityCollection RetrieveOpenActivitiesForCase(Guid caseId)
		{
			//	Scan for all open activities relating to this case
			Microsoft.Xrm.Sdk.Query.QueryExpression query = new Microsoft.Xrm.Sdk.Query.QueryExpression { EntityName = "activitypointer", ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true) };
			
			query.Criteria.AddCondition("regardingobjectid", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, caseId);
			query.Criteria.AddCondition("statecode", Microsoft.Xrm.Sdk.Query.ConditionOperator.NotEqual, 1);
			//query.Criteria.AddCondition("statecode", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, 4);

			return  ((CrmHelper)CrmHelper).OrganizationService.RetrieveMultiple(query);
		}

		private void GetSupportContract(Entities.Account account, Entities.Case caseItem)
		{
			var contracts = CrmHelper.GetEntityList<Entities.Contract>(new List<CrmQuery>
																			{ 
																				new CrmQuery { Key = "accountid", Value = account.Id, FilterType = FilterType.And, Operator = FilterOperator.Equal, }, 
																				new CrmQuery { Key = "activeon", Value = DateTime.Today, FilterType = FilterType.And, Operator = FilterOperator.OnOrBefore, }, 
																				new CrmQuery { Key = "expireson", Value = DateTime.Now, FilterType = FilterType.And, Operator = FilterOperator.OnOrAfter, }, 
																				new CrmQuery { Key = "statuscode", Value = (int)ContractState.Invoiced, FilterType = FilterType.And, Operator = FilterOperator.Equal, }, 
																			});
			foreach (var contract in contracts)
			{
				if (contract.DefaultSeverity > 0)
				{
					caseItem.Severity = contract.DefaultSeverity;
					caseItem.ContractId = new CrmEntityReference(Contract.EntityLogicalName, contract.Title, contract.Id);
					var contractLines = CrmHelper.GetEntityList<Entities.ContractLine>(new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("contractid", contract.Id), });
					foreach (var contractline in contractLines)
					{
						if (contractline.AllotmentsRemaining > 0)
						{
							caseItem.ContractDetailId = new CrmEntityReference(Entities.ContractLine.LogicalName, contractline.Title, contractline.Id);
							break;
						}
					}
					break;
				}
			}
		}
	}
}
