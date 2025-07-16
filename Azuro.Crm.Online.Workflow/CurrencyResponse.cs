using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Azuro.Crm.Online.Workflow
{
	internal class CurrencyResponse
	{
		/// <summary>
		/// Hard-coded the parsing of the Json string because Partial Trust is an A-hole.
		/// </summary>
		/// <param name="input">The Json string returned from the service call.</param>
		public CurrencyResponse(string input)
		{
			//	Strip opening and closing brace
			var json = input.Trim('{', '}', '\n', ' ');
			var parms = json.Split('\n');
			bool rates = false;
			foreach (var p in parms)
			{
				var kvp = p.Split(':');
				var key = kvp[0].Trim(' ', '"', '\n');
				var value = kvp[1].Trim(',', '"', ' ', '\n');
				switch (key)
				{
					case "disclaimer":
						Disclaimer = value;
						break;
					case "license":
						License = value;
						break;
					case "base":
						Base = value;
						break;
					case "timestamp":
						Timestamp = long.Parse(value);
						break;
					case "rates":
						rates = true;
						Rates = new Dictionary<string, decimal>();
						break;
					default:
						if (rates)
						{
							//	Process Rates
							Rates.Add(key, decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
						}
						break;
				}
			}
		}
		public string Disclaimer { get; set; }

		public string License { get; set; }

		public long Timestamp { get; set; }

		public string Base { get; set; }

		public Dictionary<string, decimal> Rates { get; set; }
	}
}