using Azuro.Common;
using Azuro.Common.Configuration;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel.Description;

namespace Azuro.Crm.Integration
{
    public sealed class CrmHelper : ICrmHelper
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private ServerConnection _connection = null;
        private ServerConnection Connection { get { return _connection ?? (_connection = new ServerConnection()); } }

        private IDiscoveryService _discoveryService = null;
        public IDiscoveryService DiscoveryService { get { return _discoveryService ?? (_discoveryService = CreateDiscoveryService()); } }

        private IOrganizationService _organizationService = null;
        public IOrganizationService OrganizationService
        {
            //get { return _organizationService ?? (_organizationService = CreateOrganizationService()); }
            //	Seems the org service has a limited lifespan :(
            get { return CheckOrganizationService() ? _organizationService : CreateOrganizationService(); }
            set { _organizationService = value; }
        }

        private string DiscoveryServiceUrl { get { return Config.DiscoveryServiceUrl ?? "http://localhost:5555/XRMServices/2011/Discovery.svc"; } }

        private CrmConfigurationSection _config;
        private CrmConfigurationSection Config { get { return _config ?? (_config = ConfigurationHelper.GetSection<CrmConfigurationSection>(CrmConfigurationSection.SectionName) ?? new CrmConfigurationSection()); } }

        public Guid OrganisationId { get; set; }

        public string GetOrganisationUrl()
        {
            return GetOrganisationUrl(OrganisationId);
        }

        public string GetOrganisationUrl(Guid organisationId)
        {
            OrganizationDetail org = GetOrganisationDetail(organisationId);

            return org != null ? org.Endpoints[EndpointType.OrganizationService] : null;
        }

        public string GetApplicationUrl(Guid organisationId)
        {
            OrganizationDetail org = GetOrganisationDetail(organisationId);

            return org != null ? org.Endpoints[EndpointType.WebApplication] : null;
        }

        private OrganizationDetail GetOrganisationDetail(Guid organisationId)
        {
            OrganizationDetailCollection orgs = Connection.DiscoverOrganizations(DiscoveryService);
            if (orgs == null || orgs.Count == 0)
                throw new ArgumentOutOfRangeException("orgs", string.Format("orgs returned {0}", orgs == null ? "null" : "0"));

            //	find the Guid
            OrganizationDetail org = null;
            string orgids = Environment.NewLine;
            foreach (OrganizationDetail o in orgs)
            {
                orgids += o.UniqueName + " - " + o.OrganizationId.ToString() + Environment.NewLine;
                if (o.OrganizationId.CompareTo(organisationId) == 0)
                {
                    org = o;
                    break;
                }
            }

            if (org == null)
                throw new ArgumentNullException("Org", string.Format("Unable to find OrganisationId '{0}' on Url '{1}'{2}", organisationId, Config.DiscoveryServiceUrl, orgids));

            return org;
        }

        private IDiscoveryService CreateDiscoveryService()
        {
            Uri uri = null, homeRealmUri = null;
            ClientCredentials clientCredentials = null, deviceCredentials = null;

            uri = new Uri(Config.DiscoveryServiceUrl);
            clientCredentials = CreateClientCredentials();

            return new DiscoveryServiceProxy(uri, homeRealmUri, clientCredentials, deviceCredentials);
        }

        private bool CheckOrganizationService()
        {
			return false;

			//	TODO: The below code is failing, workaround is to force new OrgService each time.

            //	Taken from the AutoRefreshSecurityToken sample in the CRM SDK
            //if (_organizationService == null)
            //    return false;
            //else
            //{
            //    //	Try renew security token if it's near expiry
            //    var orgProxy = _organizationService as OrganizationServiceProxy;
            //    if (orgProxy != null)
            //    {
            //        Logger.Trace("CRMHelper: OrgProxy is not NULL.");
            //        if (orgProxy.SecurityTokenResponse != null && orgProxy.SecurityTokenResponse.Response.Lifetime.Expires <= DateTime.UtcNow.AddMinutes(30))
            //        {
            //            try
            //            {
            //                orgProxy.Authenticate();
            //            }
            //            catch (Exception ex)
            //            {
            //                //	Reset all the service types.
            //                _connection = null;
            //                _discoveryService = null;
            //                if (orgProxy.SecurityTokenResponse == null || orgProxy.SecurityTokenResponse.Response.Lifetime.Expires <= DateTime.UtcNow)
            //                {
            //                    Logger.Info("Token expired.");
            //                    return false;
            //                }

            //                // Ignore the exception 
            //                Logger.Error(ex, "An exception ocurred while reauthenticating the organization service.");
            //                return false;

            //            }
            //        }
            //    }
            //}
            ////	Continue if the _organizationService is not null
            //return true;
        }

