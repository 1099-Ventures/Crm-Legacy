using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Azuro.CrmIntegration;
using Azuro.CrmIntegration.CRM;
using Azuro.Models;

namespace Azuro.WF.SupportDeskNotifications
{
    public class NotificationActivity : CodeActivity
    {
        #region Private Members

        private ICaseService caseService;
        private IEmailService emailService;
        private ISmsService smsService;
        private ISupportNotificationService supportNotificationService;
        private INotificationChannelService notificationChannelService;

        private int eventType = 334070003;

        #endregion

        [Input("EntityReference input")]
        [ReferenceTarget("incident")]
        public InArgument<EntityReference> CaseReference { get; set; }

        [Input("Event type input")]
        public InArgument<int> EventType { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            if (CaseReference.Get(executionContext).Id == Guid.Empty)
                throw new InvalidPluginExecutionException("No case provided as input");

            this.eventType = EventType.Get(executionContext);

            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService orgService = serviceFactory.CreateOrganizationService(context.InitiatingUserId);

            this.Execute(CaseReference.Get(executionContext).Id, orgService);
        }

        public void Execute(Guid id, IOrganizationService orgService)
        {
            this.caseService = new CaseService(orgService);
            this.emailService = new EmailService(orgService);
            this.smsService = new SmsService(orgService);
            this.supportNotificationService = new SupportNotificationService(orgService);
            this.notificationChannelService = new NotificationChannelService(orgService);

            var caseResult = this.caseService.SelectById(id);
            if (caseResult != null)
            {
                if (caseResult.ContractLine != Guid.Empty)
                {
                    var notifications = this.supportNotificationService.SelectByContractLine(caseResult.ContractLine);

                    foreach (var notify in notifications)
                    {
                        if (notify.Event == this.eventType)
                        {
                            var channels = this.notificationChannelService.SelectBySupportNotification(notify.Id);

                            foreach (var channel in channels)
                            {
                                var users = this.notificationChannelService.SelectUsersForNotificationChannel(channel.Id);
                                var queues = this.notificationChannelService.SelectQueuesForNotificationChannel(channel.Id);
                                var contacts = this.notificationChannelService.SelectContactsForNotificationChannel(channel.Id);

                                if (channel.Channel == 334070000)
                                {
                                    foreach (var user in users)
                                        CreateEmailforUser(channel, notify, user.Id, caseResult);

                                    foreach (var queue in queues)
                                        CreateEmailforQueue(channel, notify, queue, caseResult);

                                    foreach (var contact in contacts)
                                        CreateEmailforContact(channel, notify, contact.Id, caseResult);

                                    if (channel.SendToOwner)
                                    {
                                        if (caseResult.OwnerId != Guid.Empty)
                                            CreateEmailforUser(channel, notify, caseResult.OwnerId, caseResult);
                                    }
                                    if (channel.SendToRequestor)
                                    {
                                        if (caseResult.CustomerId != Guid.Empty)
                                        {
                                            if (caseResult.CustomerType == "account")
                                                CreateEmailforAccount(channel, notify, caseResult.CustomerId, caseResult);
                                            else if (caseResult.CustomerType == "contact")
                                                CreateEmailforContact(channel, notify, caseResult.CustomerId, caseResult);
                                        }
                                    }
                                }
                                else if (channel.Channel == 334070001)
                                {
                                    foreach (var user in users)
                                        CreateSMSforUser(channel, notify, user, caseResult);

                                    foreach (var contact in contacts)
                                        CreateSMSforContact(channel, notify, contact, caseResult);

                                    if (channel.SendToOwner)
                                    {
                                    }
                                    if (channel.SendToRequestor)
                                    {
                                        if (caseResult.CustomerId != Guid.Empty)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateEmailforUser(NotificationChannel channel, SupportNotification notification, Guid itemId, Case caseItem)
        {
            var email = new Email();

            email.Description = channel.Message;
            email.To = itemId.ToString();
            email.From = caseItem.OwnerId.ToString();

            email.RegardingId = caseItem.Id;
            email.Subject = channel.Subject;
            email.UserId = itemId;
            email.OwnerId = caseItem.OwnerId;

            email.FromType = "systemuser";
            email.ToType = "systemuser";

            this.emailService.Insert(email);
        }

        private void CreateEmailforQueue(NotificationChannel channel, SupportNotification notification, Queue item, Case caseItem)
        {
            var email = new Email();

            email.Description = channel.Message;
            email.To = item.Id.ToString();
            email.From = caseItem.OwnerId.ToString();

            email.RegardingId = caseItem.Id;
            email.Subject = channel.Subject;
            email.UserId = item.Id;
            email.OwnerId = caseItem.OwnerId;

            email.FromType = "systemuser";
            email.ToType = "queue";

            this.emailService.Insert(email);
        }


        private void CreateEmailforContact(NotificationChannel channel, SupportNotification notification, Guid itemId, Case caseItem)
        {
            var email = new Email(); 
            
            email.Description = channel.Message;
            email.To = itemId.ToString();
            email.From = caseItem.OwnerId.ToString();

            email.RegardingId = caseItem.Id;
            email.Subject = channel.Subject;
            email.UserId = itemId;
            email.OwnerId = caseItem.OwnerId;

            email.FromType = "systemuser";
            email.ToType = "contact";

            this.emailService.Insert(email);
        }

        private void CreateEmailforAccount(NotificationChannel channel, SupportNotification notification, Guid itemId, Case caseItem)
        {
            var email = new Email();

            email.Description = channel.Message;
            email.To = itemId.ToString();
            email.From = caseItem.OwnerId.ToString();

            email.RegardingId = caseItem.Id;
            email.Subject = channel.Subject;
            email.UserId = itemId;
            email.OwnerId = caseItem.OwnerId;

            email.FromType = "systemuser";
            email.ToType = "account";

            this.emailService.Insert(email);
        }

        private void CreateSMSforUser(NotificationChannel channel, SupportNotification notification, User item, Case caseItem)
        {
            if (!string.IsNullOrEmpty(item.MobileNo))
            {
                var sms = new Sms();
                sms.MobilePhone = item.MobileNo;
                sms.Message = channel.Message;
                sms.Subject = channel.Subject;
                sms.OwnerId = caseItem.OwnerId;
                sms.RegardingId = caseItem.Id;
                sms.Direction = 0;

                this.smsService.Insert(sms);
            }
        }

        private void CreateSMSforContact(NotificationChannel channel, SupportNotification notification, Contact item, Case caseItem)
        {
            if (!string.IsNullOrEmpty(item.MobileNo))
            {
                var sms = new Sms();
                sms.MobilePhone = item.MobileNo;
                sms.Message = channel.Message;
                sms.Subject = channel.Subject;
                sms.OwnerId = caseItem.OwnerId;
                sms.RegardingId = caseItem.Id;
                sms.Direction = 0;

                this.smsService.Insert(sms);
            }
        }
    }
}
