using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Azuro.Crm.Integration;
using Microsoft.Crm.Sdk.Messages;

namespace Azuro.Crm.Workflow.SupportDeskNotifications
{
	public class NotificationActivity : ACrmCodeActivity
	{
		[Input("Entity Reference (Case) (Input)")]
		[ReferenceTarget("incident")]
		public InArgument<EntityReference> CaseReference { get; set; }

		[Input("Entity Reference (Note) (Input)")]
		[ReferenceTarget("annotation")]
		public InArgument<EntityReference> NoteReference { get; set; }

		[Input("Event Type (Input)")]
		[AttributeTarget("azuro_notificationcommunication", "azuro_event")]
		public InArgument<OptionSetValue> EventType { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			var caseId = CaseReference.Get(context).Id;
			if (caseId == Guid.Empty)
				throw new InvalidPluginExecutionException("No case provided as input");

			ProcessNotifications(context, caseId, (Azuro.Crm.Entities.NotificationEventType)EventType.Get(context).Value);
		}

		private void ProcessNotifications(CodeActivityContext context, Guid caseId, Azuro.Crm.Entities.NotificationEventType eventType)
		{
			Trace(context, "Entering ProcessNotifications");

			var crmHelper = GetCrmHelper(context);
			var caseItem = crmHelper.GetEntity<Entities.Case>(caseId);

			Trace(context, "CaseItem found - [{0}]", caseItem != null);

			if (caseItem != null)
			{
				List<Entities.NotificationCommunication> notifications = null;
				if (CrmEntityReference.IsValid(caseItem.ContractId))
				{
					notifications = crmHelper.GetEntityList<Entities.NotificationCommunication>(new List<CrmQuery>
																						{ 
																							new CrmQuery
																							{
																								Key = "contractid", 
																								Value = caseItem.ContractId.ReferencedEntityId, 
																								Operator = FilterOperator.Equal, 
																								FilterType = FilterType.And, 
																							},	
																						},
																						new CrmLinkReference
																						{
																							LinkName = "azuro_contract_azuro_notificationcomms",
																							FromAttribute = "azuro_notificationcommunicationid",
																							ToAttribute = "azuro_notificationcommunicationid"
																						});

					Trace(context, "Retrieved notifications - [{0}]", notifications != null ? notifications.Count.ToString() : "Null");

					if (notifications != null && notifications.Count > 0)
					{
						ProcessNotifications(context, crmHelper, eventType, caseItem, notifications);
						return;
					}
				}

				notifications = crmHelper.GetEntityList<Entities.NotificationCommunication>(new List<CrmQuery>
																						{ 
																							new CrmQuery
																							{
																								Key = "azuro_isdefault", 
																								Value = true, 
																								Operator = FilterOperator.Equal, 
																								FilterType = FilterType.And, 
																							},
																							new CrmQuery
																							{
																								Key = "statecode", 
																								Value = 0, 
																								Operator = FilterOperator.Equal, 
																								FilterType = FilterType.And, 
																							},
																						});

				Trace(context, "Retrieved default notifications - [{0}]", notifications != null ? notifications.Count.ToString() : "Null");

				ProcessNotifications(context, crmHelper, eventType, caseItem, notifications);
			}
		}

		private void ProcessNotifications(CodeActivityContext context, ICrmHelper crmHelper, Azuro.Crm.Entities.NotificationEventType eventType, Entities.Case caseItem, List<Entities.NotificationCommunication> notifications)
		{
			foreach (var notification in notifications)
			{
				Trace(context, "ApplicableCaseOrigins: {0} | CaseOriginCode: {1}", caseItem.CaseOriginCode, notification.ApplicableCaseOrigins);

				if ((Entities.NotificationEventType)notification.Event == eventType
					&& (notification.ValidForAllSeverities.GetValueOrDefault(true) || caseItem.Severity == notification.Severity)
					&& (notification.ApplicableToAllCaseOrigins.GetValueOrDefault(true) || CaseOriginHelper.IsValidOrigin(crmHelper, caseItem.CaseOriginCode, notification.ApplicableCaseOrigins)))
				{
					if ((Entities.CommunicationChannel)notification.Channel == Entities.CommunicationChannel.Email)
					{
						CreateEmailNotifications(context, crmHelper, notification, caseItem);
					}
					else if ((Entities.CommunicationChannel)notification.Channel == Entities.CommunicationChannel.Sms)
					{
						CreateSmsNotifications(context, crmHelper, notification, caseItem);
					}
					else
					{
						throw new InvalidPluginExecutionException(string.Format("Invalid notification communication channel specified: [{0}]", notification.Channel));
					}
				}
			}
		}

