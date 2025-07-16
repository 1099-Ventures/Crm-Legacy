using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class NotificationChannel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public int Channel { get; set; }

        public bool SendToRequestor { get; set; }

        public bool SendToOwner { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
