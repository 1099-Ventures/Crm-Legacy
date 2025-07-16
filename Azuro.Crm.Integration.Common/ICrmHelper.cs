using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Integration
{
	//	TODO: This interface might not 
	public interface ICrmHelper
	{
		Guid OrganisationId { get; set; }

		//	All the entities must be generic, in order for the interface to be useable for both CRM 4, 2011 and 2013
		string GetOrganisationUrl(Guid organisationId);
		string GetApplicationUrl(Guid organisationId);
		T GetEntity<T>(Guid entityId, bool throwOnError = false) where T : CrmEntity;
		T GetEntity<T>(string keyName, object keyValue, bool throwOnError = false) where T : CrmEntity;
		CrmEntity GetEntity(string entityName, string keyName, object keyValue, bool throwOnError = false);
		List<T> GetEntityList<T>() where T : CrmEntity;
		List<T> GetEntityList<T>(List<KeyValuePair<string, object>> filter) where T : CrmEntity;
		List<T> GetEntityList<T>(List<CrmQuery> filter, CrmLinkReference link = null) where T : CrmEntity;
		List<CrmEntity> GetEntityList(string entityName);
		List<CrmEntity> GetEntityList(string entityName, List<KeyValuePair<string, object>> filter);
		List<CrmEntity> GetEntityList(string entityName, List<CrmQuery> filter, CrmLinkReference link = null);
		void Update<T>(T entity) where T : CrmEntity;
		void Insert<T>(T entity) where T : CrmEntity;
		void SetStatus<T>(T entity) where T : CrmEntity;
		void Delete<T>(Guid id) where T : CrmEntity;
		TResponse Execute<TRequest, TResponse>(TRequest request)
			where TResponse : class
			where TRequest : class;
		int? GetOptionSetValueForText(string entityName, string attributeName, string label);
		int? GetOptionSetValueForText<T>(string attributeName, string label) where T : CrmEntity;
		int? GetOptionSetValueForText(string globalOptionSetName, string label);
		string GetOptionSetValueText<T>(string attribute, int optionValue);
	}
}