		private void CreateMessageBody(ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem, ref string subject, ref string body)
		{
			//	TODO: How to include the Note text into a notification??
			if (!string.IsNullOrEmpty(notification.EmailTemplateId))
			{
				var itr = new InstantiateTemplateRequest
				{
					TemplateId = new Guid(notification.EmailTemplateId),
					ObjectId = caseItem.Id,
					ObjectType = Entities.Case.LogicalName,
				};

				var response = crmHelper.Execute<InstantiateTemplateRequest, InstantiateTemplateResponse>(itr);

				if (response.EntityCollection.Entities != null
					&& response.EntityCollection.Entities.Count == 1
					&& response.EntityCollection.Entities[0].LogicalName == "email")
				{
					body = response.EntityCollection.Entities[0].Attributes["description"].ToString();
					subject = response.EntityCollection.Entities[0].Attributes["subject"].ToString();
				}
			}
			else
			{
				body = notification.Message;
				subject = notification.Subject;
			}
		}

		private void CreateSmsNotifications(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem)
		{
			//	Retrieve users
			CreateSmsForUsers(context, crmHelper, notification, caseItem);

			//	Retrieve contacts
			CreateSmsforContact(context, crmHelper, notification, caseItem);

			if (notification.SendtoOwner)
			{
				var owner = crmHelper.GetEntity<Entities.User>(caseItem.OwnerId.ReferencedEntityId);
				if (!string.IsNullOrEmpty(owner.MobilePhone))
				{
					var sms = CreateSms(crmHelper, notification, caseItem, owner.MobilePhone);
					Send(context, crmHelper, sms);
				}
			}

			if (notification.SendtoRequestor)
			{
				string mobileNo = string.Empty;
				var contactId = CrmEntityReference.IsValid(caseItem.ResponsibleContactId) ? caseItem.ResponsibleContactId.ReferencedEntityId
					: CrmEntityReference.IsValid(caseItem.ContactId) ? caseItem.ContactId.ReferencedEntityId : Guid.Empty;
				if (contactId != Guid.Empty)
				{
					var contact = crmHelper.GetEntity<Entities.Contact>(contactId);
					if (contact != null && !string.IsNullOrEmpty(contact.MobilePhone))
						mobileNo = contact.MobilePhone;
					else if (caseItem.AccountId != null && caseItem.AccountId.ReferencedEntityId != Guid.Empty)
					{
						var account = crmHelper.GetEntity<Entities.Account>(caseItem.AccountId.ReferencedEntityId);
						if (account != null && account.Attributes.ContainsKey("MobilePhone") && !string.IsNullOrEmpty(account.Attributes["MobilePhone"] as string))
							mobileNo = account.Attributes["MobilePhone"].ToString();
					}
				}
				if (!string.IsNullOrEmpty(mobileNo))
				{
					var sms = CreateSms(crmHelper, notification, caseItem, mobileNo);
					Send(context, crmHelper, sms);
				}
			}
		}

		private void CreateSmsforContact(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem)
		{
			var contacts = RetrieveContacts(context, crmHelper, notification);
			foreach (var contact in contacts)
			{
				if (string.IsNullOrEmpty(contact.MobilePhone))
					continue;

				var sms = CreateSms(crmHelper, notification, caseItem, contact.MobilePhone);
				Send(context, crmHelper, sms);
			}
		}

