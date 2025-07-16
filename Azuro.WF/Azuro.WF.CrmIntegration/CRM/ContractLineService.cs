using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Azuro.CrmIntegration.CRM
{
    public class ContractLineService : IContractLineService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

       #region Constructor

        public ContractLineService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        public ContractLine SelectContractLineById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""contractdetail"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='contractdetailid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            ContractLine item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new ContractLine();

                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["contractdetailid"].ToString());

                if (singleResult.Attributes.Contains("productid"))
                    item.ProductId = ((EntityReference)singleResult["productid"]).Id;
            }

            return item;
        }
    }
}
