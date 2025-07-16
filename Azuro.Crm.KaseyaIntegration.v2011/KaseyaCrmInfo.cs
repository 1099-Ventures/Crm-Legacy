using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaIntegration
{
	[CrmEntity("new_kaseyainfo")]
	public class KaseyaCrmInfo : CrmEntity<KaseyaCrmInfo>
	{
		[CrmEntity("new_kaseyainfoid")]
		public Guid Id { get; set; }

		[CrmField("new_name")]
		public string GroupName { get; set; }

		[CrmField("new_accid")]
		public CrmEntityReference AccountID { get; set; }

		[CrmField("new_contractid")]
		public CrmEntityReference ContractID { get; set; }
	}
}