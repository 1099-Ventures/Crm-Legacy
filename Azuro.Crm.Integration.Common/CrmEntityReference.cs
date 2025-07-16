using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Integration
{
	public class CrmEntityReference
	{
		public string EntityName { get; set; }
		public string PrimaryAttributeName { get; set; }
		public Guid ReferencedEntityId { get; set; }

		public CrmEntityReference() { }
		public CrmEntityReference(string name, string primaryAttribute, Guid id)
		{
			EntityName = name;
			PrimaryAttributeName = primaryAttribute;
			ReferencedEntityId = id;
		}

		public static bool IsValid(CrmEntityReference reference)
		{
			return (reference != null && reference.ReferencedEntityId != Guid.Empty);
		}
	}
}
