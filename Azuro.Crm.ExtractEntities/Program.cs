using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Azuro.Common;
using System.IO;
using System.Configuration;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.ServiceModel.Description;
using System.Net;

namespace Azuro.Crm.ExtractEntities
{
	enum CrmAuthenticationType
	{
		IFD,
		Windows,
	}

	class Program
	{
		private static ClientCredentials CreateClientCredentials(CrmAuthenticationType authType, string username = null, string password = null)
		{
			ClientCredentials clientCredentials = new ClientCredentials();
			if (authType == CrmAuthenticationType.IFD)
			{
				clientCredentials.UserName.UserName = username;
				clientCredentials.UserName.Password = password;
			}
			else
			{
				if (string.IsNullOrEmpty(username))
				{
					clientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
					clientCredentials.Windows.ClientCredential = (NetworkCredential)CredentialCache.DefaultNetworkCredentials;
				}
				else
				{
					clientCredentials.Windows.ClientCredential = new NetworkCredential(username, password);
				}
			}

			return clientCredentials;
		}

		static void Main(string[] args)
		{
			string organization = string.Empty;
			do
			{
				Console.Write("Organization Url: ");
				organization = Console.ReadLine();
				if (string.IsNullOrEmpty(organization))
					organization = ConfigurationManager.AppSettings["OrganizationUrl"];
				if (string.IsNullOrEmpty(organization))
					Console.WriteLine("No Organization Url specified on command line or in config, please enter");
			} while (string.IsNullOrEmpty(organization));

			Console.Write("Output Path: ");
			var path = Console.ReadLine();
			if (string.IsNullOrEmpty(path))
				path = string.Format(@"C:\Development\Azuro\Azuro.Crm\Exported Entities\{0}", organization);
			Console.Write("Enter a specific class to export (or leave blank for all): ");
			var exportEntity = Console.ReadLine();

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			//			var helper = CrmHelperFactory.Create(new Guid(organization));
			var uri = string.Format("{0}/XRMServices/2011/Organization.svc", organization.TrimEnd('/'));
			var orgProxy = new Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy(new Uri(uri), null, CreateClientCredentials(CrmAuthenticationType.Windows), null);
			//var orgProxy = new Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy(new Uri(uri), null, CreateClientCredentials(CrmAuthenticationType.IFD, "azurodev\andreo", "April2015"), null);
			orgProxy.SdkClientVersion = "5.0.0.0";

			if (string.IsNullOrEmpty(exportEntity))
			{
				ExtractAllEntities(organization, path, orgProxy);
			}
			else
			{
				ExtractEntity(path, exportEntity, orgProxy);
			}

			Console.WriteLine("Done Creating Classes. Press a key.");
			Console.ReadKey();
		}

		private static void ExtractEntity(string path, string exportEntity, Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy orgProxy)
		{
			do
			{
				var entityRequest = new RetrieveEntityRequest
				{
					LogicalName = exportEntity,
					EntityFilters = EntityFilters.Attributes,
				};

				try
				{
					var entityResponse = (RetrieveEntityResponse)orgProxy.Execute(entityRequest);

					if (entityResponse.EntityMetadata.Attributes == null || entityResponse.EntityMetadata.Attributes.Length == 0)
					{
						Console.WriteLine("Skipping [{0}] due to no attributes", exportEntity);
					}

					Export.ExportEntity(entityResponse.EntityMetadata, path);
				}
				catch (Exception ex)
				{
					Console.WriteLine("An exception occurred: {1}{0}{2}", Environment.NewLine, ex.Message, ex.StackTrace);
				}
				Console.Write("Enter a specific class to export (or leave blank to end): ");
				exportEntity = Console.ReadLine();

			} while (!string.IsNullOrEmpty(exportEntity));
		}

		private static void ExtractAllEntities(string organization, string path, Microsoft.Xrm.Sdk.Client.OrganizationServiceProxy orgProxy)
		{
			Console.WriteLine("Retrieving all entities for [{0}]", organization);

			var request = new RetrieveAllEntitiesRequest()
			{
				EntityFilters = EntityFilters.Entity,
				RetrieveAsIfPublished = true
			};


			// Retrieve the MetaData.
			RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)orgProxy.Execute(request);

			Console.WriteLine("Exporting classes for [{0}] to [{1}]", organization, path);

			using (TextWriter twObjectType = File.CreateText(Path.Combine(path, "ObjectTypeCode.cs")))
			{
				twObjectType.WriteLine("using System;");
				twObjectType.WriteLine("using System.Collections.Generic;");
				twObjectType.WriteLine("using System.Linq;");
				twObjectType.WriteLine("using System.Text;");
				twObjectType.WriteLine("using System.Xml.Serialization;");
				twObjectType.WriteLine("using Azuro.Common;");
				twObjectType.WriteLine("using Azuro.Crm.Integration;");
				twObjectType.WriteLine("using System.IO;");
				twObjectType.WriteLine();
				twObjectType.WriteLine("namespace Azuro.Crm.Entities");
				twObjectType.WriteLine("{");
				twObjectType.WriteLine("\tpublic enum ObjectTypeCode");
				twObjectType.WriteLine("\t{");

				foreach (var entity in response.EntityMetadata)
				{
					var entityRequest = new RetrieveEntityRequest
					{
						LogicalName = entity.LogicalName,
						EntityFilters = EntityFilters.Attributes,
					};

					var entityResponse = (RetrieveEntityResponse)orgProxy.Execute(entityRequest);

					if (entityResponse.EntityMetadata.Attributes == null || entityResponse.EntityMetadata.Attributes.Length == 0)
					{
						Console.WriteLine("Skipping [{0}] due to no attributes", entity.LogicalName);
						continue;
					}

					twObjectType.WriteLine("\t\t{0} = {1},", Export.GetDisplayName(entity.DisplayName, entity.LogicalName), entity.ObjectTypeCode);
					Export.ExportEntity(entityResponse.EntityMetadata, path);
				}
				twObjectType.WriteLine("\t}");
				twObjectType.WriteLine("};");
			}
		}
	}
}
