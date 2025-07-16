using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Azuro.Crm.Integration.Nable.Entities
{
	public class CaseResolvedNotification : Azuro.Common.Messaging.SerializedMessage<CaseResolvedNotification>
	{
		[XmlElement("ExternalReference")]
		public int ExternalReference { get; set; }

		[XmlElement("CaseNumber")]
		public string CaseNumber { get; set; }

		[XmlElement("CaseStatus")]
		public string CaseStatus { get; set; }

		[XmlElement("ResolutionDescription")]
		public string ResolutionDescription { get; set; }
	}
}
