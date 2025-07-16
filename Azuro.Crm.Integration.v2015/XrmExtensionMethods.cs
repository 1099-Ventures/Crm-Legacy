using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Integration
{
	public static class XrmExtensionMethods
	{
		public static Money Sum<T>(this IEnumerable<T> list, Func<T, Money> selector) where T : Entity
		{
			return new Money(list.Sum(t => selector(t)?.Value ?? 0M));
		}
	}
}
