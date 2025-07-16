using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.Entities
{
	[CrmEntity("azuro_smsconfiguration")]
	public class SmsConfiguration : CrmEntity<SmsConfiguration>
	{
		[CrmField("azuro_smsconfigurationid", IsIdentity = true)]
		public Guid SmsConfigurationId { get; set; }

		[CrmField("azuro_accountinternationaldialcode")]
		public string AccountInternationalDialCode { get; set; }

		[CrmField("azuro_contactinternationaldialcode")]
		public string ContactInternationalDialCode { get; set; }

		[CrmField("azuro_leadinternationaldialcode")]
		public string LeadInternationalDialCode { get; set; }

		[CrmField("azuro_accountmobilephonenumber")]
		public string AccountMobilePhoneNumber { get; set; }

		[CrmField("azuro_contactmobilephonenumber")]
		public string ContactMobilePhoneNumber { get; set; }

		[CrmField("azuro_leadmobilephonenumber")]
		public string LeadMobilePhoneNumber { get; set; }

		[CrmField("azuro_smstriggereventqueue")]
		public string SmsTriggerEventQueue { get; set; }

		[CrmField("azuro_smsfootermessage")]
		public string SmsFooterMessage { get; set; }

		[CrmField("azuro_smsprovider")]
		public SmsProvider SmsProvider { get; set; }

		[CrmField("azuro_allowmultipartmessages")]
		public AllowMultipartMessages AllowMultipartMessages { get; set; }

		public short NumberOfMessages
		{
			get
			{
				if (!Enum.IsDefined(typeof(AllowMultipartMessages), AllowMultipartMessages)) return 1;
				var dv = Azuro.Common.AttributeHelper.GetCustomAttribute<System.ComponentModel.DefaultValueAttribute>(typeof(AllowMultipartMessages).GetMember(this.AllowMultipartMessages.ToString())[0]);
				return Convert.ToInt16(dv.Value);
			}
		}
	}
}
