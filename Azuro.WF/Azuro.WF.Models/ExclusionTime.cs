using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class ExclusionTime
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }

        public bool DontSms { get; set; }
    }
}
