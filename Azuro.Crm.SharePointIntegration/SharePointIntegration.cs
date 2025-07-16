using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using System.Xml;
using Microsoft.Xrm.Sdk.Query;

namespace Azuro.CRM.SharePointIntegration
{
	public class SharePointIntegration : IPlugin
	{
		string debugText = "";

		public void Execute(IServiceProvider serviceProvider)
		{
			debugText = "Inside Execute \n";
			System.Threading.Mutex mtx = null;
			try
			{
				Microsoft.Xrm.Sdk.IPluginExecutionContext context = (Microsoft.Xrm.Sdk.IPluginExecutionContext)
					serviceProvider.GetService(typeof(Microsoft.Xrm.Sdk.IPluginExecutionContext));
				if (context.InputParameters.Contains("Target") &&
				context.InputParameters["Target"] is Entity)
				{
					Entity entity = (Entity)context.InputParameters["Target"];
					debugText += "target \n";

					IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
					IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);


					Entity spSite = new Entity("sharepointsite");
					Entity spEntDocLoc = new Entity("sharepointdocumentlocation");
					Entity spDocLoc = new Entity("sharepointdocumentlocation");
					EntityCollection result = null;
					string defaultSharepointSite = "http://zeus:1901";
					string spSiteId = "AA391D48-8CE1-E111-A8E7-00155D80010D";
					string defaultSite = "Default Site";
					string defaultDocumentLocation = "";
					string defaultEntityLocation = entity.LogicalName;
					string fetchXmlTemplate = "", docLocAttr = "";
					string absoluteURL = "";
					bool useDefaults = false;

					Guid _spSiteId, _spDocEntId, _spDocId;

					defaultDocumentLocation = GetSharepointAttribute(entity.LogicalName);
					docLocAttr = entity.GetAttributeValue<string>(defaultDocumentLocation);

					if (docLocAttr == "" || docLocAttr == null || docLocAttr == "null")
					{
						throw new InvalidPluginExecutionException("Document location attribute is null");
					}

					absoluteURL = defaultSharepointSite + "/" + defaultEntityLocation + "/" + docLocAttr;
					debugText += absoluteURL;

					CreateSharePointFolder(absoluteURL);

					fetchXmlTemplate = @"<fetch mapping='logical'>
                                                <entity name='[ENTITY_NAME]'><all-attributes/>
                                                    <filter type=""and"">
                                                            <condition attribute=""[ATTR_NAME]"" operator=""eq"" value='[ATTR_VALUE]'/></filter></entity></fetch>";

					if (useDefaults)
					{
						_spSiteId = new Guid(spSiteId);
						_spDocEntId = new Guid(GetDocEntId(entity.LogicalName));
					}
					else
					{
						debugText = debugText + "Before executing fetch \n";
						result = GetResults(fetchXmlTemplate, "sharepointsite", "name", defaultSite, service);
						debugText = debugText + "After executing fetch \n";
						if (result.Entities.Count == 0)
						{
							debugText = debugText + "sharepointsite create \n";

							spSite.Attributes.Add("name", defaultSite);
							spSite.Attributes.Add("description", "Default Sharepoint Location");
							spSite.Attributes.Add("absoluteurl", defaultSharepointSite);
							// Create a SharePoint site record named Sample SharePoint Site.
							_spSiteId = service.Create(spSite);
						}
						else if (result.Entities.Count == 1)
						{
							debugText = debugText + "sharepointsite found \n";
							spSite = result.Entities.First();
							_spSiteId = result.Entities.First().Id;
						}
						else
						{
							throw new InvalidPluginExecutionException("Multiple sharepoint locations are found");
						}

						debugText = debugText + "GUID for sharepoint site " + spSite.LogicalName + " is " + _spSiteId.ToString();
						result = GetResults(fetchXmlTemplate, "sharepointdocumentlocation", "name", entity.LogicalName, service);
						debugText = debugText + "After executing fetch for sharepoint document location\n";
						if (result.Entities.Count == 0)
						{
							debugText += "sharepoint document site create \n";
							spEntDocLoc.Attributes.Add("name", entity.LogicalName);
							spEntDocLoc.Attributes.Add("description", "Default SharePoint Document Location record for " + entity.LogicalName);
							// Set the Sample SharePoint Site created earlier as the parent site.
							spEntDocLoc.Attributes.Add("parentsiteorlocation", new EntityReference("sharepointsite", _spSiteId));
							spEntDocLoc.Attributes.Add("relativeurl", entity.LogicalName);
							debugText += "before sharepoint document site create \n";
							_spDocEntId = service.Create(spEntDocLoc);
							debugText += "after sharepoint document site create \n";
						}
						else
						{
							spEntDocLoc = result.Entities.First();
							_spDocEntId = spEntDocLoc.Id;

						}
					}
					debugText = debugText + "sharepoint document site create \n";
					result = GetResults(fetchXmlTemplate, "sharepointdocumentlocation", "name", docLocAttr, service);
					debugText = debugText + "After executing fetch \n";
					if (result.Entities.Count == 0)
					{
						debugText = debugText + "sharepointsite create \n";
						spDocLoc.Attributes.Add("name", docLocAttr);
						spDocLoc.Attributes.Add("description", "Default SharePoint Document Location record for " + docLocAttr);
						// Set the Sample SharePoint Site created earlier as the parent site.
						spDocLoc.Attributes.Add("parentsiteorlocation", new EntityReference("sharepointdocumentlocation", _spDocEntId));
						spDocLoc.Attributes.Add("relativeurl", docLocAttr);
						// Associate this document location instance with the entity record
						spDocLoc.Attributes.Add("regardingobjectid", new EntityReference(entity.LogicalName, entity.Id));
						_spDocId = service.Create(spDocLoc);

					}


				}
			}
			catch (Exception ex)
			{
				if (mtx != null)
				{
					mtx.ReleaseMutex();
					mtx = null;
				}
				throw new InvalidPluginExecutionException("An error occured in SharepointIntegration plugin" + ex + "\n" + debugText);

			}
			finally
			{
				if (mtx != null)
				{
					mtx.ReleaseMutex();
					mtx = null;
				}
				debugText = null;
			}
		}