		private void CreateSmsForUsers(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem)
		{
			var users = RetrieveUsers(context, crmHelper, notification);
			foreach (var user in users)
			{
				if (string.IsNullOrEmpty(user.MobilePhone))
					continue;

				var sms = CreateSms(crmHelper, notification, caseItem, user.MobilePhone);
				Send(context, crmHelper, sms);
			}
		}

		private Entities.Sms CreateSms(ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem, string mobileNo)
		{
			string message = string.Empty, subject = string.Empty;
			CreateMessageBody(crmHelper, notification, caseItem, ref subject, ref message);

			var sms = new Entities.Sms
			{
				Direction = true,
				DueDate = DateTime.Now,
				MobilePhone = mobileNo,
				Subject = subject,
				Message = Azuro.Common.Formatting.HtmlFormatter.StripHtml(message),
				Status = Entities.SmsStatus.Send,
				ActivityStatus = Entities.SmsActivityStatus.Open,
				StatusReason = Entities.SmsStatusReason.Open,
				Provider = Entities.SmsProvider.Clickatell, //	HACK: Hard-coded for Expediency
			};

			//CrmHelper.Insert(sms);

			//sms.Status = Entities.SmsStatus.Send;
			//sms.StatusReason = Entities.SmsStatusReason.Open;

			//CrmHelper.SetStatus(sms);

			return sms;
		}

		private void CreateEmailNotifications(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem)
		{
			//	TODO: Create entity references in generated entities
			//	Retrieve users
			CreateEmailForUsers(context, crmHelper, notification, caseItem);

			//	Retrieve queues
			CreateEmailForQueues(context, crmHelper, notification, caseItem);

			//	Retrieve contacts
			CreateEmailForContacts(context, crmHelper, notification, caseItem);

			//	Get owner and requestor -- is this kept on the case?
			if (notification.SendtoOwner)
			{
				var target = CreateActivityPartyList(new List<Entities.User> { crmHelper.GetEntity<Entities.User>(caseItem.OwnerId.ReferencedEntityId), });
				var email = CreateEmail(context, crmHelper, notification, caseItem, target);
				//Trace("Sending Owner Email to {0} - {1}", target[0].Attributes["systemuserid"], email.to[0].Attributes["FullName"]);
				//email.Subject += " - Owner";
				Send(context, crmHelper, email);
			}

			//	TODO: Wherever account is used, find primary contact id on the account.
			if (notification.SendtoRequestor)
			{
				List<CrmEntity> target = null;
				if (CrmEntityReference.IsValid(caseItem.ResponsibleContactId))
					target = CreateActivityPartyList(new List<Entities.Contact> { crmHelper.GetEntity<Entities.Contact>(caseItem.ResponsibleContactId.ReferencedEntityId), });
				else if (CrmEntityReference.IsValid(caseItem.CustomerId))
				{
					if (caseItem.CustomerId.EntityName == "contact")
						target = CreateActivityPartyList(new List<Entities.Contact> { crmHelper.GetEntity<Entities.Contact>(caseItem.CustomerId.ReferencedEntityId), });
					else if (caseItem.CustomerId.EntityName == "account")
						target = CreateActivityPartyList(new List<Entities.Account> { crmHelper.GetEntity<Entities.Account>(caseItem.CustomerId.ReferencedEntityId), });
				}
				else if (CrmEntityReference.IsValid(caseItem.ContactId))
					target = CreateActivityPartyList(new List<Entities.Contact> { crmHelper.GetEntity<Entities.Contact>(caseItem.ContactId.ReferencedEntityId), });
				else if (CrmEntityReference.IsValid(caseItem.AccountId))
					target = CreateActivityPartyList(new List<Entities.Account> { crmHelper.GetEntity<Entities.Account>(caseItem.AccountId.ReferencedEntityId), });

				if (target != null)
				{
					var email = CreateEmail(context, crmHelper, notification, caseItem, target);
					//email.Subject += " - Requestor";
					Send(context, crmHelper, email);
				}
			}
		}

