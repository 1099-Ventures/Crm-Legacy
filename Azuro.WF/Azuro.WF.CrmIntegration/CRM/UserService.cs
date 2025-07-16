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
    public class UserService : IUserService
    {
       #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

       #region Constructor

        public UserService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

       #region Public Methods

        public User SelectByEmailAddress(string emailAddress)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""systemuser"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='internalemailaddress' operator='eq' value='" + emailAddress + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            User item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new User();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["systemuserid"].ToString());
            }

            return item;
        }

        #endregion
    }
}
