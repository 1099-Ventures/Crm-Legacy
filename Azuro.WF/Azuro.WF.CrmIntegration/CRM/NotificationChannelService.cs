using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Azuro.CrmIntegration.CRM
{
    public class NotificationChannelService : INotificationChannelService
    {
        #region Public Members

        public IOrganizationService orgService { get; set; }

        #endregion

        #region Constructor

        public NotificationChannelService(IOrganizationService _orgService)
        {
            this.orgService = _orgService;
        }

        #endregion

        #region Public Methods

        public NotificationChannel SelectById(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""abs_azuro_notificationcommunication"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='azuro_notificationcommunicationid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            NotificationChannel item = null;
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                item = new NotificationChannel();
                var singleResult = result.Entities[0];
                item.Id = new Guid(singleResult["azuro_notificationcommunicationid"].ToString());

                if (singleResult.Attributes.ContainsKey("azuro_sendtorequestor"))
                    item.SendToRequestor = (bool)singleResult["azuro_sendtorequestor"];

                if (singleResult.Attributes.ContainsKey("azuro_sendtoowner"))
                    item.SendToOwner = (bool)singleResult["azuro_sendtoowner"];
            }

            return item;
        }

        public List<NotificationChannel> SelectBySupportNotification(Guid id)
        {
            var fetch = @"<fetch mapping=""logical"">
                              <entity name=""azuro_notificationcommunication"">
                                 <all-attributes/>
                                <filter type='and'>  " +
                                    "<condition attribute='azuro_supportitemid' operator='eq' value='" + id + "' /> " +
                                @"</filter> 
                              </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };
            
            List<NotificationChannel> results = new List<NotificationChannel>();
            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                foreach (var singleResult in result.Entities)
                {
                    var item = new NotificationChannel();
                    item.Id = new Guid(singleResult["azuro_notificationcommunicationid"].ToString());

                    if (singleResult.Attributes.ContainsKey("azuro_channel"))
                        item.Channel = ((OptionSetValue) singleResult["azuro_channel"]).Value;

                    if (singleResult.Attributes.ContainsKey("azuro_name"))
                        item.Name = singleResult["azuro_name"].ToString();

                    if (singleResult.Attributes.ContainsKey("azuro_message"))
                        item.Message = singleResult["azuro_message"].ToString();

                    if (singleResult.Attributes.ContainsKey("azuro_sendtorequestor"))
                        item.SendToRequestor = (bool) singleResult["azuro_sendtorequestor"];

                    if (singleResult.Attributes.ContainsKey("azuro_sendtoowner"))
                        item.SendToOwner = (bool)singleResult["azuro_sendtoowner"];

                    results.Add(item);
                }
            }

            return results;
        }

        public List<User> SelectUsersForNotificationChannel(Guid id)
        {
            var results = new List<User>();

            var fetch = @"<fetch mapping=""logical"">
                       <entity name=""azuro_notificationcommunication"">
                      <filter type='and'>  " +
                     "<condition attribute='azuro_notificationcommunicationid' operator='eq' value='" + id + "' /> " +
                    @"</filter> 
                      <link-entity name=""azuro_azuro_notificationcommunication_systemu"" from=""azuro_notificationcommunicationid"" to=""azuro_notificationcommunicationid"">
                        <link-entity name=""systemuser"" from=""systemuserid"" to=""systemuserid"">
                            <all-attributes/>
		                </link-entity>
                      </link-entity> 	
                      </entity>	
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                foreach (var singleResult in result.Entities)
                {
                    var item = new User();
                    item.Id = new Guid(((AliasedValue)singleResult["systemuser2.systemuserid"]).Value.ToString());

                    results.Add(item);
                }
            }

            return results;
        }

        public List<Queue> SelectQueuesForNotificationChannel(Guid id)
        {
            var results = new List<Queue>();

            var fetch = @"<fetch mapping=""logical"">
                       <entity name=""azuro_notificationcommunication"">
                         <all-attributes/>
                      <filter type='and'>  " +
                     "<condition attribute='azuro_notificationcommunicationid' operator='eq' value='" + id + "' /> " +
                    @"</filter> 		
    		            <link-entity name=""azuro_azuro_notificationcommunication_queue"" from=""azuro_notificationcommunicationid"" to=""azuro_notificationcommunicationid"">
			             <link-entity name=""queue"" from=""queueid"" to=""queueid"" >
                            <all-attributes/>
                            </link-entity>
		                </link-entity>
                        </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                foreach (var singleResult in result.Entities)
                {
                    var item = new Queue();
                    item.Id = new Guid(((AliasedValue)singleResult["queue2.queueid"]).Value.ToString());

                    results.Add(item);
                }
            }

            return results;
        }

        public List<Contact> SelectContactsForNotificationChannel(Guid id)
        {
            var results = new List<Contact>();

            var fetch = @"<fetch mapping=""logical"">
                      <entity name=""azuro_notificationcommunication"">
                      <filter type='and'>  " +
                     "<condition attribute='azuro_notificationcommunicationid' operator='eq' value='" + id + "' /> " +
                    @"</filter> 		
                        <link-entity name=""azuro_azuro_notificationcommunication_contact"" from=""azuro_notificationcommunicationid"" to=""azuro_notificationcommunicationid"">
			                <link-entity name=""contact"" from=""contactid"" to=""contactid"">
                              <all-attributes/>
                            </link-entity>
		                </link-entity>
                        </entity> 
                      </fetch>";

            // Excute the fetch query and get the xml result.
            RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
            {
                Query = new FetchExpression(fetch)
            };

            var result = ((RetrieveMultipleResponse)orgService.Execute(fetchRequest)).EntityCollection;
            if (result.Entities.Count > 0)
            {
                foreach (var singleResult in result.Entities)
                {
                    var item = new Contact();
                    item.Id = new Guid(((AliasedValue)singleResult["contact2.contactid"]).Value.ToString());

                    results.Add(item);
                }
            }

            return results;
        }

        #endregion
    }
}
