using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class Note
    {
        public Guid Id { get; set; }

        public string Subject { get; set; }

        public string NoteText { get; set; }

        public Guid RegardingId { get; set; }

        public string RegardingType { get; set; }

        public string Filename { get; set; }

        public int Filesize { get; set; }

        public string Mimetype { get; set; }

        public string Document { get; set; }

        public string Body { get; set; }
    }
}
