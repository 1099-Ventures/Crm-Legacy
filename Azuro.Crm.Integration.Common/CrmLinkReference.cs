using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Integration
{
	public class CrmLinkReference
	{
		public string LinkName { get; set; }
		public string FromAttribute { get; set; }
		public string ToAttribute { get; set; }
	}
}
