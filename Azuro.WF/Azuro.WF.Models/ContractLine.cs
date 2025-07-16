using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class ContractLine
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ProductId { get; set; }
    }
}
