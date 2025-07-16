using Azuro.Crm.Online.Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Online.Integration
{
	public abstract class APlugin : IPlugin
	{
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
			GetTracingService(serviceProvider).Trace("Retrieving Default Target Entity from the Context");
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
			GetTracingService(serviceProvider).Trace("Retrieving [{0}] from the Context", entityName);
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

		protected T GetEntity<T>(IOrganizationService service, Guid id, ColumnSet columns = null) where T : Entity
		{
			return service.Retrieve(GetEntityLogicalName<T>(), id, columns ?? new ColumnSet(true)).ToEntity<T>();
		}

		protected Entity GetEntity(IOrganizationService service, string entityName, Guid id, ColumnSet columns = null)
		{
			return service.Retrieve(entityName, id, columns ?? new ColumnSet(true));
		}

		protected string GetEntityLogicalName<T>() where T : Entity
		{
			var f = typeof(T).GetField("EntityLogicalName");
			return f.GetRawConstantValue() as string;
		}

		protected int GetOptionSetValue(OptionSetValue optionSet)
		{
			return ((Microsoft.Xrm.Sdk.OptionSetValue)optionSet).Value;
		}

		protected string GetOptionSetText(IServiceProvider serviceProvider, IOrganizationService service, string entityName, string attribute, int optionValue)
		{
			string optionLabel = String.Empty;

			var options = GetOptionSetDefinition(serviceProvider, service, entityName, attribute);

			if (options != null)
			{
				var optionMeta = options.FirstOrDefault(o => o.Value == optionValue);
				// If our numeric value matches, set the string to our status code label
				return optionMeta?.Label.UserLocalizedLabel.Label;
			}
			else
			{
				var msg = string.Format("Optionset [{0}] was not found on [{1}]", attribute, entityName);
				LogError(serviceProvider, msg);
				throw new InvalidPluginExecutionException(msg);
			}
		}

		public string GetOptionSetText<T>(IServiceProvider serviceProvider, IOrganizationService service, string attribute, int optionValue) where T : Entity
		{
			return GetOptionSetText(serviceProvider, service, GetEntityLogicalName<T>(), attribute, optionValue);
		}

		protected int GetOptionSetValueFromText(IServiceProvider serviceProvider, IOrganizationService service, string entityName, string attribute, string text)
		{
			var msg = "";
			var options = GetOptionSetDefinition(serviceProvider, service, entityName, attribute);

			if (options != null)
			{
				var optionMeta = options.FirstOrDefault(o => o.Label.LocalizedLabels.FirstOrDefault(a => string.Compare(a.Label, text, true) == 0) != null);
				// If our numeric value matches, set the string to our status code label
				if (optionMeta != null && optionMeta.Value.HasValue)
					return optionMeta.Value.Value;
				else
				{
					msg = string.Format("Option Value [{0}] was not found for [{1}.{2}]", text, entityName, attribute);
				}
			}
			else
			{
				msg = string.Format("Optionset [{0}] was not found on [{1}]", attribute, entityName);
			}

			LogError(serviceProvider, msg);
			throw new InvalidPluginExecutionException(msg);
		}

		protected OptionMetadataCollection GetOptionSetDefinition(IServiceProvider serviceProvider, IOrganizationService service, string entityName, string attribute)
		{
			var attrMetadata = GetAttributeMetadata(service, entityName, attribute);
			var picklistMetadata = attrMetadata as PicklistAttributeMetadata;
			if (attrMetadata != null)
			{
				// For every status code value within all of our status codes values
				//  (all of the values in the drop down list)
				return attrMetadata is PicklistAttributeMetadata ? ((PicklistAttributeMetadata)attrMetadata).OptionSet.Options
						: attrMetadata is StateAttributeMetadata ? ((StateAttributeMetadata)attrMetadata).OptionSet.Options
						: attrMetadata is StatusAttributeMetadata ? ((StatusAttributeMetadata)attrMetadata).OptionSet.Options
						: null;
			}
			else
			{
				var msg = string.Format("Field [{0}] on [{1}] is not an OptionSet.", attribute, entityName);
				LogError(serviceProvider, "Field [{0}] on [{1}] is not an OptionSet.", attribute, entityName);
				throw new InvalidPluginExecutionException(msg);
			}
		}

		protected AttributeMetadata GetAttributeMetadata(IOrganizationService service, string entityName, string attribute)
		{
			RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
			{
				EntityLogicalName = entityName,
				LogicalName = attribute,
				RetrieveAsIfPublished = true,
			};

			RetrieveAttributeResponse attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
			return attributeResponse.AttributeMetadata;
		}

		protected bool IsValid(Entity entity, string attributeName)
		{
			return entity.Attributes.Contains(attributeName)
				&& entity.Attributes[attributeName] != null;
		}

		protected string LogCrmTrace(IServiceProvider serviceProvider, LogSeverity severity, string msg, params object[] args)
		{
			var message = string.Format("[{0}]|{1}|{2}", GetContext(serviceProvider).CorrelationId, severity, string.Format(msg, args));
			GetTracingService(serviceProvider).Trace(message);
			return message;
		}

		protected void LogTrace(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			LogCrmTrace(serviceProvider, LogSeverity.Trace, msg, args);
		}

		protected void LogDebug(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			LogCrmTrace(serviceProvider, LogSeverity.Debug, msg, args);
		}

		protected void LogInfo(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			LogCrmTrace(serviceProvider, LogSeverity.Info, msg, args);
		}

		protected void LogWarn(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			LogCrmTrace(serviceProvider, LogSeverity.Warn, msg, args);
		}

		protected void LogError(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			LogCrmTrace(serviceProvider, LogSeverity.Error, msg, args);
		}

		protected void LogFatal(IServiceProvider serviceProvider, string msg, params object[] args)
		{
			LogCrmTrace(serviceProvider, LogSeverity.Fatal, msg, args);
		}
	}
}
