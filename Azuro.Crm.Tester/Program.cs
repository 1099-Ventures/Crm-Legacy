using System;
using Azuro.Crm.Entities;
using Azuro.Crm.Integration;
using Azuro.Crm.Plugin;
using Azuro.MSMQ;
using Azuro.Sms.Clickatell;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;
using Azuro.Crm.Workflow;
using Azuro.Crm.Workflow.SupportDeskNotifications;
using System.Net;
using System.IO;
using System.Web;
using Azuro.Crm.SmsData;
using System.Configuration;

namespace Azuro.Crm.Tester
{
	class Program
	{
		private static Guid OrganizationId
		{
			get { return Guid.Parse(ConfigurationManager.AppSettings["OrganizationId"]); }
		}

		private static string SmsTriggerQueueName
		{
			get { return ConfigurationManager.AppSettings["SmsTriggerQueueName"]; }
		}

		private static CrmHelper CreateCrmHelper()
		{
			CrmHelper helper = new CrmHelper();
			helper.OrganisationId = OrganizationId;
			return helper;
		}

		static void Main(string[] args)
		{
			ConsoleKeyInfo cki;
			do
			{
				Console.Clear();
				Console.WriteLine("Tester Menu:");
				Console.WriteLine("1 - Send SMS Activity");
				Console.WriteLine("2 - Test UpdateHandler");
				Console.WriteLine("3 - Insert SMSTriggerEvent");
				Console.WriteLine("4 - Test Retrieve Marketing List Items");
				Console.WriteLine("5 - Test Update SMS Activity Status");
				Console.WriteLine("6 - Test Mobile Dialcode");
				Console.WriteLine("7 - Test EmailToCase Workflow Activity");
				Console.WriteLine("8 - Test Create Email Template");
				Console.WriteLine("9 - Load Email");
				Console.WriteLine("0 - Send Notifications");
				Console.WriteLine("a - Test Escalations");
				Console.WriteLine("b - Test Get Entity");
				Console.WriteLine("c - Test N-Able Notifications");
				Console.WriteLine("d - Test N-Able Acknowledgement");
				Console.WriteLine("e - Test Saving SMS Send Log");
				Console.WriteLine("x - Exit");

				cki = Console.ReadKey();
				Console.WriteLine();
				bool validSelection = true;
				switch (cki.KeyChar)
				{
					case '1':
						TestSendSmsActivity();
						break;
					case '2':
						TestUpdateHandler();
						break;
					case '3':
						TestInsertSmsTriggerEvent();
						break;
					case '4':
						TestRetrieveMarketingList();
						break;
					case '5':
						TestUpdateSmsActivityStatus();
						break;
					case '6':
						TestGetMobileDialCode();
						break;
					case '7':
						TestEmailToCase();
						break;
					case '8':
						CreateEmailTemplate();
						break;
					case '9':
						LoadEmail();
						break;
					case '0':
						SendSupportNotifications();
						break;
					case 'a':
					case 'A':
						TestSupportEscalations();
						break;
					case 'b':
					case 'B':
						TestGetEntity();
						break;
					case 'c':
					case 'C':
						TestNAbleMessagePost();
						break;
					case 'd':
					case 'D':
						TestNableAcknowledgement();
						break;
					case 'e':
					case 'E':
						SmsSendLog sms = null;
						Console.WriteLine("Sms Send Log Created with ID: {0}", TestCreateSmsSendLog(ref sms));
						Console.WriteLine("Sms Send Log still valid: {0}", sms.ID);
						break;
					default:
						validSelection = false;
						break;
				}
				if (validSelection)
				{
					Console.Write("Press any key to continue, or 'x' to exit.");
					cki = Console.ReadKey();
				}
			} while (cki.KeyChar != 'x');
		}

		private static long TestCreateSmsSendLog(ref SmsSendLog sms)
		{
			sms = new SmsSendLog
			{
				ProviderId = "1212121212",
				Provider = "Clickatell",
				Message = "Sent",
				MobileNumber = "0823715679",
				OrganizationId = Guid.NewGuid(),
				ProviderStatus = "Sending",
				ProviderStatusMessage = "Sending",
				ActivityId = Guid.NewGuid(),
				DateSent = DateTime.Now,
				DateCreated = DateTime.Now,
				DateDelivered = DateTime.Now,
				DateChanged = DateTime.Now,
			};

			using (var ctx = new AzuroSMSEntities())
			{
				ctx.SmsSendLogs.Add(sms);
				ctx.SaveChanges();
			}

			return sms.ID;
		}

		private static void TestNableAcknowledgement()
		{
			Integration.Nable.CaseResolvedMessageHandler crmh = new Integration.Nable.CaseResolvedMessageHandler();
			crmh.ProcessMessage(new Integration.Nable.Entities.CaseResolvedNotification { ResolutionDescription = "blah", CaseStatus = "Resolved", ExternalReference = 12345 });
		}

