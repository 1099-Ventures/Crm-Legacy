using System;
using System.ServiceModel;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Azuro.CrmIntegration.CRM
{
    public class CaseService : ICaseService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public CaseService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public Case SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""incident"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='incidentid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Case item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Case();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["incidentid"].ToString());

                item.CaseRef = singleResult["ticketnumber"].ToString();

                if (singleResult.Attributes.ContainsKey("contractdetailid"))
                {
                    item.ContractLine = ((EntityReference)singleResult["contractdetailid"]).Id;
                }

                if (singleResult.Attributes.ContainsKey("contractid"))
                {
                    item.Contract = ((EntityReference)singleResult["contractid"]).Id;
                }

                if (singleResult.Attributes.ContainsKey("customerid"))
                {
                    item.CustomerId = ((EntityReference)singleResult["customerid"]).Id;
                }

                if (singleResult.Attributes.ContainsKey("ownerid"))
                {
                    item.OwnerId = ((EntityReference)singleResult["ownerid"]).Id;
                }
            }

            return item;
        }


        /// <summary>
        /// Insert case. 
        /// You have to unselect "Automatically move records to owners default queue..." on Case entity for this to insert.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CreateResponse Insert(Case entity)
        {
            try
            {
                var incident = new Entity();
                incident.LogicalName = "incident";
                incident.Attributes["title"] = entity.Title;
                incident.Attributes["description"] = entity.Description;

                if (entity.ContactId != Guid.Empty)
                {
                    incident.Attributes["customerid"] = new EntityReference("contact", entity.ContactId);
                }

                if(entity.Account != null && entity.Account.Id != Guid.Empty)
                {
                    incident.Attributes["customerid"] = new EntityReference("account", entity.Account.Id);
                }

                if (entity.Contract != Guid.Empty)
                {
                    incident.Attributes["contractid"] = new EntityReference("contract", entity.Contract);
                }

                if (entity.ContractLine != Guid.Empty)
                {
                    incident.Attributes["contractdetailid"] = new EntityReference("contractdetail", entity.ContractLine);
                }

                if (entity.ProductId != Guid.Empty)
                {
                    incident.Attributes["productid"] = new EntityReference("product", entity.ProductId);
                }

                // Create the case item.
                var create = new CreateRequest { Target = incident };

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


        //TODO: Still to do
        public OrganizationResponse Update(Case entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "new_sms";

                detail.Attributes["new_smsid"] = entity.Id;
                // detail.Attributes["new_subject"] = entity.Subject;

                // Create the request object.
                UpdateRequest update = new UpdateRequest();
                update.Target = detail;

                // Create the request object.
                return (OrganizationResponse)orgService.Execute(update);
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

        public Case SelectByReference(string reference)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""incident"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='subject' operator='like' value='%" + reference + "%' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Case item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Case();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["incidentid"].ToString());
            }

            return item;
        }
    }
}
