using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class Case
    {
        public Guid Id { get; set; }

        public string CaseRef { get; set; }

        public string CaseType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IncidentStage { get; set; }

        public Guid ContactId { get; set; }

        public Account Account { get; set; }

        public Guid Contract { get; set; }

        public Guid ContractLine { get; set; }

        public Guid ProductId { get; set; }

        public Guid OwnerId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerType { get; set; }
    }
}
