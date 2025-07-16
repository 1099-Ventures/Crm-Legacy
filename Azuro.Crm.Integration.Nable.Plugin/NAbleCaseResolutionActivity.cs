using Azuro.Crm.Entities;
using Azuro.Crm.Integration.Nable.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Integration.Nable.Plugin
{
	public class NAbleCaseResolutionActivity : ACrmCodeActivity
	{
		[Input("Entity Reference (Case) (Input)")]
		[ReferenceTarget("incident")]
		public InArgument<EntityReference> CaseInput { get; set; }

		//private NAbleConfiguration _cfg = null;
		//private NAbleConfiguration Config { get { return _cfg ?? (_cfg = RetrieveNableConfiguration()); } }

		protected override void OnExecute(CodeActivityContext context)
		{
			if (!IsValid(context, CaseInput))
				throw new InvalidPluginExecutionException("No Valid Case Supplied");

			Trace(context, "Retrieving Case");

			var crmHelper = GetCrmHelper(context);
			var caseItem = crmHelper.GetEntity<Case>(CaseInput.Get(context).Id);

			Trace(context, "Creating Queue Message");
			int nableRef = 0;
			int.TryParse(caseItem.ExternalSystemReference, out nableRef);
			var crn = new CaseResolvedNotification
			{
				CaseNumber = caseItem.TicketNumber,
				CaseStatus = crmHelper.GetOptionSetValueText<Case>("statuscode", caseItem.StatusCode),
				ExternalReference = nableRef,
				ResolutionDescription = caseItem.ResolutionDescription,
			};

			Trace(context, "Inserting Message into Queue");
			Azuro.MSMQ.QueueHelper.Insert(RetrieveNableConfiguration(context, crmHelper).NAbleTriggerEventQueue, crn);
		}

		private NAbleConfiguration RetrieveNableConfiguration(CodeActivityContext context, ICrmHelper crmHelper)
		{
			NAbleConfiguration cfg = null;
			Trace(context, "Retrieving Configuration");
			var cfglist = crmHelper.GetEntityList<NAbleConfiguration>();
			if (cfglist.Count == 1)
			{
				Trace(context, "Configuration entity found");
				cfg = cfglist[0];
			}
			else
			{
				Trace(context, "No, or multiple configuration entities found, creating default entity.");
				cfg = new NAbleConfiguration { NAbleTriggerEventQueue = @".\private$\NAbleResolutionQueue", };
			}

			return cfg;
		}
	}
}
