using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Azuro.Crm.KaseyaIntegration
{
	[Serializable]
	[XmlRoot("KaseyaTriggerEvent")]
	public class KaseyaTicketTriggerEventMessage : Azuro.Common.Messaging.SerializedMessage<KaseyaTicketTriggerEventMessage>
	{
		[XmlElement("incidentid")]
		public Guid Incidentid { get; set; }
		[XmlElement("organisationId")]
		public Guid OrganisationId { get; set; }
		[XmlElement("ticketnumber")]
		public string ticketnumber { get; set; }
	}
}
