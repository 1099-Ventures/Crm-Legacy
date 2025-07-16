using Azuro.Crm.Entities;
using Azuro.Crm.Integration;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Workflow
{
	public class QueueHelper
	{
		private ICrmHelper CrmHelper { get; set; }
		public QueueHelper(ICrmHelper helper)
		{
			CrmHelper = helper;
		}

		public void Assign(Case caseItem, Guid queueId)
		{
			AddToQueueRequest addToSourceQueue = new AddToQueueRequest
			{
				DestinationQueueId = queueId,
				Target = new EntityReference(Case.LogicalName, caseItem.Id),
			};

			var qResponse = CrmHelper.Execute<AddToQueueRequest, AddToQueueResponse>(addToSourceQueue);
		}
	}
}
