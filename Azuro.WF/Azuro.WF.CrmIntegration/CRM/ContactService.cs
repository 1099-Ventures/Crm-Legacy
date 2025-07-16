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
    public class ContactService : IContactService
    {
       #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

       #region Constructor

        public ContactService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

       #region Public Methods

        public Contact SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""contact"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='contactid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Contact item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Contact();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["contactid"].ToString());
            }

            return item;
        }

        public Contact SelectByFullName(string fullname)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""contact"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='fullname' operator='like' value='" + fullname + "%' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            var fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            Contact item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Contact();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["contactid"].ToString());
            }

            return item;
        }

        public Contact SelectByEmailId(Guid id)
        {
            var fetch = @"<fetch mapping='logical' page='1' count='1'>
                                        <entity name='contact'>
                                            <attribute name='contactid' />
                                            <attribute name='firstname' /> 
                                            <attribute name='parentcustomerid' /> 
                                            <order attribute='fullname' descending='false' /> 
                                            <link-entity name='activityparty' from='partyid' to='contactid' alias='ad'>
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

            Contact item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            
            if (result.Entities.Count > 0)
            {
                item = new Contact();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["contactid"].ToString());
                
                //Set the account
                if (singleResult.Attributes.Contains("parentcustomerid"))
                {
                    var account = singleResult["parentcustomerid"] as EntityReference;
                    if (account != null) 
                        item.Account = new Account { Id = account.Id, Name = account.Name};
                }
            }

            return item;
        }

        public Contact SelectByEmail(string email)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""contact"">
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

            Contact item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new Contact();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["contactid"].ToString());
            }

            return item;
        }

        #endregion
    }
}
