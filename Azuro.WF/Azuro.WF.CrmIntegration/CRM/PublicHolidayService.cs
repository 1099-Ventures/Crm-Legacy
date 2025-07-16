using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Azuro.Models;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace Azuro.CrmIntegration.CRM
{
    public class PublicHolidayService : IPublicHolidayService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public PublicHolidayService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public PublicHoliday SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""new_publicholiday"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='new_publicholidayid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            PublicHoliday item = null;
            var singleResult = ((RetrieveEntityResponse)orgService.Execute(fetchRequest)).Results;

            if (singleResult != null)
            {
                item = new PublicHoliday();
                item.Id = (Guid)singleResult["new_publicholidayid"];
                item.Name = singleResult["new_name"].ToString();
                item.HolidayDate = (DateTime)singleResult["new_holidaydate"];

                if (singleResult["new_dontsms"] != null)
                    item.DontSms = (bool)singleResult["new_dontsms"];
                else
                    item.DontSms = false;

                if (singleResult["new_isreligious"] != null)
                    item.IsReligious = (bool)singleResult["new_isreligious"];
                else
                    item.IsReligious = false;
            }

            return item;
        }

        public List<PublicHoliday> SelectAll()
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""new_publicholiday"">
                                 <all-attributes/>
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            List<PublicHoliday> items = new List<PublicHoliday>();
            var results = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;

            if (results != null)
            {
                foreach (var singleResult in results.Entities)
                {
                    var item = new PublicHoliday();
                    item.Id = (Guid)singleResult["new_exclusiontimeid"];
                    item.Name = singleResult["new_name"].ToString();
                    item.HolidayDate = (DateTime)singleResult["new_holidaydate"];

                    if (singleResult["new_dontsms"] != null)
                        item.DontSms = (bool)singleResult["new_dontsms"];
                    else
                        item.DontSms = false;

                    if (singleResult["new_isreligious"] != null)
                        item.IsReligious = (bool)singleResult["new_isreligious"];
                    else
                        item.IsReligious = false;

                    items.Add(item);
                }
            }

            return items;
        }
        public OrganizationResponse Insert(PublicHoliday entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "new_publicholiday";
                detail.Attributes["new_name"] = entity.Name;

                // Create the request object.
                CreateRequest create = new CreateRequest();
                create.Target = detail;

                return (OrganizationResponse)orgService.Execute(create);
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

        public OrganizationResponse Update(PublicHoliday entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "new_publicholiday";

                detail.Attributes["new_publicholidayid"] = entity.Id;
                detail.Attributes["new_name"] = entity.Name;

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
    }
}