		private void CreateEmailForContacts(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem)
		{
			Trace(context, "Entering CreateEmailForContacts");
			var contacts = RetrieveContacts(context, crmHelper, notification);
			foreach (var contact in contacts)
			{
				var target = CreateActivityPartyList(new List<Entities.Contact> { crmHelper.GetEntity<Entities.Contact>(contact.Id), });
				var email = CreateEmail(context, crmHelper, notification, caseItem, target);
				Send(context, crmHelper, email);
			}
			Trace(context, "Leaving CreateEmailForContacts");
		}

		private void CreateEmailForQueues(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem)
		{
			var queues = crmHelper.GetEntityList<Entities.Queue>(new List<CrmQuery> 
																	{
																		new CrmQuery 
																		{
																			Key = "azuro_notificationcommunicationid", 
																			Value= notification.Id, 
																			Operator= FilterOperator.Equal, 
																			FilterType= FilterType.And
																		}
																	},
																	new CrmLinkReference
																	{
																		LinkName = "azuro_azuro_notificationcommunication_queue",
																		FromAttribute = "queueid",
																		ToAttribute = "queueid"
																	});

			foreach (var queue in queues)
			{
				var target = CreateActivityPartyList(new List<Entities.Queue> { crmHelper.GetEntity<Entities.Queue>(queue.Id), });
				var email = CreateEmail(context, crmHelper, notification, caseItem, target);
			}
		}

		private void CreateEmailForUsers(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem)
		{
			var users = RetrieveUsers(context, crmHelper, notification);
			foreach (var user in users)
			{
				var target = CreateActivityPartyList(new List<Entities.User> { crmHelper.GetEntity<Entities.User>(user.Id), });
				var email = CreateEmail(context, crmHelper, notification, caseItem, target);
				Send(context, crmHelper, email);
			}
		}

		private Entities.Email CreateEmail(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Entities.Case caseItem, List<CrmEntity> target)
		{
			var email = new Entities.Email();

			if (notification.EmailTemplateId != null)
			{
				var itr = new InstantiateTemplateRequest
				{
					TemplateId = new Guid(notification.EmailTemplateId),
					ObjectId = caseItem.Id,
					ObjectType = Entities.Case.LogicalName,
				};

				var response = (InstantiateTemplateResponse)crmHelper.Execute<InstantiateTemplateRequest, InstantiateTemplateResponse>(itr);

				if (response.EntityCollection.Entities != null
					&& response.EntityCollection.Entities.Count == 1
					&& response.EntityCollection.Entities[0].LogicalName == "email")
				{
					email.Description = response.EntityCollection.Entities[0].Attributes["description"].ToString();
					email.Subject = response.EntityCollection.Entities[0].Attributes["subject"].ToString();
				}
			}
			else
			{
				email.Description = notification.Message;
				email.Subject = notification.Subject;
			}

			if (notification.SendfromOwner)
				email.from = CreateActivityPartyList(new List<Entities.User> { crmHelper.GetEntity<Entities.User>(caseItem.OwnerId.ReferencedEntityId), });
			else
				email.from = CreateActivityPartyList(new List<CrmEntityReference> { notification.SendFrom, });

			email.to = target;
			email.RegardingObjectId = new CrmEntityReference(Entities.Case.LogicalName, caseItem.Title, caseItem.Id);
			email.OwnerId = caseItem.OwnerId;
			email.DirectionCode = true;
			email.DeliveryAttempts = 0;
			//email.Sender = ToString();

			crmHelper.Insert(email);

			return email;
		}

		private void Send(CodeActivityContext context, ICrmHelper crmHelper, Entities.Email email)
		{
			var sendEmail = new SendEmailRequest
			{
				EmailId = email.Id,
				TrackingToken = string.IsNullOrEmpty(email.TrackingToken) ? string.Empty : email.TrackingToken,
				IssueSend = true,
			};

			var sendEmailresp = crmHelper.Execute<SendEmailRequest, SendEmailResponse>(sendEmail);
		}

		private void Send(CodeActivityContext context, ICrmHelper crmHelper, Entities.Sms sms)
		{
			crmHelper.Insert(sms);
		}

