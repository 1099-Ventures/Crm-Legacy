using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Configuration;
using System.Xml.Serialization;

namespace Azuro.Crm.Integration
{
	/// <summary>
	/// A Handler class to load the CrmConfigurationSection
	/// </summary>
	public class CrmConfigurationSectionHandler : ConfigurationSectionHandler<CrmConfigurationSection> { }

	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	[XmlRoot(SectionName)]
	public class CrmConfigurationSection : AConfigurationSection
	{
		public const string SectionName = "Azuro.Crm.Integration";

		[XmlElement("IntegrationType")]
		public string IntegrationTypeName { get; set; }

		[XmlIgnore]
		public Type IntegrationType
		{
			get { return string.IsNullOrEmpty(IntegrationTypeName) ? null : Type.GetType(IntegrationTypeName); }
		}

		[XmlElement("DiscoveryServiceUrl")]
		public string DiscoveryServiceUrl { get; set; }

		[XmlElement("AuthenticationType")]
		public CrmAuthenticationType AuthenticationType { get; set; }

		[XmlElement("UserName")]
		public string UserName { get; set; }

		[XmlElement("Password")]
		public string Password { get; set; }

#if DEBUG
		public CrmConfigurationSection()
		{
			//UserName = "azurodev\\-admin-johannu";
			//Password = "";
			AuthenticationType = CrmAuthenticationType.IFD;
			DiscoveryServiceUrl = "https://crm-dev.azuro.co.za/XRMServices/2011/Discovery.svc";
		}
#endif

		public enum CrmAuthenticationType
		{
			Windows,
			IFD,
		}
	}
}
