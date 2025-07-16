using Microsoft.Xrm.Sdk;
using System;

namespace Azuro.Crm.Integration
{
	public abstract class APlugin : IPlugin
	{
		protected IPluginExecutionContext GetContext(IServiceProvider serviceProvider)
		{
			return (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
		}

		protected Azuro.Crm.Integration.CrmHelper GetCrmHelper(IServiceProvider serviceProvider)
		{
			var crmHelper = new Azuro.Crm.Integration.CrmHelper();
			crmHelper.OrganisationId = GetContext(serviceProvider).OrganizationId;
			crmHelper.OrganizationService = GetOrganizationService(serviceProvider);

			return crmHelper;
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

		protected int GetOptionSetValue(object crmValue)
		{
			return ((Microsoft.Xrm.Sdk.OptionSetValue)crmValue).Value;
		}

		protected void SetOptionSetValue(object crmValue, int value)
		{
			((Microsoft.Xrm.Sdk.OptionSetValue)crmValue).Value = value;
		}

		/// <summary>
		/// This method handles the required CRM interaction, but calls the overloaded method.
		/// It will facilitate required interactions, prior to being able to properly use the abstract class's functions
		/// </summary>
		/// <param name="serviceProvider">The serviceProvider supplied by CRM.</param>
		public abstract void Execute(IServiceProvider serviceProvider);
	}
}
