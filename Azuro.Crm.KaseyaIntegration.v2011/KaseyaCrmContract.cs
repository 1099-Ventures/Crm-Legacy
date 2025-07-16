using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaIntegration
{
	[CrmEntity("contract")]
	public class KaseyaCrmContract
	{
		[CrmField("contractid")]
		public Guid Id { get; set; }
	}
}
