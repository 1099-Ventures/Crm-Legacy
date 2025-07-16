using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Test
{
	public class ConsoleTracingService : ITracingService, IDisposable
	{
		public void Dispose()
		{
			//	Dispose of stuff
		}

		public void Trace(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
	}
}