		private string GetDocEntId(string EntityName)
        {
            switch (EntityName)
            {
                case "account":
                    return "F379A2D4-8A9A-E111-8459-00155D01750A";
                case "opportunity":
                    return "642D28B5-479B-E111-8459-00155D01750A";
                default:
                    return "";
            }
        }

        private EntityCollection GetResults(string FetchXML, string EntityName, string AttrName, string AttrValue, IOrganizationService service)
        {
            FetchXML = FetchXML.Replace("[ENTITY_NAME]", EntityName);
            FetchXML = FetchXML.Replace("[ATTR_NAME]", AttrName);
            FetchXML = FetchXML.Replace("[ATTR_VALUE]", AttrValue);
            debugText = debugText + FetchXML;
            return service.RetrieveMultiple(new FetchExpression(FetchXML));
        }

        private static string GetSharepointAttribute(string EntityName)
        {
            switch (EntityName)
            {
                case "account":
                    return "accountid";
                case "opportunity":
                    return "name";
                default:
                    return "";
            }
        }

        private static void CreateSharePointFolder(string docfolderUrl)
        {
            if (docfolderUrl == String.Empty || docfolderUrl.IndexOf("/") == -1)
            {
                return;
            }
            try
            {
                // last part is the folder name
                string folderName = docfolderUrl.Substring(docfolderUrl.LastIndexOf("/") + 1);
                // remove the folder name
                docfolderUrl = docfolderUrl.Replace("/" + folderName, "");
                // get the document libray name
                string docLib = docfolderUrl.Substring(docfolderUrl.LastIndexOf("/") + 1);
                // now remove the doc lib to leave the sharepoint site url
                string sharePointSiteUrl = docfolderUrl.Replace("/" + docLib, "");

                SharePointList.Lists listsWS = new SharePointList.Lists();
                SharePointView.Views viewsWS = new SharePointView.Views();

                listsWS.Url = sharePointSiteUrl + "/_vti_bin/lists.asmx";
                viewsWS.Url = sharePointSiteUrl + "/_vti_bin/views.asmx";
                viewsWS.UseDefaultCredentials = true;
                viewsWS.UseDefaultCredentials = true;

                XmlNode viewCol = viewsWS.GetViewCollection(docLib);
                XmlNode viewNode = viewCol.SelectSingleNode("*[@DisplayName='All Documents']");
                string viewName = viewNode.Attributes["Name"].Value.ToString();

                /*Get Name attribute values (GUIDs) for list and view. */
                System.Xml.XmlNode ndListView = listsWS.GetListAndView(docLib, viewName);

                /*Get Name attribute values (GUIDs) for list and view. */
                string strListID = ndListView.ChildNodes[0].Attributes["Name"].Value;
                string strViewID = ndListView.ChildNodes[1].Attributes["Name"].Value;
                // load the CAML query
                XmlDocument doc = new XmlDocument();
                string xmlCommand;
                xmlCommand = "<Method ID='1' Cmd='New'><Field Name='FSObjType'>1</Field><Field Name='BaseName'>" + folderName + "</Field> <Field Name='ID'>New</Field></Method>";
                XmlElement ele = doc.CreateElement("Batch");
                ele.SetAttribute("OnError", "Continue");
                ele.SetAttribute("ListVersion", "1");
                ele.SetAttribute("ViewName", strViewID);

                ele.InnerXml = xmlCommand;

                XmlNode resultNode = listsWS.UpdateListItems(strListID, ele);

                // check for errors
                NameTable nt = new NameTable();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
                nsmgr.AddNamespace("tns", "http://schemas.microsoft.com/sharepoint/soap/");
                if (resultNode != null)
                { // look for error text in case of duplicate folder or invalid folder name
                    XmlNode errNode = resultNode.SelectSingleNode("tns:Result/tns:ErrorText", nsmgr);
                    if (errNode != null)
                    {
                        // Write error to log;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
	}
}
