using System;

namespace Azuro.Models
{
    public class Contact
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public Account Account { get; set; }

        public string MobileNo { get; set; }
    }
}
