using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Azuro.CrmIntegration.CRM
{
    public class SupportNotificationService : ISupportNotificationService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public SupportNotificationService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public SupportNotification SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""abs_supportnotification"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='abs_supportnotificationid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            SupportNotification item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new SupportNotification();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["abs_supportnotificationid"].ToString());
            }

            return item;
        }

        public List<SupportNotification> SelectByContractLine(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""azuro_supportnotification"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='azuro_supportitemid' operator='eq' value='" + id.ToString() + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };
            
            List<SupportNotification> results = new List<SupportNotification>();
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                foreach (var singleResult in result.Entities)
                {
                    var item = new SupportNotification();
                    item.Id = new Guid(singleResult["azuro_supportnotificationid"].ToString());

                    if (singleResult.Attributes.ContainsKey("azuro_event"))
                        item.Event = ((OptionSetValue) singleResult["azuro_event"]).Value;

                    if (singleResult.Attributes.ContainsKey("azuro_name"))
                        item.Name = singleResult["azuro_name"].ToString();

                    results.Add(item);
                }
            }

            return results;
        }

        #endregion
    }
}
