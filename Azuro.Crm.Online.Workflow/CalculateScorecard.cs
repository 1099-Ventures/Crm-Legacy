using Azuro.Crm.Online.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Online.Workflow
{
	/// <summary>
	/// Creating a simple lead scoring activity, that will be extended to be a scorecard on any entity.
	/// 
	/// It expects a Scorecard to be set, and will infer the required entity to calculate based on the contextual primary entity.
	/// </summary>
	public class CalculateScorecard : ACrmCodeActivity
	{
		[Input("Scorecard")]
		[RequiredArgument]
		[ReferenceTarget("azuro_scorecard")]
		public InArgument<EntityReference> Scorecard { get; set; }

		[Output("Record Id")]
		public OutArgument<int> Score { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			//	Get the primary entity
			var pe = GetPrimaryEntity(context);

			//	Retrieve scorecard
			var sci = Scorecard.Get(context);
			var scorecard = Retrieve<azuro_scorecard>(context, sci.LogicalName, sci.Id);

			//	Validate scorecard
			Validate(pe, scorecard);

			//	Calculate
			var score = Calculate(pe, scorecard);

			Score.Set(context, score);
		}

		private int Calculate(Entity pe, azuro_scorecard scorecard)
		{
			throw new NotImplementedException();
		}

		private void Validate(Entity pe, azuro_scorecard scorecard)
		{
			throw new NotImplementedException();
		}
	}
}
