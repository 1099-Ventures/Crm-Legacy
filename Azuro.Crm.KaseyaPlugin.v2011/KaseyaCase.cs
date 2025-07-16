using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaPlugin
{
	[CrmEntity("incident")]
	public class KaseyaCase
	{
		[CrmField("statuscode")]
		public CaseStatus statuscode { get; set; }

		[CrmField("incidentid", true)]
		public Guid Id { get; set; }

		[CrmField("ticketnumber")]
		public string ticketnumber { get; set; }
	}
}
