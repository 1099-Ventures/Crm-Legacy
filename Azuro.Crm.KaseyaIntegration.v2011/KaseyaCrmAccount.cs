using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaIntegration
{
	[CrmEntity("account")]
	public class KaseyaCrmAccount
	{
		[CrmField("accountid")]
		public Guid Id { get; set; }
		[CrmField("name")]
		public string Name { get; set; }
		
	}
}
