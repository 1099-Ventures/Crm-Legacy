using Azuro.Configuration;
using System;
using System.Xml.Serialization;

namespace Azuro.Crm.Integration.Nable
{
	[Serializable]
	[XmlRoot(SectionName)]
	public class NableConfigurationSection : AConfigurationSection
	{
		public const string SectionName = "Azuro.Crm.Nable";

		[XmlAttribute("organizationId")]
		public Guid OrganizationId { get; set; }

		[XmlElement("NableLogin")]
		public NableLogin NableLoginDetails { get; set; }

		[XmlAttribute("crmUserId")]
		public string CrmUser { get; set; }

		[XmlAttribute("defaultSeverity")]
		public int DefaultSeverity { get; set; }

		[XmlAttribute("defaultAccountId")]
		public string __DefaultAccount { get; set; }

		[XmlIgnore]
		public Guid DefaultAccount
		{
			get { return string.IsNullOrEmpty(__DefaultAccount) ? Guid.Empty : Guid.Parse(__DefaultAccount); }
		}
	}

	public class NableLogin
	{
		[XmlAttribute("userName")]
		public string UserName { get; set; }
		[XmlAttribute("password")]
		public string Password { get; set; }
	}

	public class NableConfigurationSectionHandler : ConfigurationSectionHandler<NableConfigurationSection> { }
}
