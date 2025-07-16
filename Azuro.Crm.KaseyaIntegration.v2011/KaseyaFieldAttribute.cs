using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.KaseyaIntegration
{
    public class KaseyaFieldAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsIdentity { get; set; }
		public string CrmFieldMapp { get; set; }

		public KaseyaFieldAttribute(string name)
		{
			Name = name;
			
		}
        public KaseyaFieldAttribute(string name, string crmFieldMapp)
        {
            Name = name;
			CrmFieldMapp = crmFieldMapp;
        }

        public KaseyaFieldAttribute(string name, bool isIdentity)
        {
            IsIdentity = isIdentity;
        }

    }
}
