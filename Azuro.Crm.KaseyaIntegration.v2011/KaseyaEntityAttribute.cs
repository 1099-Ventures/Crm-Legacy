using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.KaseyaIntegration
{
    public class KaseyaEntityAttribute : Attribute
    {
        public string Name { get; set; }

        public KaseyaEntityAttribute(string name)
        {
            Name = name;
        }
    }
}
