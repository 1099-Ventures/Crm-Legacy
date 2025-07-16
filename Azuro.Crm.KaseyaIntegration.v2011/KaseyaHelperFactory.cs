using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common;
using Azuro.Common.Configuration;

namespace Azuro.Crm.KaseyaIntegration
{
	public static class KaseyaHelperFactory
	{
		private static KaseyaConfigurationSection _config;
		private static KaseyaConfigurationSection Config
		{
			get { return _config ?? (_config = ConfigurationHelper.GetSection<KaseyaConfigurationSection>(KaseyaConfigurationSection.SectionName)); }

		}
		
		public static IKaseyaHelper Create()
		{
            IKaseyaHelper helper = Azuro.Common.Util.CreateObject<IKaseyaHelper>(Config.IntegrationType);
			return helper;
		}
	}
}
