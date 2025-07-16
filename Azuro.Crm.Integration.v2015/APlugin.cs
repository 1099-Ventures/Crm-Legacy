using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Integration
{
	public abstract class APlugin : IPlugin
	{
		protected static Logger Logger = LogManager.GetCurrentClassLogger();

		public void Execute(IServiceProvider serviceProvider)
		{
			IPluginExecutionContext context = GetContext(serviceProvider);
			IOrganizationService service = GetOrganizationService(serviceProvider);
			LogTrace(serviceProvider, "Plugin context depth: [{0}]", context.Depth);

			Execute(serviceProvider, service, context);
		}

		protected abstract void Execute(IServiceProvider serviceProvider, IOrganizationService service, IPluginExecutionContext context);

		protected IPluginExecutionContext GetContext(IServiceProvider serviceProvider)
		{
			return (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
		}

		protected ITracingService GetTracingService(IServiceProvider serviceProvider)
		{
			ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
			if (tracingService == null)
				throw new InvalidPluginExecutionException("Failed to retrieve the Tracing Service.");

			return tracingService;
		}

		protected IOrganizationService GetOrganizationService(IServiceProvider serviceProvider)
		{
			IOrganizationServiceFactory orgSvcFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
			if (orgSvcFactory == null)
				throw new InvalidPluginExecutionException("Failed to retrieve the Organization Service Factory.");
			IOrganizationService organizationService = orgSvcFactory.CreateOrganizationService(GetContext(serviceProvider).UserId);
			if (organizationService == null)
				throw new InvalidPluginExecutionException("Failed to retrieve the Organization Service.");

			return organizationService;
		}

		protected T Execute<T>(IOrganizationService service, OrganizationRequest request) where T : OrganizationResponse
		{
			return (T)service.Execute(request);
		}

		protected Microsoft.Xrm.Sdk.Entity GetPrimaryEntity(IServiceProvider serviceProvider)
		{
			LogCrmTrace(serviceProvider, "Retrieving Default Target Entity from the Context");
			var context = GetContext(serviceProvider);
			if (context.InputParameters != null && context.InputParameters.Contains("Target"))
			{
				return GetPrimaryEntity(serviceProvider, ((Entity)context.InputParameters["Target"]).LogicalName);
			}
			else
				throw new InvalidPluginExecutionException(OperationStatus.Failed, "No Target Entity found in Plugin Context.");
		}

		protected Microsoft.Xrm.Sdk.Entity GetPrimaryEntity(IServiceProvider serviceProvider, string entityName)
		{
			LogCrmTrace(serviceProvider, "Retrieving [{0}] from the Context", entityName);
			Entity entity = null;
			var context = GetContext(serviceProvider);
			if (context.InputParameters != null
				&& context.InputParameters.Contains("Target")
				&& context.InputParameters["Target"] is Entity)
			{
				entity = (Entity)context.InputParameters["Target"];
				if (entity.LogicalName == entityName)
					return entity;
				else
					throw new InvalidPluginExecutionException(OperationStatus.Failed, string.Format("Context Target [{0}] is not of the expected Type [{1}]", entity.LogicalName, entityName));
			}
			else
				throw new InvalidPluginExecutionException(OperationStatus.Failed, "No Target Entity found in Plugin Context.");
		}
		protected T GetPrimaryEntity<T>(IServiceProvider serviceProvider, string entityName) where T : Entity
		{
			return GetPrimaryEntity(serviceProvider, entityName).ToEntity<T>();
		}
		protected T GetPrimaryEntity<T>(IServiceProvider serviceProvider) where T : Entity
		{
			return GetPrimaryEntity(serviceProvider).ToEntity<T>();
		}

		protected T GetEntity<T>(IOrganizationService service, Guid id, ColumnSet columns = null) where T : Entity
		{
			return service.Retrieve(GetEntityLogicalName<T>(), id, columns ?? new ColumnSet(true)).ToEntity<T>();
		}

		public List<Entity> GetEntityList(IOrganizationService service, string entityName)
		{
			return GetEntityList(service, entityName, null);
		}

		public List<Entity> GetEntityList(IOrganizationService service, string entityName, List<KeyValuePair<string, object>> filter)
		{
			return GetEntityList<Entity>(service, entityName, filter);
		}

		public List<T> GetEntityList<T>(IOrganizationService service) where T : Entity
		{
			return GetEntityList<T>(service, GetEntityLogicalName<T>(), null);
		}

		public List<T> GetEntityList<T>(IOrganizationService service, string entityName, List<KeyValuePair<string, object>> filter) where T : Entity
		{
			var qe = BuildQuery(entityName, filter);

			return GetEntityList<T>(service, qe);
		}

		protected List<T> GetEntityList<T>(IOrganizationService service, QueryExpression query) where T : Entity
		{
			var result = service.RetrieveMultiple(query);
			List<T> list = new List<T>(result.Entities.Count);
			foreach (var entity in result.Entities)
			{
				list.Add(entity.ToEntity<T>());
			}

			return list;
		}

		private QueryExpression BuildQuery(string entityName, List<KeyValuePair<string, object>> filter)
		{
			var qe = new QueryExpression
			{
				EntityName = entityName,
				ColumnSet = new ColumnSet(true),
				Criteria = new FilterExpression(LogicalOperator.And),
			};

			if (filter != null)
			{
				foreach (var kvp in filter)
				{
					var ce = new ConditionExpression { EntityName = entityName, AttributeName = kvp.Key, Operator = ConditionOperator.Equal, };
					ce.Values.Add(kvp.Value);
					qe.Criteria.AddCondition(ce);
				}
			}

			return qe;
		}

		protected string GetEntityLogicalName<T>() where T : Entity
		{
			var f = typeof(T).GetField("EntityLogicalName");
			return f.GetRawConstantValue() as string;
		}

		protected int GetOptionSetValue(object optionSet)
		{
			if (optionSet is Microsoft.Xrm.Sdk.OptionSetValue)
				return ((Microsoft.Xrm.Sdk.OptionSetValue)optionSet).Value;
			else
				throw new ArgumentException("APlugin.GetOptionSetValue expects a 'Microsoft.Xrm.Sdk.OptionSetValue' to be passed in.");
		}

		protected void SetOptionSetValue(object optionSet, int value)
		{
			if (optionSet is Microsoft.Xrm.Sdk.OptionSetValue)
				((Microsoft.Xrm.Sdk.OptionSetValue)optionSet).Value = value;
			else
				throw new ArgumentException("APlugin.GetOptionSetValue expects a 'Microsoft.Xrm.Sdk.OptionSetValue' to be passed in.");
		}

		protected string GetOptionSetValueText(IOrganizationService service, string entityName, string attribute, int optionValue)
		{
			string optionLabel = String.Empty;
			RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
			{
				EntityLogicalName = entityName,
				LogicalName = attribute,
				RetrieveAsIfPublished = true
			};

			RetrieveAttributeResponse attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
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

		protected bool IsValid(Entity entity, string attributeName)
		{
			return entity.Attributes.Contains(attributeName)
				&& entity.Attributes[attributeName] != null;
		}

		protected string LogCrmTrace(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			var message = string.Format("[{0}]|{1}", GetContext(serviceProvider).CorrelationId, string.Format(msg, args));
			GetTracingService(serviceProvider).Trace(message);
			return message;
		}

		protected void LogTrace(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			Logger.Trace(LogCrmTrace(serviceProvider, msg, args));
		}

		protected void LogDebug(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			Logger.Debug(LogCrmTrace(serviceProvider, msg, args));
		}

		protected void LogInfo(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			Logger.Info(LogCrmTrace(serviceProvider, msg, args));
		}

		protected void LogWarn(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			Logger.Warn(LogCrmTrace(serviceProvider, msg, args));
		}

		protected void LogError(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			Logger.Error(LogCrmTrace(serviceProvider, msg, args));
		}

		protected void LogFatal(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			Logger.Fatal(LogCrmTrace(serviceProvider, msg, args));
		}
	}
}