		private List<Entities.User> RetrieveUsers(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification)
		{
			var users = crmHelper.GetEntityList<Entities.User>(new List<CrmQuery> 
																	{
																		new CrmQuery 
																		{
																			Key = "azuro_notificationcommunicationid", 
																			Value= notification.Id, 
																			Operator= FilterOperator.Equal, 
																			FilterType= FilterType.And
																		}
																	},
																	new CrmLinkReference
																	{
																		LinkName = "azuro_azuro_notificationcommunication_systemu",
																		FromAttribute = "systemuserid",
																		ToAttribute = "systemuserid"
																	});
			return users;
		}

		private List<Entities.Contact> RetrieveContacts(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification)
		{
			var contacts = crmHelper.GetEntityList<Entities.Contact>(new List<CrmQuery> 
																	{
																		new CrmQuery 
																		{
																			Key = "azuro_notificationcommunicationid", 
																			Value= notification.Id, 
																			Operator= FilterOperator.Equal, 
																			FilterType= FilterType.And
																		}
																	},
																	new CrmLinkReference
																	{
																		LinkName = "azuro_azuro_notificationcommunication_contact",
																		FromAttribute = "contactid",
																		ToAttribute = "contactid"
																	});
			return contacts;
		}

		private List<CrmEntity> CreateActivityPartyList(List<Entities.User> users)
		{
			return users.ConvertAll<CrmEntity>(a => CreateActivityParty(a));
		}

		private Entities.ActivityParty CreateActivityParty(Entities.User user)
		{
			return new Entities.ActivityParty
			{
				PartyId = new CrmEntityReference(Entities.User.LogicalName, user.FullName, user.Id),
			};
		}

		private List<CrmEntity> CreateActivityPartyList(List<Entities.Contact> contacts)
		{
			return contacts.ConvertAll<CrmEntity>(a => CreateActivityParty(a));
		}

		private Entities.ActivityParty CreateActivityParty(Entities.Contact contact)
		{
			return new Entities.ActivityParty
			{
				PartyId = new CrmEntityReference(Entities.Contact.LogicalName, contact.FullName, contact.Id),
			};
		}

		private List<CrmEntity> CreateActivityPartyList(List<Entities.Account> accounts)
		{
			return accounts.ConvertAll<CrmEntity>(a => CreateActivityParty(a));
		}

		private Entities.ActivityParty CreateActivityParty(Entities.Account account)
		{
			return new Entities.ActivityParty
			{
				PartyId = new CrmEntityReference(Entities.Account.LogicalName, account.Name, account.Id),
			};
		}

		private List<CrmEntity> CreateActivityPartyList(List<Entities.Queue> queues)
		{
			return queues.ConvertAll<CrmEntity>(a => CreateActivityParty(a));
		}

		private Entities.ActivityParty CreateActivityParty(Entities.Queue queue)
		{
			return new Entities.ActivityParty
			{
				PartyId = new CrmEntityReference(Entities.Queue.LogicalName, queue.Name, queue.Id),
			};
		}

		private List<CrmEntity> CreateActivityPartyList(List<CrmEntityReference> entities)
		{
			return entities.ConvertAll<CrmEntity>(a => CreateActivityParty(a));
		}

		private Entities.ActivityParty CreateActivityParty(CrmEntityReference reference)
		{
			return new Entities.ActivityParty
			{
				PartyId = reference,
			};
		}

		private void CreateEmailforAccount(CodeActivityContext context, ICrmHelper crmHelper, Entities.NotificationCommunication notification, Guid itemId, Entities.Case caseItem)
		{
			var email = new Entities.Email();

			//email.Description = channel.Message;
			//email.To = itemId.ToString();
			//email.From = caseItem.OwnerId.ToString();

			//email.RegardingId = caseItem.Id;
			//email.Subject = channel.Subject;
			//email.UserId = itemId;
			//email.OwnerId = caseItem.OwnerId;

			//email.FromType = "systemuser";
			//email.ToType = "account";

			crmHelper.Insert(email);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void TestNotifications(Guid organizationId, Guid caseId, Azuro.Crm.Entities.NotificationEventType eventType)
		{
			//_crmHelper = CrmHelperFactory.Create(organizationId);
			//ProcessNotifications(caseId, eventType);
		}
	}
}