        private IOrganizationService CreateOrganizationService()
        {
            Uri uri = null, homeRealmUri = null;
            ClientCredentials clientCredentials = null, deviceCredentials = null;

            uri = new Uri(GetOrganisationUrl(OrganisationId));
            clientCredentials = CreateClientCredentials();

            return new OrganizationServiceProxy(uri, homeRealmUri, clientCredentials, deviceCredentials);
        }

        private ClientCredentials CreateClientCredentials()
        {
            ClientCredentials clientCredentials = new ClientCredentials();
            if (Config.AuthenticationType == Azuro.Crm.Integration.CrmConfigurationSection.CrmAuthenticationType.IFD)
            {
                clientCredentials.UserName.UserName = Config.UserName;
                clientCredentials.UserName.Password = Config.Password;
            }
            else
            {
                clientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                clientCredentials.Windows.ClientCredential = (NetworkCredential)CredentialCache.DefaultNetworkCredentials;
            }

            return clientCredentials;
        }

        public List<CrmEntity> GetEntityList(string entityName)
        {
            return GetEntityList(entityName, (List<CrmQuery>)null);
        }

        public List<CrmEntity> GetEntityList(string entityName, List<KeyValuePair<string, object>> filter)
        {
            List<CrmQuery> query = new List<CrmQuery>(filter.Count);
            query.AddRange(filter.ConvertAll<CrmQuery>(a => { return new CrmQuery { Key = a.Key, Value = a.Value, Operator = FilterOperator.Equal, FilterType = FilterType.And }; }));
            return GetEntityList(entityName, query);
        }

        public List<CrmEntity> GetEntityList(string entityName, List<CrmQuery> filter, CrmLinkReference link = null)
        {
            var fetch = BuildFetchQuery(entityName, filter, link);
            return GetEntityList<CrmEntity>(fetch);
        }

        public List<T> GetEntityList<T>() where T : CrmEntity
        {
            return GetEntityList<T>((List<CrmQuery>)null);
        }

        public List<T> GetEntityList<T>(List<KeyValuePair<string, object>> filter) where T : CrmEntity
        {
            List<CrmQuery> query = new List<CrmQuery>(filter.Count);
            query.AddRange(filter.ConvertAll<CrmQuery>(a => { return new CrmQuery { Key = a.Key, Value = a.Value, Operator = FilterOperator.Equal, FilterType = FilterType.And }; }));
            return GetEntityList<T>(query);
        }

        public List<T> GetEntityList<T>(List<CrmQuery> filter, CrmLinkReference link = null) where T : CrmEntity
        {
            var fetch = BuildFetchQuery(GetEntityName<T>(), filter, link);
            return GetEntityList<T>(fetch);
        }

