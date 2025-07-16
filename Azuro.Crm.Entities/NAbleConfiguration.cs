using Azuro.Crm.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Entities
{
	[CrmEntity("azuro_nableconfiguration")]
	public class NAbleConfiguration : CrmEntity
	{
		[CrmField("azuro_nableconfigurationid", IsRequired = true, IsIdentity = true)]
		public Guid Id { get; set; }

		[CrmField("azuro_nabletriggereventqueue")]
		public string NAbleTriggerEventQueue { get; set; }
	}
}
