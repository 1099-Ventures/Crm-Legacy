using System;

namespace Azuro.Models
{
    public class Sms
    {
        public Guid Id { get; set; }

        public string MobilePhone { get; set; }

        public string Message { get; set; }

        public string ACK { get; set; }

        public DateTime? SentDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int Direction { get; set; }

        public string Subject { get; set; }

        public string Status { get; set; }

        public Guid OwnerId { get; set; }

        public Guid RegardingId { get; set; }
    }
}
