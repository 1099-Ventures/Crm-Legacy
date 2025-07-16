using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using NLog;
using System;
using System.Activities;

namespace Azuro.Crm.Integration
{
	/// <summary>
	/// A base class containing useful and commonly required Crm Workflow Activity functions.
	/// </summary>
	public abstract class ACrmCodeActivity : CodeActivity
	{
		protected static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Overload this method to implement the OnExecute logic.
		/// </summary>
		/// <param name="context">The workflow context as received from the workflow invocation call.</param>
		protected abstract void OnExecute(CodeActivityContext context);

		protected ITracingService GetTracingService(CodeActivityContext context)
		{
			return context.GetExtension<ITracingService>();
		}

		protected IWorkflowContext GetWorkflowContext(CodeActivityContext context)
		{
			return context.GetExtension<IWorkflowContext>();
		}

		protected ICrmHelper GetCrmHelper(CodeActivityContext context)
		{
			var crmHelper = new Azuro.Crm.Integration.CrmHelper();
			crmHelper.OrganisationId = GetWorkflowContext(context).OrganizationId;
			crmHelper.OrganizationService = context.GetExtension<IOrganizationService>();

			return crmHelper;
		}

		protected void Trace(CodeActivityContext context, string message, params object[] parms)
		{
#if DEBUG
			if (GetTracingService(context) != null)
#endif
				GetTracingService(context).Trace(message, parms);
		}

		protected void LogTrace(CodeActivityContext context, string message, params object[] parms)
		{
			LogTrace(context.ActivityInstanceId, message, parms);
		}
		protected void LogTrace(string correlationId, string message, params object[] parms)
		{
			Logger.Trace("{0}|{1}", correlationId, string.Format(message, parms));
		}

		protected override void Execute(CodeActivityContext context)
		{
			Trace(context, "Entering Execute - OrgId = {0}", GetWorkflowContext(context).OrganizationId);
			OnExecute(context);

#if DEBUG
			throw new InvalidOperationException("Running in DEBUG MODE - Check Traces");
#endif
		}

		protected bool IsValid(CodeActivityContext context, InArgument<EntityReference> entityArgument)
		{
			if (context == null || entityArgument == null)
				return false;

			var reference = entityArgument.Get(context);
			return (reference != null && reference.Id != Guid.Empty);
		}
	}
}
