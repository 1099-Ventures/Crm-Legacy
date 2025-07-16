using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.KaseyaIntegration
{
	public class InvalidSessionIDException : Exception
	{
		public InvalidSessionIDException(string message)
			: base(message)
		{ }
	}
}