		private static void TestNAbleMessagePost()
		{
			using (var wc = new WebClient())
			{
				//using (var sw = new StreamWriter(wc.OpenWrite("http://localhost:57071/NotificationProcessor.aspx", "POST")))
				//using (var sw = new StreamWriter(wc.OpenWrite("http://localhost/NAbleNotification/NotificationProcessor.aspx", "POST")))
				using (var sw = new StreamWriter(wc.OpenWrite("http://conclave.azurodev.local:8080/NotificationProcessor.aspx", "POST")))
				{
					var rng = new Random();
					var notification = new Integration.Nable.Entities.Notification
					{
						ActiveNotificationTriggerID = rng.Next().ToString(),
						AffectedService = "Disk",
						CustomerName = "Azuro",
						DeviceName = "Device",
						DeviceProperty = new List<Integration.Nable.Entities.KeywordWithParameterNode>() { new Integration.Nable.Entities.KeywordWithParameterNode { parameter = "Location", Value = "SA" } },
						DeviceURI = "192.168.0.1",
						NcentralURI = "mso.triple4cloud.com",
						ProbeURI = "192.168.0.1",
						QualitativeNewState = "Failed",
						QualitativeOldState = "Warning",
						QuantitativeNewState = HttpUtility.UrlEncode("Total disk size: 5119996\nDisk space used:340 blah blah"),
						TaskIdent = "C:",
						TimeOfStateChange = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
						ExternalCustomerID = "Azuro",
					};

					Console.WriteLine(notification.ToString());

					sw.Write(notification.ToString());
					sw.Flush();
					sw.Close();
				}
			}
		}

		private static void TestGetEntity()
		{
			var h = CrmHelperFactory.Create(new Guid());
			var e = h.GetEntity("", "", "");
			System.Diagnostics.Debug.Assert(e == null);
		}

		private static void TestSupportEscalations()
		{
			var activity = new EscalationActivity();
			activity.TestEscalations(OrganizationId, new Guid("7928EEF5-5988-E311-A439-00155D004301"));
		}

		private static void SendSupportNotifications()
		{
			var notification = new NotificationActivity();
			notification.TestNotifications(OrganizationId, new Guid("A1544A9A-C134-E311-B38E-00155D004301"), NotificationEventType.Creation);
		}

		private static void LoadEmail()
		{
			var helper = CreateCrmHelper();

			var email = helper.GetEntity<Entities.Email>(new Guid("F1360988-468A-E211-8243-00155D004301"));

			Console.WriteLine("Email found: {0}", email.Subject);
		}

		private static void CreateEmailTemplate()
		{
			var helper = CreateCrmHelper();

			var itr = new InstantiateTemplateRequest
			{
				TemplateId = new Guid("D0B742B5-E945-4073-981C-11C0BEB4E2B1"),
				ObjectId = new Guid("9C2F182F-D966-E211-AB0D-00155D004301"),
				ObjectType = Case.LogicalName,
			};
			var response = (InstantiateTemplateResponse)helper.OrganizationService.Execute(itr);

			if (response.EntityCollection.Entities != null
				&& response.EntityCollection.Entities.Count == 1
				&& response.EntityCollection.Entities[0].LogicalName == "email")
			{
				Console.WriteLine(response.EntityCollection.Entities[0].Attributes["description"]);
			}
		}

		private static void TestEmailToCase()
		{
			QueueEmailToCaseActivity activity = new QueueEmailToCaseActivity();
			activity.TestActivity(OrganizationId, new Guid("F4FAAE76-8632-E311-B38E-00155D004301"));
		}

		private static void TestGetMobileDialCode()
		{
			CrmHelper helper = CreateCrmHelper();
			var cfg = RetrieveSmsConfiguration(helper);

			var fetch = @"<fetch mapping=""logical"">
							<entity name='" + Contact.EntityLogicalName + @"'>
								<all-attributes/>
								<filter type='and'>  
									<condition attribute='contactid' operator='eq' value='6675FF22-97A6-E111-8B21-00155D80010D' />
								</filter> 
							</entity> 
						</fetch>";

			// Excute the fetch query and get the xml result.
			var result = helper.OrganizationService.RetrieveMultiple(new FetchExpression(fetch));
			var contact = result.Entities[0];

			if (contact.Attributes.ContainsKey(cfg.ContactInternationalDialCode))
			{
				var mobilefield = contact[cfg.ContactInternationalDialCode];
				if (mobilefield is OptionSetValue)
					Console.WriteLine(helper.GetOptionSetValueText<Entities.Contact>(cfg.ContactInternationalDialCode, ((OptionSetValue)mobilefield).Value));
			}
		}

