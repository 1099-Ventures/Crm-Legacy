using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Azuro.CrmIntegration.CRM
{
    public class QueueItemService : IQueueItemService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public QueueItemService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public QueueItem SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""queueitem"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='queueitemid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            QueueItem item = null;
            var singleResult = orgService.Execute(fetchRequest).Results;

            if (singleResult != null)
            {
                item = new QueueItem();

                var queueItem = ((EntityCollection)singleResult.Values.First()).Entities[0].Attributes.Values.ToArray();

                item.Id = (Guid)singleResult["new_smsid"];
                //item.Subject = singleResult["new_subject"].ToString();
            }

            return item;
        }

        public QueueItem GetQueueByName(string name)
        {
            //was for getting Queue by email
            //if (type.Contains("@"))
            //{
            //    var split1 = type.Split('@');
            //    var split2 = split1[1].Split('.');
            //    type = split2[0];
            //}

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

            QueueItem item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new QueueItem();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["activityid"].ToString());
                item.ObjectId = new Guid(singleResult["objectid"].ToString());
                item.QueueId = new Guid(singleResult["queueid"].ToString());
            }


            return item;
        }

        public OrganizationResponse Insert(QueueItem entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "queueitem";
                detail.Attributes["objectid"] = new EntityReference(entity.ObjectType, entity.ObjectId);
                detail.Attributes["queueid"] = new EntityReference("queue", entity.QueueId);

                // Create the request object.
                var create = new CreateRequest();
                create.Target = detail;

                return orgService.Execute(create);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fex)
            {
                throw fex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OrganizationResponse Delete(Guid id)
        {
            try
            {
                var detail = new Entity();

                // Create the request object.
                var delete = new DeleteRequest();
                delete.Target = new EntityReference("queueitem", id);

                return orgService.Execute(delete);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fex)
            {
                throw fex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
