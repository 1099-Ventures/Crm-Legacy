﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.KaseyaIntegration
{
	[CrmEntity("product")]
	public class KaseyaCrmProduct
	{
		[CrmField("productid")]
		public Guid ProductID { get; set; }
	}
}
