using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Azuro.Configuration;

namespace Azuro.Crm.KaseyaIntegration
{
    public class KaseyaConfigurationSectionHandler : ConfigurationSectionHandler<KaseyaConfigurationSection> { }

	
    [Serializable]
    [XmlRoot(SectionName)]
    public class KaseyaConfigurationSection : AConfigurationSection
    {
		string _temp = "Azuro.Kaseya.Intergration.KaseyaHelper, Azuro.Kaseya.Common";
        public const string SectionName = "Azuro.Kaseya.Intergration";

		[XmlElement("Integeration_Name")]
		public string IntegrationTypeName { get { return _temp; } set { _temp = value; } }

		[XmlIgnore]
		public Type IntegrationType
		{
			get { return string.IsNullOrEmpty(IntegrationTypeName) ? null : Type.GetType(IntegrationTypeName); }
		}

		[XmlElement("Kaseya_ws_host")]
		public string KaseyaWsHost { get; set; }

		[XmlElement("Kaseya_ws_module")]
		public string KaseyaWsModule { get; set; }

		[XmlElement("Kaseya_username")]
		public string KaseyaUsername { get; set; }

		[XmlElement("Kaseya_password")]
		public string KaseyaPassword { get; set; }

		[XmlElement("kaseya_crm_contact")]
		public string KaseyaCrmContact { get; set; }

		[XmlElement("Kaseya_hash_algorithm")]
		public string KaseyaHashAlgorithm { get; set; }

		[XmlElement("Kaseya_crm_account_name")]
		public string KaseyaCrmAccountName { get; set; }

		[XmlElement("Kaseya_crm_organisation_id")]
		public string KaseyaCrmOrganisationID { get; set; }

		[XmlElement("Kaseya_import_timer")]
		public int KaseyaImportTimer { get; set; }
    }
}
