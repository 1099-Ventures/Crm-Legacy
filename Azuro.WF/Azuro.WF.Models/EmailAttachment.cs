using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class EmailAttachment
    {
        public enum ObjectTypeCode
        {
            EmailTemplate = 2010,
            Email = 4200,
        };

        public Guid ActivityMimeAttachmentId { get; set; }
        public Guid ActivityId { get; set; }
        public Guid ObjectId { get; set; }
        public ObjectTypeCode ObjectTypeCode { get; set; }
        public string Subject { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Body { get; set; }
    }
}
