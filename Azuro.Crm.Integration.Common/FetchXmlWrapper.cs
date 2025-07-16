using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Azuro.Crm.Integration
{
	/*
	<fetch mapping="logical">
        <entity name="new_sms">
            <all-attributes/>
			<filter type='and'>
				<condition attribute='new_ack' operator='eq' value='' />
			</filter> 
        </entity> 
	</fetch>
	*/

	public enum FilterMapping
	{
		Logical,
		Physical,
	}

	public enum FilterType
	{
		[System.ComponentModel.DefaultValue("and")]
		And,
		[System.ComponentModel.DefaultValue("or")]
		Or,
		[System.ComponentModel.DefaultValue("xor")]
		Xor,
	}

	public enum FilterOperator
	{
		[System.ComponentModel.DefaultValue("eq")]
		Equal,
		[System.ComponentModel.DefaultValue("ne")]
		NotEqual,
		[System.ComponentModel.DefaultValue("gt")]
		GreaterThan,
		[System.ComponentModel.DefaultValue("lt")]
		LessThan,
		[System.ComponentModel.DefaultValue("gte")]
		GreaterThanEqual,
		[System.ComponentModel.DefaultValue("lte")]
		LessThanEqual,
		[System.ComponentModel.DefaultValue("like")]
		Contains,
		[System.ComponentModel.DefaultValue("on-or-before")]
		OnOrBefore,
		[System.ComponentModel.DefaultValue("on-or-after")]
		OnOrAfter,
		[System.ComponentModel.DefaultValue("on")]
		On,
	}

	[XmlRoot("fetch")]
	public class FetchXmlWrapper
	{
		[XmlAttribute("mapping")]
		public FilterMapping Mapping { get; set; }

		[XmlElement("entity")]
		public FetchXmlEntity Entity { get; set; }
	}

	public class FetchXmlEntity
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlElement("filter")]
		public List<Filter> Filters { get; set; }
	}

	public class Filter
	{
		[XmlAttribute("type")]
		public FilterType Type { get; set; }

		[XmlIgnore]
		public bool AllAttributes { get; set; }

		[XmlElement("condition")]
		public List<FilterCondition> Conditions { get; set; }
	}

	public class FilterCondition
	{
		[XmlAttribute("attribute")]
		public string Attribute { get; set; }

		[XmlAttribute("operator")]
		public FilterOperator Operator { get; set; }

		[XmlAttribute("value")]
		public string Value { get; set; }
	}
}
