using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Integration
{
	public class CrmEntityAttribute : Attribute
	{
		public string Name { get; set; }

		public CrmEntityAttribute(string name)
		{
			Name = name;
		}
	}
}
