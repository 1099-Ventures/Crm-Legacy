using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Azuro.Crm.SmsMessages
{
	/// <summary>
	/// The sms response message (based on Clickatell's response fields, so may change in future)
	/// </summary>
	[Serializable]
	[XmlRoot("SMSResponse")]
	public class SmsResponseMessage : Azuro.Common.Messaging.SerializedMessage<SmsResponseMessage>
	{
		[XmlAttribute("receiverType")]
		public string ProviderReceiverType { get; set; }
		[XmlAttribute("providerId")]
		public string ProviderId { get; set; }
		[XmlAttribute("providerMsgId")]
		public string ProviderMsgId { get; set; }
		[XmlAttribute("clientMsgId")]
		public string ClientMsgId { get; set; }
		[XmlAttribute("status")]
		public MessageStatus Status { get; set; }
		[XmlElement("ProviderStatus")]
		public string ProviderStatus { get; set; }
		[XmlElement("ProviderStatusMessage")]
		public string ProviderStatusMessage { get; set; }
		[XmlElement("TimeStamp")]
		public DateTime ProviderTimeStamp { get; set; }
		[XmlElement("From")]
		public string From { get; set; }
		[XmlElement("To")]
		public string To { get; set; }
		[XmlElement("Charge")]
		public decimal Charge { get; set; }
	}
}