		private static Entities.SmsConfiguration RetrieveSmsConfiguration(ICrmHelper helper)
		{
			Console.WriteLine("Retrieve Sms Configuration Object");

			SmsConfiguration cfg = null;
			var cfglist = helper.GetEntityList<SmsConfiguration>();
			if (cfglist.Count == 1)
			{
				Console.WriteLine("configuration entity found");
				cfg = cfglist[0];
			}
			else if (cfglist.Count > 1)
			{
				Console.WriteLine("There can only be one configuration entity");
			}

			Console.WriteLine("Configuration - {0}", cfg != null ? "Obtained" : "Not Loaded");
			return cfg;
		}

		private static void TestUpdateSmsActivityStatus()
		{
			//	Retrieve sms entity
			CrmHelper helper = new CrmHelper();
			helper.OrganisationId = OrganizationId;

			//var sms = helper.GetEntity<Entities.Sms>(new Guid("40A1DC2B-3BB9-E111-A353-00155D80010D"));

			//sms.ActivityStatus = SmsActivityStatus.Completed;
			//sms.StatusReason = SmsStatusReason.Completed;
			//sms.Provider = SmsProvider.Clickatell;

			SetStateRequest setStateRequest = new SetStateRequest();

			setStateRequest.EntityMoniker = new EntityReference(Entities.Sms.LogicalName, new Guid("40A1DC2B-3BB9-E111-A353-00155D80010D"));
			setStateRequest.State = new OptionSetValue((int)SmsActivityStatus.Scheduled);
			setStateRequest.Status = new OptionSetValue((int)SmsStatusReason.Scheduled);

			var response = helper.OrganizationService.Execute(setStateRequest);

			//helper.Update(sms);
		}

		private static void TestRetrieveMarketingList()
		{
			Guid campaignId = new Guid("1E379C9E-D4A3-E111-80B7-00155D80010D");

			CrmHelper helper = new CrmHelper();
			helper.OrganisationId = OrganizationId;

			var fetch = @"<fetch mapping=""logical"">
							<entity name='azuro_smscampaignactivity'>
								<all-attributes/>
								<filter type='and'>
									<condition attribute='azuro_smscampaignactivityid' operator='eq' value='" + campaignId + @"' />
								</filter> 
							</entity> 
						</fetch>";

			// Excute the fetch query and get the xml result.
			var fetchRequest = new RetrieveMultipleRequest
			{
				Query = new FetchExpression(fetch)
			};

			var result = ((RetrieveMultipleResponse)helper.OrganizationService.Execute(fetchRequest)).EntityCollection;
			if (result.Entities.Count != 1)
			{
				Console.WriteLine("Nothing found.");
				return;
			}

			var campaignEntity = result.Entities[0];

			//	Get the associated marketing list
			RetrieveRelationshipRequest rel = new RetrieveRelationshipRequest { Name = "azuro_azuro_smscampaignactivity_list" };
			RetrieveRelationshipResponse relResp = (RetrieveRelationshipResponse)helper.OrganizationService.Execute(rel);

			fetch = @"<fetch mapping=""logical"">
							<entity name='list'>
								<all-attributes/>
								<filter type='and'>
									<condition attribute='azuro_smscampaignid' operator='eq' value='" + campaignId + @"' />
								</filter> 
							</entity> 
						</fetch>";

			fetchRequest = new RetrieveMultipleRequest
			{
				Query = new FetchExpression(fetch)
			};

			result = ((RetrieveMultipleResponse)helper.OrganizationService.Execute(fetchRequest)).EntityCollection;
			if (result.Entities.Count < 1)
			{
				Console.WriteLine("No lists found.");
				return;
			}

			Dictionary<string, Entities.Sms> _smses = new Dictionary<string, Entities.Sms>();

			foreach (var entity in result.Entities)
			{
				bool isDynamicList = (bool)entity.Attributes["type"];
				if (isDynamicList)
				{
					var fetchlistmembers = new RetrieveMultipleRequest
					{
						Query = new FetchExpression(entity.Attributes["query"] as string)
					};

					var fetchmembersresult = ((RetrieveMultipleResponse)helper.OrganizationService.Execute(fetchlistmembers)).EntityCollection;
					if (fetchmembersresult.Entities.Count < 1)
					{
						Console.WriteLine("No lists members found.");
						return;
					}
				}
				else
				{
					Console.WriteLine("List is static, has {0} members, now what?", entity.Attributes["membercount"]);

					string entityname = null, attributename = null;
					bool skipIfAccount = false;
					switch ((ListCreatedFromCode)entity.Attributes["membertype"])
					{
						case ListCreatedFromCode.Account:
							entityname = Account.EntityLogicalName;
							attributename = "accountid";
							skipIfAccount = true;
							break;
						case ListCreatedFromCode.Contact:
							entityname = Contact.EntityLogicalName;
							attributename = "contactid";
							break;
						case ListCreatedFromCode.Lead:
							entityname = Lead.EntityLogicalName;
							attributename = "leadid";
							break;
					}

					if (skipIfAccount)
					{
						Console.WriteLine("Accounts are not supported at this time");
						continue;
					}

					var fetchlistmembers = new QueryExpression()
					{
						EntityName = entityname,
						ColumnSet = new ColumnSet(true),
						LinkEntities =
						{
							new LinkEntity
							{
								LinkFromEntityName = entityname,
								LinkFromAttributeName = attributename,
								LinkToEntityName = ListMember.EntityLogicalName,
								LinkToAttributeName = "entityid",
								LinkCriteria = new FilterExpression
								{
									FilterOperator = LogicalOperator.And,
									Conditions =
									{
										new ConditionExpression
										{
											AttributeName = "listid",
											Operator = ConditionOperator.Equal,
											Values = { entity.Id }
										}
									}
								}
							}
						}
					};

					var fetchmembersresult = helper.OrganizationService.RetrieveMultiple(fetchlistmembers);
					if (fetchmembersresult.Entities.Count < 1)
					{
						Console.WriteLine("No lists members found.");
						return;
					}

					foreach (var member in fetchmembersresult.Entities)
					{
						var sms = new Entities.Sms
						{
							Direction = true,
							DueDate = DateTime.Now,
							Message = campaignEntity.Attributes["azuro_content"] as string,
							MobilePhone = member.Attributes["mobilephone"] as string,
							Status = SmsStatus.Created,
							Subject = campaignEntity.Attributes["azuro_name"] as string,
						};

						helper.Insert(sms);

						var assign = new AssignRequest
						{
							Assignee = new EntityReference(SystemUser.EntityLogicalName, (Guid)campaignEntity.Attributes["owner"]),
							Target = new EntityReference(Entities.Sms.LogicalName, sms.Id)
						};

						helper.OrganizationService.Execute(assign);
					}
				}
			}

			//	Create the relationship between Sms and CampaignActivity
			var relatedSmses = new EntityReferenceCollection();
			foreach (var sms in _smses)
				relatedSmses.Add(new EntityReference(Entities.Sms.LogicalName, sms.Value.Id));

			Relationship smsCampaignActivityRelationship = new Relationship("azuro_smscampaignactivity_azuro_smses");

			helper.OrganizationService.Associate(SmsCampaignActivity.LogicalName, campaignEntity.Id, smsCampaignActivityRelationship, relatedSmses);

			//	Update Campaign Entity, set azuro_campaignstatusreason to Executed
			campaignEntity.Attributes["azuro_campaignstatusreason"] = (int)SmsCampaignStatusReason.Executed;
			var updateCampaignRequest = new UpdateRequest
			{
				Target = campaignEntity,
			};
			var updResult = helper.OrganizationService.Execute(updateCampaignRequest);
		}

