using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class Email
    {
        public Guid Id { get; set; }

        public string To { get; set; }

        public string From { get; set; }

        public string MobilePhone { get; set; }

        public string Subject { get; set; }

        public string SubjectCategory { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public Guid RegardingId { get; set; }

        public string RegardingType { get; set; }

        public Guid ContactId { get; set; }

        public Guid UserId { get; set; }
        
        public Guid QueueId { get; set; }

        public Guid OwnerId { get; set; }

        public string ToType { get; set; }

        public string FromType { get; set; }
    }
}
