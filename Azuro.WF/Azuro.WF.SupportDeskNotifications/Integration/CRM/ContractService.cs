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
    public class ContractService : IContractService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public ContractService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        public Contract SelectContractById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""contract"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='contractid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Contract item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Contract();

                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["contractid"].ToString());

                if (singleResult.Attributes.Contains("azuro_defaultseverity"))
                    item.DefaultContractLine = ((EntityReference)singleResult["azuro_defaultseverity"]).Id;
            }

            return item;
        }

        public Contract SelectContractByCustomerId(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""contract"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='customerid' operator='eq' value='" + id + "' /> " +
                                    "<condition attribute='statecode' operator='eq' value='2' /> " + 
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Contract item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Contract();

                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["contractid"].ToString());

                if (singleResult.Attributes.Contains("azuro_defaultseverity"))
                    item.DefaultContractLine = ((EntityReference)singleResult["azuro_defaultseverity"]).Id;
            }

            return item;
        }
    }
}
