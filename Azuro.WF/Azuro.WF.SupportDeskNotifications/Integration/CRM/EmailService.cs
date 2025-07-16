using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace Azuro.CrmIntegration.CRM
{
    public class EmailService : IEmailService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public EmailService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Inserts an item into Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public CreateResponse Insert(Email email)
        {
            try
            {
                var entity = new Entity();
                entity.LogicalName = "email";

                entity.Attributes["subject"] = email.Subject;
                entity.Attributes["description"] = email.Description;

                if (email.RegardingId != Guid.Empty)
                    entity.Attributes["regardingobjectid"] = new EntityReference("incident", email.RegardingId);

                if (!String.IsNullOrEmpty(email.From) && !String.IsNullOrEmpty(email.FromType))
                {
                    var from = new EntityCollection();
                    var fromEntity = new Entity("activityparty");
                    fromEntity.Attributes["partyid"] = new EntityReference(email.FromType, new Guid(email.From));
                    from.Entities.Add(fromEntity);
                    entity.Attributes["from"] = from;
                }

                if (!String.IsNullOrEmpty(email.To) && !String.IsNullOrEmpty(email.ToType))
                {
                    var to = new EntityCollection();
                    var toEntity = new Entity("activityparty");
                    toEntity.Attributes["partyid"] = new EntityReference(email.ToType, new Guid(email.To));
                    to.Entities.Add(toEntity);
                    entity.Attributes["to"] = to;
                }

                entity.Attributes["ownerid"] = new EntityReference("systemuser", email.OwnerId);

                // Create the case item.
                var create = new CreateRequest { Target = entity };
                var result = (CreateResponse)orgService.Execute(create);

                return result;

            }
            catch (FaultException<OrganizationServiceFault> fex)
            {
                throw fex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public Email SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""email"">
                                    <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='activityid' operator='eq' value='" + id + "' /> " +
                                @"</filter>
                              </entity> 
                      </fetch>";

            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Email item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Email();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["activityid"].ToString());
                item.Subject = singleResult["subject"].ToString();
                item.Description = singleResult["description"].ToString();

                if (singleResult.GetAttributeValue<EntityReference>("regardingobjectid") != null)
                {
                    var regarding = ((EntityReference)singleResult["regardingobjectid"]);
                    item.RegardingId = regarding.Id;
                    item.RegardingType = regarding.LogicalName;
                }

                item.UserId = singleResult.GetAttributeValue<EntityCollection>("from").Entities[0].Id;
            }

            return item;
        }

        public Email SelectBySubject(string subject)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""email"">
                                    <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='subject' operator='like' value='%" + subject + "%' /> " +
                                @"</filter>
                              </entity> 
                      </fetch>";

            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Email item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Email();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["activityid"].ToString());
                item.Subject = singleResult["subject"].ToString();
                item.RegardingId = new Guid(singleResult["regardingobjectid"].ToString());
                item.Description = singleResult["description"].ToString();
                item.UserId = singleResult.GetAttributeValue<EntityCollection>("from").Entities[0].Id;
            }

            return item;
        }

        public OrganizationResponse Update(Email entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "email";
                detail.Attributes["activityid"] = entity.Id;
                detail.Attributes["regardingobjectid"] = new EntityReference("incident", entity.RegardingId);

                // Create the request object.
                var update = new UpdateRequest { Target = detail };

                // Create the request object.
                return orgService.Execute(update);
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
