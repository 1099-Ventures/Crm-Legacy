using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using NLog;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		protected override void Execute(CodeActivityContext context)
		{
			Trace(context, "Entering {0}.Execute - OrgId = {1}", this.GetType(), GetWorkflowContext(context).OrganizationId);
			OnExecute(context);

#if DEBUG
			throw new InvalidOperationException("Running in DEBUG MODE - Check Traces");
#endif
		}

		protected ITracingService GetTracingService(CodeActivityContext context)
		{
			return context.GetExtension<ITracingService>();
		}

		protected IWorkflowContext GetWorkflowContext(CodeActivityContext context)
		{
			return context.GetExtension<IWorkflowContext>();
		}

		protected IOrganizationService GetOrganizationService(CodeActivityContext context)
		{
			LogTrace(context, "GetOrganizationService");

			IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
			return serviceFactory.CreateOrganizationService(GetWorkflowContext(context).InitiatingUserId);
		}

		protected bool IsValid(EntityReference entityReference)
		{
			if (entityReference == null)
				return false;

			return (entityReference != null && entityReference.Id != Guid.Empty);
		}

		protected bool IsValid(CodeActivityContext context, InArgument<EntityReference> entityArgument)
		{
			if (context == null || entityArgument == null)
				return false;

			return IsValid(entityArgument.Get(context));
		}

		protected Entity Retrieve(CodeActivityContext context, string entityName, Guid id, ColumnSet columns = null)
		{
			LogTrace(context, $"Retrieve {entityName} - {id}");
			return GetOrganizationService(context).Retrieve(entityName, id, columns ?? new ColumnSet(true));
		}

		protected T Retrieve<T>(CodeActivityContext context, string entityName, Guid id, ColumnSet columns = null) where T : Entity
		{
			return Retrieve(context, entityName, id, columns ?? new ColumnSet(true)).ToEntity<T>();
		}

		#region Logging and Tracing
		protected string GetTraceMessage(string correlationId, string message, params object[] parms)
		{
			return string.Format("{0}|{1}", correlationId, string.Format(message, parms));
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
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
			Logger.Trace(msg);
		}

		protected void LogDebug(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
			Logger.Debug(msg);
		}

		protected void LogWarn(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
			Logger.Warn(msg);
		}
		protected void LogInfo(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
			Logger.Info(msg);
		}
		protected void LogError(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
			Logger.Error(msg);
		}
		protected void LogFatal(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
			Logger.Fatal(msg);
		}
		#endregion
	}
}
