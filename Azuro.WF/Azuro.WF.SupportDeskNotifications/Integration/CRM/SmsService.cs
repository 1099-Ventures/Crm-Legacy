using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Azuro.Models;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace Azuro.CrmIntegration.CRM
{
    public class SmsService : ISmsService
    {
        #region Public Members

        public IOrganizationService OrgService { get; set; }

        #endregion

        #region Constructor

        public SmsService(IOrganizationService orgService)
        {
            this.OrgService = orgService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Sms> SelectUnsent()
        {
            string fetch = @"<fetch mapping=""logical"">
                              <entity name=""new_sms"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                 "<condition attribute='new_ack' operator='eq' value='' /> " +
                                 @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            List<Sms> item = null;
            var result = ((RetrieveMultipleResponse)OrgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new List<Sms>();
                var singleResult = result.Entities[0];

                //item.Id = (Guid)singleResult["activityid"];
                //item.MobilePhone = singleResult["new_mobilephone"].ToString();
                //item.Subject = singleResult["new_subject"].ToString();
                //item.Message = singleResult["new_message"].ToString();
            }

            return item;
        }

        /// <summary>
        /// Select Sms by Id
        /// </summary>
        /// <param name="id">Sms Id</param>
        /// <returns>Sms object</returns>
        public Sms SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""new_sms"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='activityid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Sms item = null;
            var result = ((RetrieveMultipleResponse)OrgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Sms();
                var singleResult = result.Entities[0];

                item.Id = (Guid)singleResult["activityid"];
                item.MobilePhone = singleResult["new_mobilephone"].ToString();
                //item.Subject = singleResult["new_subject"].ToString();
                item.Message = singleResult["new_message"].ToString();
            }

            return item;
        }

        /// <summary>
        /// Insert Sms entity
        /// </summary>
        /// <param name="entity">Sms object</param>
        /// <returns>Insert response</returns>
        public OrganizationResponse Insert(Sms entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "azuro_sms";

                if (!String.IsNullOrEmpty(entity.Subject))
                    detail.Attributes["subject"] = entity.Subject;

                if (!String.IsNullOrEmpty(entity.Message))
                    detail.Attributes["azuro_message"] = entity.Message;

                if (!String.IsNullOrEmpty(entity.MobilePhone))
                    detail.Attributes["azuro_mobilephone"] = entity.MobilePhone;

                detail.Attributes["azuro_direction"] = new OptionSetValue(entity.Direction);

                detail.Attributes["regardingobjectid"] = new EntityReference("incident", entity.RegardingId);

                detail.Attributes["ownerid"] = new EntityReference("systemuser", entity.OwnerId);

                // Create the request object.
                var create = new CreateRequest {Target = detail};

                return OrgService.Execute(create);
            }
            catch (FaultException<OrganizationServiceFault> fex)
            {
                throw fex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Sms entity
        /// </summary>
        /// <param name="entity">Sms object</param>
        /// <returns>Execute response</returns>
        public OrganizationResponse Update(Sms entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "new_sms";

                detail.Attributes["activityid"] = entity.Id;
                detail.Attributes["new_ack"] = entity.ACK;

                // Create the request object.
                var update = new UpdateRequest();
                update.Target = detail;

                // Create the request object.
                return OrgService.Execute(update);
            }
            catch (FaultException<OrganizationServiceFault> fex)
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
