using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class QueueItem
    {
        public Guid Id { get; set; }

        public Guid ObjectId { get; set; }

        public string ObjectType { get; set; }

        public Guid OwnerUser { get; set; }

        public Guid QueueId { get; set; }
    }
}
