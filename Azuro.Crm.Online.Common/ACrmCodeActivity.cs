using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Online.Integration
{
	/// <summary>
	/// A base class containing useful and commonly required Crm Workflow Activity functions.
	/// This is a copy for use on CrmOnline.
	/// </summary>
	public abstract class ACrmCodeActivity : CodeActivity
	{
		/// <summary>
		/// Overload this method to implement the OnExecute logic.
		/// </summary>
		/// <param name="context">The workflow context as received from the workflow invocation call.</param>
		protected abstract void OnExecute(CodeActivityContext context);

		protected override void Execute(CodeActivityContext context)
		{
			Trace(context, "Entering Execute - OrgId = {0}", GetWorkflowContext(context).OrganizationId);
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

		protected bool IsValid(CodeActivityContext context, InArgument<EntityReference> entityArgument)
		{
			if (context == null || entityArgument == null)
				return false;

			var reference = entityArgument.Get(context);
			return (reference != null && reference.Id != Guid.Empty);
		}

		protected Entity GetPrimaryEntity(CodeActivityContext context)
		{
			var wfctx = GetWorkflowContext(context);
			return Retrieve(context, wfctx.PrimaryEntityName, wfctx.PrimaryEntityId);
		}

		protected Entity Retrieve(CodeActivityContext context, string entityName, Guid id, ColumnSet columns = null)
		{
			return GetOrganizationService(context).Retrieve(entityName, id, columns ?? new ColumnSet(true));
		}

		protected T Retrieve<T>(CodeActivityContext context, string entityName, Guid id, ColumnSet columns = null) where T : Entity
		{
			return Retrieve(context, entityName, id, columns ?? new ColumnSet(true)).ToEntity<T>();
		}

		protected List<T> RetrieveMultiple<T>(CodeActivityContext context, QueryBase query) where T : Entity
		{
			var result = GetOrganizationService(context).RetrieveMultiple(query);
			var list = new List<T>(result.Entities.Count);
			foreach (var t in result.Entities)
			{
				list.Add(t.ToEntity<T>());
			}
			return list;
		}

		public SetStateResponse SetStatus(CodeActivityContext context, Entity entity, int stateCode, int statusCode)
		{
			return SetStatus(context, entity.LogicalName, entity.Id, stateCode, statusCode);
		}

		public SetStateResponse SetStatus(CodeActivityContext context, string entityName, Guid id, int stateCode, int statusCode)
		{
			//	Update Entity Status
			SetStateRequest setStateRequest = new SetStateRequest();

			setStateRequest.EntityMoniker = new EntityReference(entityName, id);
			setStateRequest.State = new OptionSetValue(stateCode);
			setStateRequest.Status = new OptionSetValue(statusCode);

			//	TODO: Review the response values to be able to return a more meaningful result
			var response = (SetStateResponse)GetOrganizationService(context).Execute(setStateRequest);
			return response;
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
		}

		protected void LogDebug(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
		}

		protected void LogWarn(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
		}
		protected void LogInfo(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
		}
		protected void LogError(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
		}
		protected void LogFatal(CodeActivityContext context, string message, params object[] parms)
		{
			var msg = GetTraceMessage(context.ActivityInstanceId, message, parms);
			Trace(context, msg);
		}
		#endregion
	}
}
