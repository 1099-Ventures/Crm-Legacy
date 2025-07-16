using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
//using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaPlugin
{
	public abstract class APlugin
	{
		protected IServiceProvider ServiceProvider { get; set; }

		private ITracingService _tracingService = null;
		protected ITracingService TracingService { get { return _tracingService ?? (_tracingService = GetTracingService(ServiceProvider)); } }

		private IOrganizationService _organizationService = null;
		protected IOrganizationService OrganizationService { get { return _organizationService ?? (_organizationService = GetOrganizationService(ServiceProvider)); } }

		protected IPluginExecutionContext _context;
		protected IPluginExecutionContext Context { get { return _context ?? (_context = (IPluginExecutionContext)ServiceProvider.GetService(typeof(IPluginExecutionContext))); } }

		protected Azuro.Crm.Integration.CrmHelper _crmHelper = null;
		protected Azuro.Crm.Integration.CrmHelper CrmHelper
		{
			get
			{
				if (_crmHelper == null)
				{
					_crmHelper = new Azuro.Crm.Integration.CrmHelper();
					_crmHelper.OrganisationId = Context.OrganizationId;
					//_crmHelper.OrganizationService = GetOrganizationService(ServiceProvider);
				}
				return _crmHelper;
			}
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
			IOrganizationService organizationService = orgSvcFactory.CreateOrganizationService(Context.UserId);
			if (organizationService == null)
				throw new InvalidPluginExecutionException("Failed to retrieve the Organization Service.");

			return organizationService;
		}

		protected T Execute<T>(OrganizationRequest request) where T : OrganizationResponse
		{
			return (T)OrganizationService.Execute(request);
		}

		protected Entity GetPrimaryEntity(string entityName)
		{
			TracingService.Trace("Retrieving [{0}] from the Context", entityName);
			Entity entity = null;
			if (Context.InputParameters != null
				&& Context.InputParameters.Contains("Target")
				&& Context.InputParameters["Target"] is Entity)
			{
				entity = (Entity)Context.InputParameters["Target"];
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
	}
}
