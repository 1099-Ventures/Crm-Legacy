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
    public class AccountService : IAccountService
    {
         #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

       #region Constructor

        public AccountService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

       #region Public Methods

        public Account SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""account"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='accountid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };
         

            Account item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Account();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["accountid"].ToString());
            }

            return item;
        }


        public Account SelectByName(string name)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""account"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='name' operator='eq' value='" + name + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };
         

            Account item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Account();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["accountid"].ToString());
            }

            return item;
        }

        public Account SelectByEmail(string email)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""account"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='email' operator='eq' value='" + email + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Account item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Account();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["accountid"].ToString());
            }

            return item;
        }

        public Account SelectByEmailId(Guid id)
        {
            var fetch = @"<fetch mapping='logical' page='1' count='1'>
                                        <entity name='account'>
                                            <all-attributes/>
                                            <link-entity name='activityparty' from='partyid' to='accountid' alias='ad'>
                                                <link-entity name='email' from='activityid' to='activityid' alias='ae'>
                                                    <filter type='and'>
                                                        <condition attribute='activityid' operator='eq' value='" + id + @"' /> 
                                                    </filter>
                                                </link-entity>
                                            </link-entity>
                                        </entity>
                                    </fetch>";

            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Account item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;

            if (result.Entities.Count > 0)
            {
                item = new Account();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["accountid"].ToString());
            }

            return item;
        }

        #endregion
    
    }
}
