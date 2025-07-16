using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common;

namespace Azuro.Crm.Integration
{
	public class CrmEntity
	{
		protected Dictionary<string, object> _attributes;
		protected Dictionary<string, object> _updatedAttributes;
		public string LogicalName { get; set; }
		public Dictionary<string, object> Attributes
		{
			get { return _attributes ?? (_attributes = new Dictionary<string, object>()); }
		}
		public Dictionary<string, object> UpdatedAttributes
		{
			get { return _updatedAttributes ?? (_updatedAttributes = new Dictionary<string, object>()); }
		}
		protected void AddUpdatedAttribute(string attributeName, object value)
		{
			if(UpdatedAttributes.ContainsKey(attributeName))
				UpdatedAttributes[attributeName]=value;
			else
				UpdatedAttributes.Add(attributeName,value);
		}
	}

	public abstract class CrmEntity<T> : CrmEntity
	{
		public static new string LogicalName { get { return AttributeHelper.GetCustomAttribute<CrmEntityAttribute>(typeof(T)).Name; } }
		public CrmEntity()
		{
			base.LogicalName = CrmEntity<T>.LogicalName;
		}
	}
}
