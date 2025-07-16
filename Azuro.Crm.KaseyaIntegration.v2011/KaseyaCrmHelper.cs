using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common;
using Azuro.Crm.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Reflection;
using Azuro.Crm.Entities;

namespace Azuro.Crm.KaseyaIntegration
{
	//This class extend the CrmHelper class, because the are issues sending
	// the accountID for the Case.
	public class KaseyaCrmHelper
	{
		CrmHelper _crmHelper;
		public KaseyaCrmHelper(CrmHelper helper)
		{
			_crmHelper = helper;
		}

		public void Insert<T>(T entity) where T:CrmEntity
		{
			_crmHelper.Insert<T>(entity);
		}

		//public void InsertCaseEntity<T>(T caseEntity)
		//{
		//	var insertEntity = new Microsoft.Xrm.Sdk.Entity();
		//	insertEntity.LogicalName = GetEntityName<T>();
		//	CrmHelper.MapEntity<T>(caseEntity, insertEntity);
		//	SetEntityIDs<T>(caseEntity, ref insertEntity);
		//	var create = new CreateRequest { Target = insertEntity };
		//	// Create the request object.
		//	OrganizationResponse response = _crmHelper.OrganizationService.Execute(create);
		//	//TODO: Handle Response
		//}

		//private void SetEntityIDs<T>(T type, ref Microsoft.Xrm.Sdk.Entity entity)
		//{
		//	foreach (PropertyInfo pi in type.GetType().GetProperties())
		//	{
		//		if (pi.PropertyType == typeof(Guid))
		//		{

		//			//Set value for Cuntomer ID as is Required
		//			Guid _valueToSet = (Guid)pi.GetValue(type, null);

		//			if (_valueToSet != Guid.Empty)
		//			{
		//				CrmFieldAttribute fieled = AttributeHelper.GetCustomAttribute<CrmFieldAttribute>(pi);

		//				EntityReference _customerid = new EntityReference(LogicalEtityNames[fieled.Name], _valueToSet);
		//				entity[fieled.Name] = _customerid;
		//			}
		//		}
		//	}
		//}

		//private string GetEntityName<T>()
		//{
		//	CrmEntityAttribute entityAttribute = AttributeHelper.GetCustomAttribute<CrmEntityAttribute>(typeof(T));
		//	return entityAttribute != null ? entityAttribute.Name : null;
		//}

		public Guid GetAccountID(string customerName)
		{
			string key = "name";
			Entities.Account accountEntity = _crmHelper.GetEntity<Entities.Account>(key, customerName);
			return (accountEntity != null) ? accountEntity.Id : Guid.Empty;
		}

		public KaseyaCrmInfo GetKaseyaInfo(string groupName)
		{
			string key = "new_name";
			KaseyaCrmInfo kaseyaCrmInfo = _crmHelper.GetEntity<KaseyaCrmInfo>(key, groupName);
			return kaseyaCrmInfo;
		}

		public Guid GetContractID(string contractName)
		{
			string key = "title";
			Entities.Contract accountEntity = _crmHelper.GetEntity<Entities.Contract>(key, contractName);
			return (accountEntity != null) ? accountEntity.Id : Guid.Empty;
		}

		public Guid GetContractID(Guid accountID)
		{
			//string key = "customerid";
			Entities.Contract accountEntity = _crmHelper.GetEntity<Entities.Contract>(accountID);
			return (accountEntity != null) ? accountEntity.Id : Guid.Empty;
		}

		public ContractLine GetContractDetail(Guid contractID)
		{
			return _crmHelper.GetEntity<ContractLine>(contractID);
		}

		public Guid GetContractDetailID(Guid contractID)
		{
			var contractDetail = _crmHelper.GetEntity<ContractLine>(contractID);
			return (contractDetail != null) ? contractDetail.Id : Guid.Empty;
		}

		public Guid GetContractDetailID(string contracDetailName)
		{
			string key = "title";
			ContractLine accountEntity = _crmHelper.GetEntity<ContractLine>(key, contracDetailName);
			return (accountEntity != null) ? accountEntity.Id : Guid.Empty;
		}

		//		public Guid GetEntity(string accountSearchName)
		//		{
		//			string entityName = GetEntityName<KaseyaCrmAccount>();
		//			string keyName = "name";

		//			var fetch = @"<fetch mapping=""logical"">
		//							<entity name='" + entityName + @"'>
		//								<all-attributes/>
		//								<filter type='and'>
		//									<condition attribute='" + keyName + "' operator='eq' value='" + accountSearchName + @"' />
		//								</filter> 
		//							</entity> 
		//						</fetch>";
		//			_crmHelper.GetEntity(
		//		}

		//private Dictionary<string, string> _logicalEtityNames;
		//private Dictionary<string, string> LogicalEtityNames
		//{
		//	get
		//	{
		//		return _logicalEtityNames ?? (_logicalEtityNames = new Dictionary<string, string>(){
		//		{"customerid","account"},
		//		//{"contactid","contact"},
		//		{"contractid","contract"},
		//		//{"accountid","account"},
		//		{"contractdetailid", "contractdetail"},
		//		{"createdby", "systemuser"}
		//	}
		//			);
		//	}
		//}
	}


}
