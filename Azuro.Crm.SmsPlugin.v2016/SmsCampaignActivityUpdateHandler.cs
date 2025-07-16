using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Azuro.Crm.Entities;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;

namespace Azuro.Crm.Plugin
{
	public class SmsCampaignActivityUpdateHandler : Azuro.Crm.Integration.APlugin
	{
		private Dictionary<string, azuro_sms> _smses = new Dictionary<string, azuro_sms>();
		private azuro_smsconfiguration SmsConfiguration { get; set; }

		private azuro_smsconfiguration RetrieveSmsConfiguration(IServiceProvider serviceProvider, IOrganizationService service)
		{
			azuro_smsconfiguration cfg = null;
			LogTrace(serviceProvider, "Retrieving Configuration");
			var cfglist = GetEntityList<azuro_smsconfiguration>(service);
			if (cfglist.Count == 1)
			{
				LogTrace(serviceProvider, "configuration entity found");
				cfg = cfglist[0];
			}
			else if (cfglist.Count > 1)
			{
				LogTrace(serviceProvider, "There can only be one configuration entity");
			}

			LogTrace(serviceProvider, "Configuration - {0}", cfg != null ? "Obtained" : "Not Loaded");
			return cfg;
		}

		protected override void Execute(IServiceProvider serviceProvider, IOrganizationService service, IPluginExecutionContext context)
		{
			//	Get the campaign entity
			LogTrace(serviceProvider, "{0} - Plugin has started.", this);

			var target = GetPrimaryEntity<azuro_smscampaignactivity>(serviceProvider, "azuro_smscampaignactivity");
			var campaignEntity = GetEntity<azuro_smscampaignactivity>(service, target.Id);
			SmsConfiguration = RetrieveSmsConfiguration(serviceProvider, service);

			DoUpdateExecution(serviceProvider, service, campaignEntity);
		}

		public void DoUpdateExecution(IServiceProvider serviceProvider, IOrganizationService service, azuro_smscampaignactivity campaign)
		{
			LogTrace(serviceProvider, "Checking Sms Campaign Status [{0}]", campaign.azuro_CampaignStatusReasonEnum);

			if (campaign.azuro_CampaignStatusReasonEnum != azuro_CampaignStatusReason.Pending)
			{
				LogTrace(serviceProvider, "Sms Campaign is not in a Pending status.");
				return;
			}

			//	Get the associated marketing lists
			LogTrace(serviceProvider, "{0} - Retrieving the Marketing List", this);

			var fetch = @"<fetch mapping=""logical"">
							<entity name='list'>
								<all-attributes/>
								<filter type='and'>
									<condition attribute='azuro_smscampaignid' operator='eq' value='" + campaign.Id + @"' />
								</filter> 
							</entity> 
						</fetch>";

			var fetchRequest = new RetrieveMultipleRequest
			{
				Query = new FetchExpression(fetch)
			};

			var orgSvc = GetOrganizationService(serviceProvider);
			var result = (Execute<RetrieveMultipleResponse>(orgSvc, fetchRequest)).EntityCollection;
			if (result.Entities.Count < 1)
			{
				LogTrace(serviceProvider, "There are no marketing lists associated with the SMS Campaign: {0}", campaign.Id);
				return;
			}

			//	Get the list members
			foreach (var list in result.Entities)
			{
				if ((List_CreatedFromCode)list.Attributes["membertype"] == List_CreatedFromCode.Account)
				{
					//	TODO: Add Account support
					LogTrace(serviceProvider, "Accounts are not supported.");
					continue;
				}

				bool isDynamicList = (bool)list.Attributes["type"];


				ListMemberTypeInfo memberTypeInfo = null;
				switch ((List_CreatedFromCode)list.Attributes["membertype"])
				{
					case List_CreatedFromCode.Account:
						memberTypeInfo = new ListMemberTypeInfo { Code = List_CreatedFromCode.Account, MemberType = Account.EntityLogicalName, AttributeName = "accountid", };
						break;
					case List_CreatedFromCode.Contact:
						memberTypeInfo = new ListMemberTypeInfo { Code = List_CreatedFromCode.Contact, MemberType = Contact.EntityLogicalName, AttributeName = "contactid", };
						break;
					case List_CreatedFromCode.Lead:
						memberTypeInfo = new ListMemberTypeInfo { Code = List_CreatedFromCode.Lead, MemberType = Lead.EntityLogicalName, AttributeName = "leadid", };
						break;
				}

				if (isDynamicList)
				{
					ProcessDynamicList(serviceProvider, service, memberTypeInfo, list, campaign);
				}
				else
				{
					ProcessStaticList(serviceProvider, service, memberTypeInfo, list, campaign);
				}
			}

			//	Create the parent relationship between Sms and CampaignActivity
			var relatedSmses = new EntityReferenceCollection();
			LogTrace(serviceProvider, "There are {0} smses in the Dictionary", _smses.Count);
			foreach (var sms in _smses)
			{
				LogTrace(serviceProvider, "Adding SMS relationship for [{0}]", sms.Value.Id);
				relatedSmses.Add(new EntityReference(azuro_sms.EntityLogicalName, sms.Value.Id));
			}

			Relationship smsCampaignActivityRelationship = new Relationship("azuro_azuro_smscampaignactivity_azuro_sms");

			orgSvc.Associate(azuro_smscampaignactivity.EntityLogicalName, campaign.Id, smsCampaignActivityRelationship, relatedSmses);

			//	Update Campaign Entity, set azuro_campaignstatusreason to Executed
			campaign.azuro_CampaignStatusReasonEnum = azuro_CampaignStatusReason.Executed;
			orgSvc.Update(campaign);
		}

