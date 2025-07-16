using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaIntegration
{
	[CrmEntity("contractdetail")]
	public class KaseyaCrmContractDetail
	{
		[CrmField("contractdetailid")]
		public Guid Id { get; set; }

	}
}
