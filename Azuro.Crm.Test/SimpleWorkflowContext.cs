using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Azuro.Crm.Test
{
	public class SimpleWorkflowContext : IWorkflowContext, IDisposable
	{
		public string StageName => throw new NotImplementedException();

		public int WorkflowCategory => throw new NotImplementedException();

		public int WorkflowMode => throw new NotImplementedException();

		public IWorkflowContext ParentContext => throw new NotImplementedException();

		public int Mode => throw new NotImplementedException();

		public int IsolationMode => throw new NotImplementedException();

		public int Depth => throw new NotImplementedException();

		public string MessageName => throw new NotImplementedException();

		public string PrimaryEntityName => throw new NotImplementedException();

		public Guid? RequestId => throw new NotImplementedException();

		public string SecondaryEntityName => throw new NotImplementedException();

		public ParameterCollection InputParameters => throw new NotImplementedException();

		public ParameterCollection OutputParameters => throw new NotImplementedException();

		public ParameterCollection SharedVariables => throw new NotImplementedException();

		public Guid UserId => throw new NotImplementedException();

		public Guid InitiatingUserId => Guid.NewGuid();

		public Guid BusinessUnitId => throw new NotImplementedException();

		public Guid OrganizationId => Guid.NewGuid();

		public string OrganizationName => throw new NotImplementedException();

		public Guid PrimaryEntityId => throw new NotImplementedException();

		public EntityImageCollection PreEntityImages => throw new NotImplementedException();

		public EntityImageCollection PostEntityImages => throw new NotImplementedException();

		public EntityReference OwningExtension => throw new NotImplementedException();

		public Guid CorrelationId => throw new NotImplementedException();

		public bool IsExecutingOffline => throw new NotImplementedException();

		public bool IsOfflinePlayback => throw new NotImplementedException();

		public bool IsInTransaction => throw new NotImplementedException();

		public Guid OperationId => throw new NotImplementedException();

		public DateTime OperationCreatedOn => throw new NotImplementedException();

		public void Dispose()
		{
			//	Dispose of stuff
		}
	}
}
