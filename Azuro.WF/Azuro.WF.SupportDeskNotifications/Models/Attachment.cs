using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class Attachment
    {
        public string Document { get; set; }

        public Guid ActivityId { get; set; }

        public Guid Id { get; set; }

        public int AttachmentNumber { get; set; }

        public string Body { get; set; }

        public string Filename { get; set; }

        public int Filesize { get; set; }

        public string Mimetype { get; set; }

        public string Subject { get; set; }

        public Guid ObjectId { get; set; }

        public string ObjectType { get; set; }
    }
}
