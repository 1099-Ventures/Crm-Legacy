using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Test
{
	public class SimpleOrganizationServiceFactory : IOrganizationServiceFactory, IDisposable
	{
		private readonly ConcurrentBag<OrganizationServiceProxy> _issuedProxies =
			new ConcurrentBag<OrganizationServiceProxy>();

		private string ServerName { get; set; }

		public SimpleOrganizationServiceFactory(string serverName) { ServerName = serverName; }

		public IOrganizationService CreateOrganizationService(Guid? userId)
		{
			Uri organisationUri = new Uri(string.Format("{0}/XRMServices/2011/Organization.svc", ServerName));

			var serviceProxy = new OrganizationServiceProxy(organisationUri, null, CreateClientCredentials(), null);
			_issuedProxies.Add(serviceProxy);
			return serviceProxy;
		}

		private ClientCredentials CreateClientCredentials()
		{
			ClientCredentials clientCredentials = new ClientCredentials();

			//	HACK: Configuration driven Org Factory and Auth needed

			clientCredentials.UserName.UserName = "henleysa\\crmadmin";
			clientCredentials.UserName.Password = "Sc0rpions";

			//if (Config.AuthenticationType == Azuro.Crm.Integration.CrmConfigurationSection.CrmAuthenticationType.IFD)
			//{
			//	clientCredentials.UserName.UserName = Config.UserName;
			//	clientCredentials.UserName.Password = Config.Password;
			//}
			//else
			//{
			//	clientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
			//	clientCredentials.Windows.ClientCredential = (NetworkCredential)CredentialCache.DefaultNetworkCredentials;
			//}

			return clientCredentials;
		}

		public void Dispose()
		{
			foreach (var serviceProxy in _issuedProxies)
			{
				serviceProxy.Dispose();
			}
		}
	}
}
