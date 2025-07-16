using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Integration
{
	public class CrmQuery
	{
		public string Key { get; set; }
		public FilterOperator Operator { get; set; }
		public FilterType FilterType { get; set; }
		public object Value { get; set; }
	}
}
