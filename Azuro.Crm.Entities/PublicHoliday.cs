using Azuro.Crm.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Entities
{
	public class PublicHoliday : CrmEntity<PublicHoliday>
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public DateTime HolidayDate { get; set; }

		public bool DontSms { get; set; }

		public bool IsReligious { get; set; }
	}
}