        private string BuildFetchQuery(string entityName, List<CrmQuery> filter, CrmLinkReference link)
        {
            var fetch = "<fetch mapping='logical'>" + Environment.NewLine
                        + "\t<entity name='" + entityName + "'>" + Environment.NewLine
                        + "\t\t<all-attributes/>" + Environment.NewLine;

            if (link != null)
                fetch += string.Format("\t\t<link-entity name='{0}' from='{1}' to='{2}' visible='false' intersect='true'>{3}",
                                            link.LinkName,
                                            link.FromAttribute,
                                            link.ToAttribute,
                                            Environment.NewLine);

            var filtertype = string.Empty;
            bool open = false;
            if (filter != null)
            {
                foreach (var exp in filter)
                {
                    if (filtertype != exp.FilterType.ToString())
                    {
                        filtertype = exp.FilterType.ToString();
                        if (open)
                            fetch += "\t\t</filter>" + Environment.NewLine;
                        fetch += string.Format("\t\t\t<filter type='{0}'>{1}", AttributeHelper.GetFieldAttribute<System.ComponentModel.DefaultValueAttribute>(typeof(FilterType), exp.FilterType.ToString()).Value, Environment.NewLine);
                        open = true;
                    }

                    fetch += string.Format("\t\t\t\t<condition attribute='{0}' operator='{1}' value='{2}' />{3}",
                                                exp.Key,
                                                AttributeHelper.GetFieldAttribute<System.ComponentModel.DefaultValueAttribute>(typeof(FilterOperator), exp.Operator.ToString()).Value,
                                                exp.Value,
                                                Environment.NewLine);
                }

                if (open)
                    fetch += "\t\t</filter>" + Environment.NewLine;
            }

            if (link != null)
                fetch += "\t\t</link-entity>" + Environment.NewLine;

            fetch += "\t</entity>" + Environment.NewLine
                    + "</fetch>";

            Logger.Trace("Fetch Query: {0}{1}", Environment.NewLine, fetch);

            return fetch;
        }

        private List<T> GetEntityList<T>(string fetch) where T : CrmEntity
        {
            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            var result = ((RetrieveMultipleResponse)OrganizationService.Execute(fetchRequest)).EntityCollection;
            List<T> list = new List<T>(result.Entities.Count);
            foreach (var entity in result.Entities)
            {
                T item = Util.CreateObject<T>(typeof(T));
                MapFields(this, item, entity);
                MapEntityAttributes(GetEntityName<T>(), item as CrmEntity, entity);
                list.Add(item);
            }

            return list;
        }

        public T GetEntity<T>(Guid entityId, bool throwOnError = false) where T : CrmEntity
        {
            string entityName = GetEntityName<T>();

            var fetch = @"<fetch mapping=""logical"">
							<entity name='" + entityName + @"'>
								<all-attributes/>
								<filter type='and'>  ";

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                CrmFieldAttribute field = AttributeHelper.GetCustomAttribute<CrmFieldAttribute>(pi);
                if (field != null && field.IsIdentity)
                    fetch += "<condition attribute='" + field.Name + "' operator='eq' value='" + entityId + "' /> ";
            }

            fetch += @"			</filter> 
							</entity> 
						</fetch>";

