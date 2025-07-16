using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Entities
{
	/// <summary>
	/// This class represents the support calendar as per the contract template entity.
	/// It provides calculation methods for determine actual hours of support
	/// </summary>
	public class SupportCalendar
	{
		//	TODO: Add support for public holidays

		Dictionary<KeyValuePair<DayOfWeek, int>, bool> _supportCalendar;
		private Dictionary<KeyValuePair<DayOfWeek, int>, bool> Calendar
		{
			get { return _supportCalendar ?? (_supportCalendar = new Dictionary<KeyValuePair<DayOfWeek, int>, bool>()); }
		}

		public SupportCalendar(string effectivityCalendar)
		{
			var dow = DayOfWeek.Sunday;
			var hour = 0;
			foreach (var c in effectivityCalendar)
			{
				Calendar.Add(new KeyValuePair<DayOfWeek, int>(dow, hour++), (c == '+'));
				if (hour == 24)
				{
					if (dow < DayOfWeek.Saturday)
						dow++;
					else
						break;
					hour = 0;
				}
			}
		}

		public DateTime AddMinutes(DateTime startDate, int minutes)
		{
			//	Check whether startTime is inside of support time
			DateTime endDate = startDate;
			for (int i = 0; i < minutes; )
			{
				if (Calendar[new KeyValuePair<DayOfWeek, int>(endDate.DayOfWeek, endDate.Hour)])
					i++;
				endDate = endDate.AddMinutes(1);
			}

			return endDate;
		}


		public DateTime AddHours(DateTime startDate, int hours)
		{
			//	Check whether startTime is inside of support time
			DateTime endDate = startDate;
			for (int i = 0; i < hours; )
			{
				if (Calendar[new KeyValuePair<DayOfWeek, int>(endDate.DayOfWeek, endDate.Hour)])
					i++;
				endDate.AddHours(1);
			}

			return endDate;
		}
	}
}
