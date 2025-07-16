using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Integration
{
	public class CrmFieldAttribute : Attribute
	{
		public string Name { get; set; }

		public bool IsIdentity { get; set; }

		//public bool IsRelatedEntity { get; set; }

		public bool IsRequired { get; set; }

		//public string RelatedKey { get; set; }

		public bool IsPicklist { get; set; }

		public CrmFieldAttribute(string name)
		{
			Name = name;
		}

		public CrmFieldAttribute(string name, bool isIdentity)
		{
			Name = name;
			IsIdentity = isIdentity;
		}
	}
}
