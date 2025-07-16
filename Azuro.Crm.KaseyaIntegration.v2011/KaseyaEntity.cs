using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common;

namespace Azuro.Crm.KaseyaIntegration
{
    public class KaseyaEntity
    {
        protected Dictionary<string, object> _attributes;
        public Dictionary<string, object> Attributes
        {
            get 
            {
				return _attributes ?? (_attributes = new Dictionary<string, object>());
            }
        }
    }

	public abstract class KaseyaEntity<T> : KaseyaEntity
	{
		public string EntityName
		{
			get 
			{
				return AttributeHelper.GetCustomAttribute<KaseyaEntityAttribute>(typeof(T)).Name;
			}
		} 
	}
}