		private void ProcessStaticList(IServiceProvider serviceProvider, IOrganizationService service, ListMemberTypeInfo memberTypeInfo, Entity list, azuro_smscampaignactivity campaignEntity)
		{
			LogTrace(serviceProvider, "Processing static list {0} with {1} members.", list.Id, list.Attributes["membercount"]);

			var fetchlistmembers = new QueryExpression()
			{
				EntityName = memberTypeInfo.MemberType,
				ColumnSet = new ColumnSet(true),
				LinkEntities =
						{
							new LinkEntity
							{
								LinkFromEntityName = memberTypeInfo.MemberType,
								LinkFromAttributeName = memberTypeInfo.AttributeName,
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
											Values = { list.Id }
										}
									}
								}
							}
						}
			};

			var orgSvc = GetOrganizationService(serviceProvider);
			var fetchmembersresult = orgSvc.RetrieveMultiple(fetchlistmembers);
			if (fetchmembersresult.Entities.Count < 1)
			{
				LogTrace(serviceProvider, "The marketing list {0} has no members.", list.Id);
				return;
			}

			ProcessListMembers(serviceProvider, service, memberTypeInfo, fetchmembersresult.Entities, campaignEntity);
		}

		private void ProcessDynamicList(IServiceProvider serviceProvider, IOrganizationService service, ListMemberTypeInfo memberTypeInfo, Entity list, azuro_smscampaignactivity campaignEntity)
		{
			LogTrace(serviceProvider, "Processing dynamic list {0}.", list.Id);

			var fetchmembersresult = GetOrganizationService(serviceProvider).RetrieveMultiple(new FetchExpression(list.Attributes["query"] as string));
			if (fetchmembersresult.Entities.Count < 1)
			{
				LogTrace(serviceProvider, "The marketing list {0} has no members.", list.Id);
				return;
			}

			ProcessListMembers(serviceProvider, service, memberTypeInfo, fetchmembersresult.Entities, campaignEntity);
		}

		private void ProcessListMembers(IServiceProvider serviceProvider, IOrganizationService service, ListMemberTypeInfo memberTypeInfo, DataCollection<Entity> listMembers, azuro_smscampaignactivity campaignEntity)
		{
			LogTrace(serviceProvider, "Processing the list members. There are [{0}] members.", listMembers.Count);
			string[] attributes = new string[] { "azuro_content", "mobilephone", "azuro_name" };

			foreach (var member in listMembers)
			{
#if DEBUG
				var ts = GetTracingService(serviceProvider);

				foreach (string s in attributes)
				{
					bool c = campaignEntity.Attributes.ContainsKey(s);
					bool m = member.Attributes.ContainsKey(s);
					ts.Trace("{0}:{1} - [{2}]-[{3}]", s, m ? "m" : c ? "c" : "0", m | c,
						c ? campaignEntity.Attributes[s] : m ? member.Attributes[s] : "null");
				}
				foreach (var i in member.Attributes)
				{
					ts.Trace("Member Attributes: {0} - {1}", i.Key, i.Value);
				}

				foreach (var i in campaignEntity.Attributes)
				{
					ts.Trace("Campaign Attributes: {0} - {1}", i.Key, i.Value);
				}
#endif
				LogTrace(serviceProvider, "Retrieving Entity [{0}] with Attribute: [{1}]", memberTypeInfo.MemberType, memberTypeInfo.AttributeName);

				var orgSvc = GetOrganizationService(serviceProvider);
				var entity = orgSvc.Retrieve(memberTypeInfo.MemberType, (Guid)member.Attributes[memberTypeInfo.AttributeName], new ColumnSet(true));

				foreach (var i in entity.Attributes)
				{
					LogTrace(serviceProvider, "Entity Attributes: {0} - {1}", i.Key, i.Value);
				}

				var sms = new azuro_sms
				{
					azuro_Direction = true,
					ScheduledStart = DateTime.Now,
					ScheduledEnd = DateTime.Now,
					azuro_Message = campaignEntity.azuro_Content,
					azuro_MobilePhone = FormatMobilePhone(service, member),
					azuro_StatusEnum = azuro_sms_azuro_Status.Send,
					Subject = campaignEntity.azuro_name,
				};

				CreateSms(serviceProvider, service, sms, campaignEntity);

				LogTrace(serviceProvider, "Create the relationship between the Sms and the {0}.", member.LogicalName);

				//	Create the relationship between Sms and Contact / Lead
				var relatedSms = new EntityReferenceCollection();
				relatedSms.Add(new EntityReference(azuro_sms.EntityLogicalName, sms.Id));

				Relationship relationship = new Relationship("azuro_" + member.LogicalName + "_azuro_sms");

				orgSvc.Associate(member.LogicalName, member.Id, relationship, relatedSms);

				LogTrace(serviceProvider, "Relationship between the Sms {0} and the {0}:{1} created.", sms.Id, member.LogicalName, member.Id);
			}
		}

		private string FormatMobilePhone(IOrganizationService service, Entity member)
		{
			string mobilePhone = string.Empty;
			if (SmsConfiguration != null)
			{
				switch (member.LogicalName)
				{
					case "contact":
						if (!string.IsNullOrEmpty(SmsConfiguration.azuro_ContactInternationalDialcode))
						{
							mobilePhone = string.Format("{0}{1}", GetDialCode(service, member, SmsConfiguration.azuro_ContactInternationalDialcode), (member.Attributes["mobilephone"] as string).TrimStart('0'));
						}
						break;
					case "account":
						if (!string.IsNullOrEmpty(SmsConfiguration.azuro_AccountInternationalDialcode))
						{
							mobilePhone = string.Format("{0}{1}", GetDialCode(service, member, SmsConfiguration.azuro_AccountInternationalDialcode), (member.Attributes["azuro_mobilephone"] as string).TrimStart('0'));
						}
						break;
					case "lead":
						if (!string.IsNullOrEmpty(SmsConfiguration.azuro_LeadInternationalDialcode))
						{
							mobilePhone = string.Format("{0}{1}", GetDialCode(service, member, SmsConfiguration.azuro_LeadInternationalDialcode), (member.Attributes["mobilephone"] as string).TrimStart('0'));
						}
						break;
				}
			}
			return mobilePhone;
		}

		private string GetDialCode(IOrganizationService service, Entity entity, string fieldname)
		{
			if (entity.Attributes.ContainsKey(fieldname))
			{
				var mobilefield = entity[fieldname];
				if (mobilefield is OptionSetValue)
				{
					return GetOptionSetValueText(service, entity.LogicalName, fieldname, ((OptionSetValue)mobilefield).Value);
				}
				else if (mobilefield is string)
					return mobilefield as string;
			}

			return string.Empty;
		}

		private void CreateSms(IServiceProvider serviceProvider, IOrganizationService service, azuro_sms sms, Entity campaignEntity)
		{
			LogTrace(serviceProvider, "Creating Sms to {0}.", sms.azuro_MobilePhone);
			if (_smses.ContainsKey(sms.azuro_MobilePhone))
			{
				LogTrace(serviceProvider, "Number previously added to Sms List: {0}.", sms.azuro_MobilePhone);
				return;
			}
			else
				_smses.Add(sms.azuro_MobilePhone, sms);

			sms.Id = service.Create(sms);

			LogTrace(serviceProvider, "Assign the Sms to Owner to {0}.", ((EntityReference)campaignEntity.Attributes["owninguser"]).Id);

			//	Set the Owner on the Sms
			var assign = new AssignRequest
			{
				Assignee = new EntityReference(SystemUser.EntityLogicalName, ((EntityReference)campaignEntity.Attributes["owninguser"]).Id),
				Target = new EntityReference(azuro_sms.EntityLogicalName, sms.Id)
			};

			GetOrganizationService(serviceProvider).Execute(assign);
		}
	}
}
