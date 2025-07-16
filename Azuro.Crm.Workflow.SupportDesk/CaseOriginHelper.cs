using System;
using Azuro.Crm.Integration;

namespace Azuro.Crm.Workflow
{
	public class CaseOriginHelper
	{
		public static bool IsValidOrigin(ICrmHelper CrmHelper, int caseOrigin, string applicableCaseOrigins)
		{
			if (string.IsNullOrEmpty(applicableCaseOrigins))
				return false;
			if (CrmHelper == null)
				throw new ArgumentNullException("CrmHelper");
			var applicableCaseOriginsArr = applicableCaseOrigins.Split(';');
			foreach (var s in applicableCaseOriginsArr)
			{
				var originCode = CrmHelper.GetOptionSetValueForText("incident_caseorigincode", s);
				if (originCode == null)
					throw new ArgumentNullException("originCode");
				if (caseOrigin == originCode)
					return true;
			}

			return false;
		}
	}
}
