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
    public class QueueService : IQueueService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

         #region Constructor

        public QueueService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public Queue SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""queue"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='queueid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Queue item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Queue();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["queueid"].ToString());
            }

            return item;
        }

        public Queue GetQueueByName(string name)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""queue"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='name' operator='like' value='" + name + "%' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };
            
            Queue item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Queue();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["queueid"].ToString());
            }

            return item;
        }

        #endregion
    }
}
