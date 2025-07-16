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
    public class ExclusionTimeService : IExclusionTimeService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

         #region Constructor

        public ExclusionTimeService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public ExclusionTime SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""new_exclusiontime"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='new_exclusiontimeid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            ExclusionTime item = null;
            var singleResult = ((RetrieveEntityResponse)orgService.Execute(fetchRequest)).Results;

            if (singleResult != null)
            {
                item = new ExclusionTime();
                item.Id = (Guid)singleResult["new_exclusiontimeid"];
                item.Name = singleResult["new_name"].ToString();
                item.TimeFrom = singleResult["new_timefrom"].ToString();
                item.TimeTo = singleResult["new_timeto"].ToString();

                if (singleResult["new_dontsms"] != null)
                    item.DontSms = (bool)singleResult["new_dontsms"];
                else
                    item.DontSms = false;
            }

            return item;
        }

        public List<ExclusionTime> SelectAll()
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""new_exclusiontime"">
                                 <all-attributes/>
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            List<ExclusionTime> items = new List<ExclusionTime>();
            var results = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;

            if (results != null)
            {
                foreach (var singleResult in results.Entities)
                {
                    var item = new ExclusionTime();
                    item.Id = (Guid)singleResult["new_exclusiontimeid"];
                    item.Name = singleResult["new_name"].ToString();
                    item.TimeFrom = singleResult["new_timefrom"].ToString();
                    item.TimeTo = singleResult["new_timeto"].ToString(); 
                    
                    if (singleResult["new_dontsms"] != null)
                        item.DontSms = (bool)singleResult["new_dontsms"];
                    else
                        item.DontSms = false;

                    items.Add(item);
                }
            }

            return items;
        }

        public OrganizationResponse Insert(ExclusionTime entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "new_exclusiontime";
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

        public OrganizationResponse Update(ExclusionTime entity)
        {
            try
            {
                var detail = new Entity();
                detail.LogicalName = "new_exclusiontime";

                detail.Attributes["new_exclusiontimeid"] = entity.Id;
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
