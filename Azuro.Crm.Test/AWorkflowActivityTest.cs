using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Test
{
	[TestClass]
	public abstract class AWorkflowActivityTest<T> where T : Activity
	{
		/*
			var input = new Dictionary<string, object>
			{
				{ "LeadReference", new EntityReference(Lead.EntityLogicalName, new Guid("24DC5036-BF9B-E711-8100-005056A23E1F")) },
				{ "PostedFormReference", new EntityReference(cdi_postedform.EntityLogicalName, new Guid("468914A3-5F32-4DDE-9562-D8DE253F8175")) }
			};
		 */
		protected abstract Dictionary<string, object> InputParameters { get; }

		[TestMethod]
		public virtual void Execute()
		{
			WorkflowInvoker wfi = new WorkflowInvoker(CreateWorkflowActivity());

			wfi.Extensions.Add(CreateWorkflowContext());
			wfi.Extensions.Add(CreateTracingService());
			wfi.Extensions.Add(CreateOrganizationServiceFactory());

			var output = InputParameters?.Count() > 0 ? wfi.Invoke(InputParameters) : wfi.Invoke();
		}

		//protected abstract void Test();
		protected abstract T CreateWorkflowActivity();

		protected virtual IOrganizationServiceFactory CreateOrganizationServiceFactory()
		{
			var serverName = (string)ConfigurationManager.AppSettings["OrganisationUrl"];

			return new SimpleOrganizationServiceFactory(serverName);
		}

		protected virtual ITracingService CreateTracingService()
		{
			return new ConsoleTracingService();
		}

		protected virtual IWorkflowContext CreateWorkflowContext()
		{
			return new SimpleWorkflowContext();
		}
	}
}
