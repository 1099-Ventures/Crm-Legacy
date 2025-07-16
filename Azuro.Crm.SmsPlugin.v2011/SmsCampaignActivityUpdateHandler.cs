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
		private Dictionary<string, Sms> _smses = new Dictionary<string, Sms>();
		private SmsConfiguration SmsConfiguration { get; set; }

		private SmsConfiguration RetrieveSmsConfiguration(IServiceProvider serviceProvider)
		{
			SmsConfiguration cfg = null;
			GetTracingService(serviceProvider).Trace("Retrieving Configuration");
			var cfglist = GetCrmHelper(serviceProvider).GetEntityList<SmsConfiguration>();
			if (cfglist.Count == 1)
			{
				GetTracingService(serviceProvider).Trace("configuration entity found");
				cfg = cfglist[0];
			}
			else if (cfglist.Count > 1)
			{
				GetTracingService(serviceProvider).Trace("There can only be one configuration entity");
			}

			GetTracingService(serviceProvider).Trace("Configuration - {0}", cfg != null ? "Obtained" : "Not Loaded");
			return cfg;
		}

		public override void Execute(IServiceProvider serviceProvider)
		{
			//	Get the campaign entity
			GetTracingService(serviceProvider).Trace("{0} - Plugin has started.", this);

			var target = GetPrimaryEntity(serviceProvider, "azuro_smscampaignactivity");
			var campaignEntity = GetOrganizationService(serviceProvider).Retrieve(target.LogicalName, target.Id, new ColumnSet(true));
			SmsConfiguration = RetrieveSmsConfiguration(serviceProvider);

			DoUpdateExecution(serviceProvider, campaignEntity, campaignEntity.Id);
		}

		public void DoUpdateExecution(IServiceProvider serviceProvider, Entity campaignEntity, Guid campaignId)
		{
			GetTracingService(serviceProvider).Trace("Checking Sms Campaign Status [{0}]", campaignEntity.Attributes["azuro_campaignstatusreason"]);

			if ((SmsCampaignStatusReason)GetOptionSetValue(campaignEntity.Attributes["azuro_campaignstatusreason"]) != SmsCampaignStatusReason.Pending)
			{
				GetTracingService(serviceProvider).Trace("SmsCampaign is not in a Pending status.");
				return;
			}

			//	Get the associated marketing lists
			GetTracingService(serviceProvider).Trace("{0} - Retrieving the Marketing List", this);

			var fetch = @"<fetch mapping=""logical"">
							<entity name='list'>
								<all-attributes/>
								<filter type='and'>
									<condition attribute='azuro_smscampaignid' operator='eq' value='" + campaignId + @"' />
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
				GetTracingService(serviceProvider).Trace("There are no marketing lists associated with the SMS Campaign: {0}", campaignId);
				return;
			}

			//	Get the list members
			foreach (var list in result.Entities)
			{
				if ((ListCreatedFromCode)list.Attributes["membertype"] == ListCreatedFromCode.Account)
				{
					GetTracingService(serviceProvider).Trace("Accounts are not supported.");
					continue;
				}

				bool isDynamicList = (bool)list.Attributes["type"];


				ListMemberTypeInfo memberTypeInfo = null;
				switch ((ListCreatedFromCode)list.Attributes["membertype"])
				{
					case ListCreatedFromCode.Account:
						memberTypeInfo = new ListMemberTypeInfo { Code = ListCreatedFromCode.Account, MemberType = Account.EntityLogicalName, AttributeName = "accountid", };
						break;
					case ListCreatedFromCode.Contact:
						memberTypeInfo = new ListMemberTypeInfo { Code = ListCreatedFromCode.Contact, MemberType = Contact.EntityLogicalName, AttributeName = "contactid", };
						break;
					case ListCreatedFromCode.Lead:
						memberTypeInfo = new ListMemberTypeInfo { Code = ListCreatedFromCode.Lead, MemberType = Lead.EntityLogicalName, AttributeName = "leadid", };
						break;
				}

				if (isDynamicList)
				{
					ProcessDynamicList(serviceProvider, memberTypeInfo, list, campaignEntity);
				}
				else
				{
					ProcessStaticList(serviceProvider, memberTypeInfo, list, campaignEntity);
				}
			}

			//	Create the parent relationship between Sms and CampaignActivity
			var relatedSmses = new EntityReferenceCollection();
			GetTracingService(serviceProvider).Trace("There are {0} smses in the Dictionary", _smses.Count);
			foreach (var sms in _smses)
			{
				GetTracingService(serviceProvider).Trace("Adding SMS relationship for [{0}]", sms.Value.Id);
				relatedSmses.Add(new EntityReference(Sms.LogicalName, sms.Value.Id));
			}

			Relationship smsCampaignActivityRelationship = new Relationship("azuro_azuro_smscampaignactivity_azuro_sms");

			orgSvc.Associate(SmsCampaignActivity.LogicalName, campaignEntity.Id, smsCampaignActivityRelationship, relatedSmses);

			//	Update Campaign Entity, set azuro_campaignstatusreason to Executed
			SetOptionSetValue(campaignEntity.Attributes["azuro_campaignstatusreason"], (int)SmsCampaignStatusReason.Executed);
			var updateCampaignRequest = new UpdateRequest
							{
								Target = campaignEntity,
							};
			var updResult = orgSvc.Execute(updateCampaignRequest);
		}

		private void ProcessStaticList(IServiceProvider serviceProvider, ListMemberTypeInfo memberTypeInfo, Entity list, Entity campaignEntity)
		{
			GetTracingService(serviceProvider).Trace("Processing static list {0} with {1} members.", list.Id, list.Attributes["membercount"]);

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
				GetTracingService(serviceProvider).Trace("The marketing list {0} has no members.", list.Id);
				return;
			}

			ProcessListMembers(serviceProvider, memberTypeInfo, fetchmembersresult.Entities, campaignEntity);
		}

		private void ProcessDynamicList(IServiceProvider serviceProvider, ListMemberTypeInfo memberTypeInfo, Entity list, Entity campaignEntity)
		{
			GetTracingService(serviceProvider).Trace("Processing dynamic list {0}.", list.Id);

			var fetchmembersresult = GetOrganizationService(serviceProvider).RetrieveMultiple(new FetchExpression(list.Attributes["query"] as string));
			if (fetchmembersresult.Entities.Count < 1)
			{
				GetTracingService(serviceProvider).Trace("The marketing list {0} has no members.", list.Id);
				return;
			}

			ProcessListMembers(serviceProvider, memberTypeInfo, fetchmembersresult.Entities, campaignEntity);
		}

		private void ProcessListMembers(IServiceProvider serviceProvider, ListMemberTypeInfo memberTypeInfo, DataCollection<Entity> listMembers, Entity campaignEntity)
		{
			GetTracingService(serviceProvider).Trace("Processing the list members. There are [{0}] members.", listMembers.Count);
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
				GetTracingService(serviceProvider).Trace("Retrieving Entity [{0}] with Attribute: [{1}]", memberTypeInfo.MemberType, memberTypeInfo.AttributeName);

				var orgSvc = GetOrganizationService(serviceProvider);
				var entity = orgSvc.Retrieve(memberTypeInfo.MemberType, (Guid)member.Attributes[memberTypeInfo.AttributeName], new ColumnSet(true));

				foreach (var i in entity.Attributes)
				{
					GetTracingService(serviceProvider).Trace("Entity Attributes: {0} - {1}", i.Key, i.Value);
				}

				var sms = new Sms
					{
						Direction = true,
						DueDate = DateTime.Now,
						Message = campaignEntity.Attributes["azuro_content"] as string,
						MobilePhone = FormatMobilePhone(serviceProvider, member),
						Status = SmsStatus.Send,
						Subject = campaignEntity.Attributes["azuro_name"] as string,
					};

				CreateSms(serviceProvider, sms, campaignEntity);

				GetTracingService(serviceProvider).Trace("Create the relationship between the Sms and the {0}.", member.LogicalName);

				//	Create the relationship between Sms and Contact / Lead
				var relatedSms = new EntityReferenceCollection();
				relatedSms.Add(new EntityReference(Sms.LogicalName, sms.Id));

				Relationship relationship = new Relationship("azuro_" + member.LogicalName + "_azuro_sms");

				orgSvc.Associate(member.LogicalName, member.Id, relationship, relatedSms);

				GetTracingService(serviceProvider).Trace("Relationship between the Sms {0} and the {0}:{1} created.", sms.Id, member.LogicalName, member.Id);
			}
		}

		private string FormatMobilePhone(IServiceProvider serviceProvider, Entity member)
		{
			string mobilePhone = string.Empty;
			if (SmsConfiguration != null)
			{
				switch (member.LogicalName)
				{
					case "contact":
						if (!string.IsNullOrEmpty(SmsConfiguration.ContactInternationalDialCode))
						{
							mobilePhone = string.Format("{0}{1}", GetDialCode(serviceProvider, member, SmsConfiguration.ContactInternationalDialCode), (member.Attributes["mobilephone"] as string).TrimStart('0'));
						}
						break;
					case "account":
						if (!string.IsNullOrEmpty(SmsConfiguration.AccountInternationalDialCode))
						{
							mobilePhone = string.Format("{0}{1}", GetDialCode(serviceProvider, member, SmsConfiguration.AccountInternationalDialCode), (member.Attributes["azuro_mobilephone"] as string).TrimStart('0'));
						}
						break;
					case "lead":
						if (!string.IsNullOrEmpty(SmsConfiguration.LeadInternationalDialCode))
						{
							mobilePhone = string.Format("{0}{1}", GetDialCode(serviceProvider, member, SmsConfiguration.LeadInternationalDialCode), (member.Attributes["mobilephone"] as string).TrimStart('0'));
						}
						break;
				}
			}
			return mobilePhone;
		}

		private string GetDialCode(IServiceProvider serviceProvider, Entity entity, string fieldname)
		{
			if (entity.Attributes.ContainsKey(fieldname))
			{
				var mobilefield = entity[fieldname];
				if (mobilefield is OptionSetValue)
				{
					return GetCrmHelper(serviceProvider).GetOptionSetValueText(entity.LogicalName, fieldname, ((OptionSetValue)mobilefield).Value);
				}
				else if (mobilefield is string)
					return mobilefield as string;
			}

			return string.Empty;
		}

		private void CreateSms(IServiceProvider serviceProvider, Sms sms, Entity campaignEntity)
		{
			GetTracingService(serviceProvider).Trace("Creating Sms to {0}.", sms.MobilePhone);
			if (_smses.ContainsKey(sms.MobilePhone))
			{
				GetTracingService(serviceProvider).Trace("Number previously added to Sms List: {0}.", sms.MobilePhone);
				return;
			}
			else
				_smses.Add(sms.MobilePhone, sms);

			var newSms = new Entity
							{
								LogicalName = Sms.LogicalName,
								Attributes =
											{
												{ "azuro_message", sms.Message },
												{ "azuro_mobilephone", sms.MobilePhone },
												{ "azuro_direction", sms.Direction },
												{ "azuro_status", new OptionSetValue((int)sms.Status) },
												{ "subject", sms.Subject },
												{ "scheduledend", sms.DueDate },
											}

							};

			sms.Id = GetOrganizationService(serviceProvider).Create(newSms);

			GetTracingService(serviceProvider).Trace("Assign the Sms to Owner to {0}.", ((EntityReference)campaignEntity.Attributes["owninguser"]).Id);

			//	Set the Owner on the Sms
			var assign = new AssignRequest
			{
				Assignee = new EntityReference(SystemUser.EntityLogicalName, ((EntityReference)campaignEntity.Attributes["owninguser"]).Id),
				Target = new EntityReference(Sms.LogicalName, sms.Id)
			};

			GetOrganizationService(serviceProvider).Execute(assign);
		}
	}
}