            return GetEntity<T>(fetch, throwOnError);
        }

        public T GetEntity<T>(string keyName, object keyValue, bool throwOnError = false) where T : CrmEntity
        {
            string entityName = GetEntityName<T>();

            var fetch = @"<fetch mapping=""logical"">
							<entity name='" + entityName + @"'>
								<all-attributes/>
								<filter type='and'>
									<condition attribute='" + keyName + "' operator='eq' value='" + keyValue + @"' />
								</filter> 
							</entity> 
						</fetch>";

            return GetEntity<T>(fetch, throwOnError);
        }

        public CrmEntity GetEntity(string entityName, string keyName, object keyValue, bool throwOnError = false)
        {
            var fetch = @"<fetch mapping=""logical"">
							<entity name='" + entityName + @"'>
								<all-attributes/>
								<filter type='and'>
									<condition attribute='" + keyName + "' operator='eq' value='" + keyValue + @"' />
								</filter> 
							</entity> 
						</fetch>";

            // Excute the fetch query and get the xml result.
            var result = OrganizationService.RetrieveMultiple(new FetchExpression(fetch));
            if (IsValidGet(result, throwOnError))
            {
                var singleResult = result.Entities[0];

                CrmEntity entity = new CrmEntity();
                MapFields(this, entity, singleResult);
                MapEntityAttributes(entityName, entity, singleResult);
                return entity;
            }

            return null;
        }

        private T GetEntity<T>(string fetch, bool throwOnError) where T : CrmEntity
        {
            // Excute the fetch query and get the xml result.
            var result = OrganizationService.RetrieveMultiple(new FetchExpression(fetch));
            if (IsValidGet(result, throwOnError))
            {
                T item = Util.CreateObject<T>(typeof(T));
                var singleResult = result.Entities[0];

                MapFields(this, item, singleResult);
                MapEntityAttributes(GetEntityName<T>(), item as CrmEntity, singleResult);
                item.UpdatedAttributes.Clear();

                return item;
            }

            return default(T);
        }

        private bool IsValidGet(EntityCollection result, bool throwOnError)
        {
            if (result.Entities.Count == 1)
                return true;
            else if (throwOnError)
            {
                if (result.Entities.Count == 0)
                    throw new InvalidOperationException("No data return by the Get Operation.");
                else
                    throw new InvalidOperationException("Multiple results from the Get Operation where only one was expected.");
            }

            return false;
        }

        public void Update<T>(T entity) where T : CrmEntity
        {
            var updateEntity = new Microsoft.Xrm.Sdk.Entity();
            updateEntity.LogicalName = GetEntityName<T>();

            if (updateEntity.LogicalName != null)
            {
                MapEntity<T>(entity, updateEntity, true);
                entity.UpdatedAttributes.Clear();

                // Create the request object.
                var update = new UpdateRequest { Target = updateEntity, };

                // Create the request object.
                OrganizationResponse response = OrganizationService.Execute(update);
                //	TODO: Handle Response

                var count = response.Results.Count;
            }
            else
                throw new InvalidOperationException(string.Format("Object {0} does not have the CrmEntityAttribute set.", entity));
        }

        public void Insert<T>(T entity) where T : CrmEntity
        {
            var insertEntity = new Microsoft.Xrm.Sdk.Entity();
            insertEntity.LogicalName = GetEntityName<T>();
            MapEntity<T>(entity, insertEntity);
            var create = new CreateRequest { Target = insertEntity };
            // Create the request object.
            OrganizationResponse response = OrganizationService.Execute(create);
            //	TODO: Handle Response

            PropertyInfo pi = typeof(T).GetProperty("Id");
            if (pi != null && response.Results != null && response.Results.Count > 0 && response.Results.Contains("id"))
                pi.SetValue(entity, response.Results["id"], null);
        }

        public void SetStatus<T>(T entity) where T : CrmEntity
        {
            //	Update SMS Activity Status
            SetStateRequest setStateRequest = new SetStateRequest();

            setStateRequest.EntityMoniker = new EntityReference(GetEntityName<T>(), GetEntityIdentity(entity));
            setStateRequest.State = new OptionSetValue((int)GetEntityState(entity));
            setStateRequest.Status = new OptionSetValue((int)GetEntityStatus(entity));

            var response = (SetStateResponse)OrganizationService.Execute(setStateRequest);
        }

        public string GetOptionSetValueText(string entityName, string attribute, int optionValue)
        {
            return GetOptionSetValueText(this, entityName, attribute, optionValue);
        }

        private static string GetOptionSetValueText(CrmHelper helper, string entityName, string attribute, int optionValue)
        {
            string optionLabel = String.Empty;
            RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attribute,
                RetrieveAsIfPublished = true
            };

            RetrieveAttributeResponse attributeResponse = (RetrieveAttributeResponse)helper.OrganizationService.Execute(attributeRequest);
            AttributeMetadata attrMetadata = (AttributeMetadata)attributeResponse.AttributeMetadata;
            if (attrMetadata is PicklistAttributeMetadata)
            {
                PicklistAttributeMetadata picklistMetadata = (PicklistAttributeMetadata)attrMetadata;
                // For every status code value within all of our status codes values
                //  (all of the values in the drop down list)
                foreach (OptionMetadata optionMeta in picklistMetadata.OptionSet.Options)
                {
                    // Check to see if our current value matches
                    if (optionMeta.Value == optionValue)
                    {
                        // If our numeric value matches, set the string to our status code label
                        optionLabel = optionMeta.Label.UserLocalizedLabel.Label;
                        break;
                    }
                }
            }
            else if (attrMetadata is StateAttributeMetadata)
            {
                StateAttributeMetadata stateMetadata = (StateAttributeMetadata)attrMetadata;
                // For every status code value within all of our status codes values
                //  (all of the values in the drop down list)
                foreach (OptionMetadata optionMeta in stateMetadata.OptionSet.Options)
                {
                    // Check to see if our current value matches
                    if (optionMeta.Value == optionValue)
                    {
                        // If our numeric value matches, set the string to our status code label
                        optionLabel = optionMeta.Label.UserLocalizedLabel.Label;
                        break;
                    }
                }
            }
            else if (attrMetadata is StatusAttributeMetadata)
            {
                StatusAttributeMetadata statusMetadata = (StatusAttributeMetadata)attrMetadata;
                // For every status code value within all of our status codes values
                //  (all of the values in the drop down list)
                foreach (OptionMetadata optionMeta in statusMetadata.OptionSet.Options)
                {
                    // Check to see if our current value matches
                    if (optionMeta.Value == optionValue)
                    {
                        // If our numeric value matches, set the string to our status code label
                        optionLabel = optionMeta.Label.UserLocalizedLabel.Label;
                        break;
                    }
                }
            }
            return optionLabel;
        }

        public string GetOptionSetValueText<T>(string attribute, int optionValue)
        {
            return GetOptionSetValueText(GetEntityName<T>(), attribute, optionValue);
        }

        private static string GetEntityName<T>()
        {
            CrmEntityAttribute entityAttribute = AttributeHelper.GetCustomAttribute<CrmEntityAttribute>(typeof(T));
            return entityAttribute != null ? entityAttribute.Name : null;
        }

        private Guid GetEntityIdentity<T>(T entity)
        {
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                CrmFieldAttribute field = AttributeHelper.GetCustomAttribute<CrmFieldAttribute>(pi);
                if (field != null && field.IsIdentity && pi.PropertyType == typeof(Guid))
                    return (Guid)pi.GetValue(entity, null);
            }

            return Guid.Empty;
        }

        private int GetEntityState<T>(T entity)
        {
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                CrmFieldAttribute field = AttributeHelper.GetCustomAttribute<CrmFieldAttribute>(pi);
                if (field != null && field.Name == "statecode")
                    return (int)pi.GetValue(entity, null);
            }

            return -1;
        }

        private int GetEntityStatus<T>(T entity)
        {
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                CrmFieldAttribute field = AttributeHelper.GetCustomAttribute<CrmFieldAttribute>(pi);
                if (field != null && field.Name == "statuscode")
                    return (int)pi.GetValue(entity, null);
            }

            return -1;
        }

        public static Microsoft.Xrm.Sdk.Entity MapEntity<T>(T item, Microsoft.Xrm.Sdk.Entity entity, bool forUpdate = false) where T : CrmEntity
        {
            foreach (PropertyInfo pi in item.GetType().GetProperties())
            {
                CrmFieldAttribute field = AttributeHelper.GetCustomAttribute<CrmFieldAttribute>(pi);
                if (field != null)
                {
                    if (forUpdate && !(field.IsIdentity || item.UpdatedAttributes.ContainsKey(field.Name)))
                        continue;
                    var value = pi.GetValue(item, null);
                    if (value == null)
                        continue;
                    if (entity.Attributes.Contains(field.Name))
                        entity.Attributes[field.Name] = SafeSetValue(entity.Attributes[field.Name].GetType(), pi.GetValue(item, null), field);
                    else if (IsValidValue(pi.PropertyType, value))
                        entity.Attributes.Add(new KeyValuePair<string, object>(field.Name, SafeSetValue(pi.PropertyType, value, field)));
                }
            }
            return entity;
        }

        private static bool IsValidValue(Type type, object value)
        {
            return !(type == typeof(int) && (int)value == 0)
                    && !(type == typeof(DateTime) && (DateTime)value == DateTime.MinValue)
                    && !(type == typeof(Guid) && (Guid)value == Guid.Empty);
        }

        public static T MapFields<T>(CrmHelper helper, T item, Microsoft.Xrm.Sdk.Entity singleResult) where T : CrmEntity
        {
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (pi.Name == "RelatedEntities" && singleResult.RelatedEntities.Count > 0)
                {
                    //	TODO: This needs to be done with a Dictionary and some more smarts ... :)
                    var relatedEntities = new List<CrmEntityReference>();
                    foreach (var relatedEntity in singleResult.RelatedEntities)
                        relatedEntities.Add(new CrmEntityReference(relatedEntity.Key.SchemaName, relatedEntity.Value.EntityName, relatedEntity.Value.Entities[0].Id));
                    pi.SetValue(item, relatedEntities, null);
                }
                var field = AttributeHelper.GetCustomAttribute<CrmFieldAttribute>(pi);
                if (field != null && singleResult.Contains(field.Name))
                    pi.SetValue(item, SafeTypeConvert(helper, singleResult[field.Name], pi.PropertyType, pi.Name), null);
            }
            item.UpdatedAttributes.Clear();
            return item;
        }

        private CrmEntity MapEntityAttributes(string entityName, CrmEntity entity, Microsoft.Xrm.Sdk.Entity singleResult)
        {
            return MapEntityAttributes(this, entityName, entity, singleResult);
        }

        private static CrmEntity MapEntityAttributes(CrmHelper helper, string entityName, CrmEntity entity, Microsoft.Xrm.Sdk.Entity singleResult)
        {
            if (entity != null)
            {
                entity.LogicalName = entityName;
                foreach (var a in singleResult.Attributes)
                {
                    if (a.Value is OptionSetValue)
                    {
                        entity.Attributes.Add(a.Key, new KeyValuePair<int, string>(((OptionSetValue)a.Value).Value, GetOptionSetValueText(helper, entityName, a.Key, ((OptionSetValue)a.Value).Value)));
                    }
                    else
                    {
                        entity.Attributes.Add(a.Key, SafeTypeConvert(helper, a.Value, typeof(object), a.Key));
                    }
                }
            }
            return entity;
        }

        private object SafeTypeConvert(object crmValue, Type type, string name)
        {
            return SafeTypeConvert(this, crmValue, type, name);
        }

        private static object SafeTypeConvert(CrmHelper helper, object crmValue, Type type, string name)
        {
            if (crmValue == null)
                return null;

            try
            {
                if (crmValue is Microsoft.Xrm.Sdk.OptionSetValue)
                {
                    //IsValidCast(name, typeof(Microsoft.Xrm.Sdk.OptionSetValue), type);

                    //	TODO: Properly Parse the Enum, just in case
                    return ((Microsoft.Xrm.Sdk.OptionSetValue)crmValue).Value;
                }
                else if (crmValue is Microsoft.Xrm.Sdk.Money)
                {
                    //IsValidCast(name, typeof(Microsoft.Xrm.Sdk.Money), type);

                    return ((Microsoft.Xrm.Sdk.Money)crmValue).Value;
                }
                else if (crmValue is Microsoft.Xrm.Sdk.EntityReference)
                {
                    //IsValidCast(name, typeof(Microsoft.Xrm.Sdk.EntityReference), type);

                    EntityReference er = (Microsoft.Xrm.Sdk.EntityReference)crmValue;
                    return new CrmEntityReference(er.LogicalName, er.Name, er.Id);
                }
                else if (crmValue is Microsoft.Xrm.Sdk.EntityCollection)
                {
                    var retList = new List<CrmEntity>(((Microsoft.Xrm.Sdk.EntityCollection)crmValue).Entities.Count);
                    foreach (var entity in ((Microsoft.Xrm.Sdk.EntityCollection)crmValue).Entities)
                    {
                        var crmEntity = new CrmEntity { LogicalName = entity.LogicalName, };
                        retList.Add(MapEntityAttributes(helper, entity.LogicalName, crmEntity, entity));
                    }
                    return retList;
                }
                else
                {
                    return Util.ChangeType(crmValue, type);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(string.Format("Attempting to convert [{0}] with value [{1}] from [{2}] to [{3}]", name, crmValue, crmValue.GetType(), type), ex);
            }
        }

        ////	Fix this :S
        //private static void IsValidCast(string name, Type typeFrom, Type typeTo)
        //{
        //	if (typeFrom == typeof(Microsoft.Xrm.Sdk.OptionSetValue))
        //		typeFrom = typeof(int);
        //	else if (typeFrom == typeof(Microsoft.Xrm.Sdk.Money))
        //		typeFrom = typeof(decimal);
        //	else if (typeFrom == typeof(Microsoft.Xrm.Sdk.EntityReference) && typeTo == typeof(CrmEntityReference))
        //		return;
        //	else if (!typeTo.IsAssignableFrom(typeFrom))
        //		throw new InvalidCastException(string.Format("Attempting to convert [{0}] from [{1}] to [{2}]", name, typeFrom, typeTo));
        //}

        private static object SafeSetValue(Type toType, object setValue, CrmFieldAttribute field)
        {
            if (setValue == null)
                return null;

            try
            {
                var reference = setValue as CrmEntityReference;
                if (reference != null)
                    return new EntityReference(reference.EntityName, reference.ReferencedEntityId);
                else
                {
                    var listReference = setValue as List<CrmEntity>;
                    if (listReference != null)
                        return new EntityCollection(((List<CrmEntity>)setValue).ConvertAll<Microsoft.Xrm.Sdk.Entity>(a => MapEntity(a, new Microsoft.Xrm.Sdk.Entity(a.LogicalName))));
                    else if ((setValue is Enum || setValue is Int32) && (toType == typeof(Microsoft.Xrm.Sdk.OptionSetValue)
                        || typeof(Enum).IsAssignableFrom(toType))
                        || field.IsPicklist)
                        return new Microsoft.Xrm.Sdk.OptionSetValue((int)setValue);
                    else if (setValue is Decimal)
                        return new Microsoft.Xrm.Sdk.Money((decimal)setValue);
                    else
                        return Util.ChangeType(setValue, toType);
                }
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException(string.Format("Attempting to set [{0}] with value [{1}] to Type [{2}]", setValue, toType, field.Name), ex);
            }
        }

        public int? GetOptionSetValueForText<T>(string attributeName, string label) where T : CrmEntity
        {
            return GetOptionSetValueForText(GetEntityName<T>(), attributeName, label);
        }

        public int? GetOptionSetValueForText(string entityName, string attributeName, string label)
        {
            var request = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveAttributeResponse)OrganizationService.Execute(request);
            var picklist = response.AttributeMetadata as EnumAttributeMetadata;

            if (picklist != null)
                return GetOptionSetValueForText(picklist.OptionSet, label);

            return null;
        }

        public int? GetOptionSetValueForText(string globalOptionSetName, string label)
        {
            var request = new RetrieveOptionSetRequest
            {
                Name = globalOptionSetName,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveOptionSetResponse)OrganizationService.Execute(request);
            if (response != null && response.OptionSetMetadata != null && response.OptionSetMetadata.OptionSetType == OptionSetType.Picklist)
            {
                return GetOptionSetValueForText((OptionSetMetadata)response.OptionSetMetadata, label);
            }

            return null;
        }

        private int? GetOptionSetValueForText(OptionSetMetadata optionSet, string label)
        {
            var option = optionSet.Options.FirstOrDefault(i => i.Label.LocalizedLabels != null
                                                            && i.Label.LocalizedLabels.Count > 0
                                                            && string.Compare(i.Label.LocalizedLabels[0].Label, label, true) == 0);

            return option != null ? option.Value : -1;
        }

        public void Delete<T>(Guid id) where T : CrmEntity
        {
            OrganizationService.Delete(GetEntityName<T>(), id);
        }

        public TResponse Execute<TRequest, TResponse>(TRequest request)
            where TResponse : class
            where TRequest : class
        {
            var req = request as OrganizationRequest;

            var response = OrganizationService.Execute(req);
            return response as TResponse;
        }
    }
}