		private static void TestInsertSmsTriggerEvent()
		{
			Console.Write("Enter Sms Guid: ");
			string guid = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(guid))
			{
				guid = Guid.NewGuid().ToString();
				Console.WriteLine("WARNING: Using a default Guid will most likely result in the SMS service crashing.");
			}
			Azuro.MSMQ.QueueHelper.Insert(SmsTriggerQueueName ?? @".\private$\SmsTriggerQueue", new SmsMessages.SmsTriggerEventMessage { OrganisationId = OrganizationId, ActivityId = new Guid(guid), });
		}

		static void TestSendSmsActivity()
		{
			SmsMessages.SmsTriggerEventMessage msg = SmsMessages.SmsTriggerEventMessage.Deserialize(string.Format("<SMSTriggerEvent campaignId=\"00000000-0000-0000-0000-000000000000\" activityId=\"A1511C88-42A6-E111-8B21-00155D80010D\" organisationId=\"{0}\" />", OrganizationId.ToString()));

			CrmHelper helper = new CrmHelper();

			helper.OrganisationId = msg.OrganisationId;

			var sms = helper.GetEntity<Azuro.Crm.Entities.Sms>(msg.ActivityId);

			ClickatellClient client = new ClickatellClient();
			var csms = client.SendMessage(new Sms.Common.SmsMessage { ClientId = sms.Id.ToString(), To = sms.MobilePhone, Message = sms.Message });

			sms.ProviderMessageId = csms.ProviderId;
			sms.Status = SmsStatus.Sent;
			sms.SentDate = DateTime.Now;

			helper.Update(sms);
		}

		static void TestUpdateHandler()
		{
			CrmHelper helper = new CrmHelper();
			helper.OrganisationId = OrganizationId;

			var onUpdateHandler = new SmsActivityCreateHandler();

			//	TODO: Build Plugin Testing Framework
			//onUpdateHandler.Execute(helper.OrganizationService);
		}
	}
}
