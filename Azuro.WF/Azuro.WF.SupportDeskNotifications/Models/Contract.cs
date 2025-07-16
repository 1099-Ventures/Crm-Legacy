using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class Contract
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid DefaultContractLine { get; set; }
    }
}
