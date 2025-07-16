using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common;
using Azuro.Common.Configuration;

namespace Azuro.Crm.Integration
{
	public static class CrmHelperFactory
	{
		private static CrmConfigurationSection _config;
		private static CrmConfigurationSection Config
		{
			get { return _config ?? (_config = ConfigurationHelper.GetSection<CrmConfigurationSection>(CrmConfigurationSection.SectionName)); }
		}

		public static ICrmHelper Create(Guid orgId)
		{
			if (orgId == null)
				throw new ArgumentNullException("orgId", "The Organisation Id must be supplied.");
			//	Create Type
			if (Config == null)
				throw new NullReferenceException("Config object is null");
			else if (Config.IntegrationType == null)
				throw new NullReferenceException(string.Format("IntegrationType could not be found [{0}]", Config.IntegrationTypeName));
			else if (string.IsNullOrEmpty(Config.DiscoveryServiceUrl))
				throw new NullReferenceException("The Discovery Service Url cannot be Null.");

			ICrmHelper helper = Util.CreateObject<ICrmHelper>(Config.IntegrationType);
			helper.OrganisationId = orgId;

			return helper;
		}
	}
}
