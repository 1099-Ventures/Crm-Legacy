using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Models
{
    public class PublicHoliday
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime HolidayDate { get; set; }

        public bool DontSms { get; set; }

        public bool IsReligious { get; set; }
    }
}
