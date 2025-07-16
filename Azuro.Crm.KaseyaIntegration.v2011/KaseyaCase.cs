using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaIntegration
	
{

	[CrmEntity("incident")]
	public class KaseyaCase : CrmEntity<KaseyaCase>
	{
		[CrmField("ticketnumber")]
		public string ticketnumber { get; set; }

		[CrmField("contactid")]
		public Guid contactid { get; set; }

		[CrmField("accountid")]
		public Guid accountid { get; set; }

		[CrmField("customerid")]
		public Guid customerid { get; set; }

		[CrmField("title")]
		public string title { get; set; }

		[CrmField("description")]
		public string description { get; set; }

		[CrmField("createdon")]
		public DateTime createdon { get; set; }

		[CrmField("contractid")]
		public Guid contractid { get; set; }

		[CrmField("contractdetailid")]
		public Guid contractdetailid { get; set; }

	
	}
}
