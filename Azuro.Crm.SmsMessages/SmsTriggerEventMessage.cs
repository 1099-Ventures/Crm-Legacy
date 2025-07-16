using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Azuro.Crm.SmsMessages
{
	[Serializable]
	[XmlRoot("SMSTriggerEvent")]
	public class SmsTriggerEventMessage : Azuro.Common.Messaging.SerializedMessage<SmsTriggerEventMessage>
	{
		[XmlAttribute("activityId")]
		public Guid ActivityId { get; set; }
		[XmlAttribute("organisationId")]
		public Guid OrganisationId { get; set; }
	}
}
