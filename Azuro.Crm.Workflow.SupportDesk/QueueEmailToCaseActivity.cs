using Azuro.Crm.Entities;
using Azuro.Crm.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Reflection;

namespace Azuro.Crm.Workflow
{
	public class QueueEmailToCaseActivity : ACrmCodeActivity
	{
		//private Guid QueueItem { get; set; }
		//private Guid EmailItem { get; set; }
		//private Guid DefaultQueue { get; set; }
		//private Guid DefaultUser { get; set; }

		[Input("Email (Input)")]
		[ReferenceTarget("email")]
		public InArgument<EntityReference> EmailReference { get; set; }

		[Input("QueueItem (Input)")]
		[ReferenceTarget("queueitem")]
		public InArgument<EntityReference> QueueItemReference { get; set; }

		[Input("Default Queue (Input)")]
		[ReferenceTarget("queue")]
		public InArgument<EntityReference> DefaultQueueReference { get; set; }

		[Input("Default User (Input)")]
		[ReferenceTarget("systemuser")]
		public InArgument<EntityReference> DefaultUserReference { get; set; }

		//[Input("Always Create New Case (Input)")]
		//public InArgument<bool> AlwaysCreateNewCase { get; set; }

		//[Output("New Case (Output)")]
		//[ReferenceTarget("incident")]
		//public OutArgument<EntityReference> NewCase { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			Trace(context, "Entering QueueEmailToCaseActivity.Execute");

			if (!IsValid(context, EmailReference))
			{
				throw new InvalidPluginExecutionException("No email provided as input");
			}

			if (!IsValid(context, QueueItemReference))
			{
				throw new InvalidPluginExecutionException("No queueitem provided as input");
			}

			//DefaultQueue = IsValid(context, DefaultQueueReference) ? DefaultQueueReference.Get(context).Id : Guid.Empty;
			//DefaultUser = IsValid(context,DefaultUserReference) ? DefaultUserReference.Get(context).Id : Guid.Empty;

			CreateCaseFromEmail(context);
		}

		private void CreateCaseFromEmail(CodeActivityContext context)
		{
			Trace(context, "Entering CreateCaseFromEmail");

			var crmHelper = GetCrmHelper(context);
			var emailId = EmailReference.Get(context).Id;
			var caseItem = new Azuro.Crm.Entities.Case();
			var email = crmHelper.GetEntity<Azuro.Crm.Entities.Email>(emailId);

			Trace(context, email != null ? "Email retrieved: {0} -> {1}" : "Email was not found: {0}", emailId, email != null ? email.Sender : "");

			// Moved this code here to force the correct CaseOrigin before further processing occurs. 
			var caseOrigin = crmHelper.GetOptionSetValueForText("incident_caseorigincode", "Email");
			if (caseOrigin.HasValue)
				caseItem.CaseOriginCode = caseOrigin.Value;

			if (email != null)
			{
				if (email.RegardingObjectId == null || email.RegardingObjectId.ReferencedEntityId == Guid.Empty)
				{
					//Create a new Case
					caseItem.Title = email.Subject;
					caseItem.Description = Azuro.Common.Formatting.HtmlFormatter.StripHtml(email.Description);
					if (caseItem.Description.Length > 2000)
						caseItem.Description = caseItem.Description.Substring(0, 1975) + " ... (Msg Truncated)";

					//Fetch the email contact and account details
					CrmEntityReference senderRef = null;

					if (email.from != null && email.from.Count == 1)
					{
						var sender = email.from[0];
						if (string.Compare(sender.LogicalName, "activityparty", true) == 0)
						{
							if (sender.Attributes["partyid"] != null)
							{
								senderRef = ((CrmEntityReference)sender.Attributes["partyid"]);
							}
						}
					}

					var defaultUser = IsValid(context, DefaultUserReference) ? DefaultUserReference.Get(context).Id : Guid.Empty;
					if (senderRef == null)
					{
						Trace(context, "No sender matched - Creating default sender to force case creation.");
						senderRef = defaultUser != Guid.Empty ? new CrmEntityReference("systemuser", "systemuserid", defaultUser) : email.OwnerId;
					}
					switch (senderRef.EntityName)
					{
						case "systemuser":	//	TODO: What to do with a User sent email?
							{
								var user = crmHelper.GetEntity<Entities.User>(senderRef.ReferencedEntityId);
								//	if a forwarded mail, find first instance of sender, and try locate customer or account on that.
								string searchMail = string.Empty;
								if (email.Subject.ToLower().StartsWith("fw:"))
									searchMail = ExtractForwarderEmailAddress(caseItem);
								else	//	if its a user and not a forward mail, find the user as a contact?
									searchMail = user.InternalEMailAddress;

								if (!string.IsNullOrEmpty(searchMail))
								{
									//	find the email address
									var contactList = crmHelper.GetEntityList<Entities.Contact>(new List<CrmQuery>
																										{ 
																											new CrmQuery{ Key = "emailaddress1", Value = searchMail, FilterType = FilterType.Or, Operator = FilterOperator.Equal},
																											new CrmQuery{ Key = "emailaddress2", Value = searchMail, FilterType = FilterType.Or, Operator = FilterOperator.Equal},
																											new CrmQuery{ Key = "emailaddress3", Value = searchMail, FilterType = FilterType.Or, Operator = FilterOperator.Equal},
																										});
									if (contactList.Count > 0)
									{
										//	grab the first one?
										CreateCaseByContact(context, crmHelper, caseItem, email, contactList[0]);
									}
									else
										Trace(context, "Unable to match the user to a contact, or find a forward email address {0}", searchMail);
								}
								else
									Trace(context, "Unable to match the user to a contact, or find a forward email address {0}", searchMail);

								break;
							}
						case "contact":
							{
								var contact = crmHelper.GetEntity<Entities.Contact>(senderRef.ReferencedEntityId);
								CreateCaseByContact(context, crmHelper, caseItem, email, contact);
								break;
							}
						case "queue":
							{
								var queue = crmHelper.GetEntity<Entities.Queue>(senderRef.ReferencedEntityId);
								Trace(context, "Queue sender unhandled");
								break;
							}
						case "account":
							{
								var account = crmHelper.GetEntity<Entities.Account>(senderRef.ReferencedEntityId);
								CreateCase(context, crmHelper, account, null, caseItem, email);
								break;
							}
					}

					//	TODO: The Case Id is not pulling back, this seems like a bug in CrmHelper.
					//if (caseItem.Id != Guid.Empty)
					//	NewCase.Set(Context, new EntityReference("incident", caseItem.Id));
					//else
					//	throw new InvalidPluginExecutionException("Case Item does not have an ID!");
				}
			}
#if !DEBUG
			//	If the Case was created, remove the item from the queue
			//	TODO: Revisit this
			//if ((email.RegardingObjectId != null && email.RegardingObjectId.ReferencedEntityId != Guid.Empty) || (this.QueueItem != Guid.Empty && caseItem != null && caseItem.Id != null))
			//	CrmHelper.Delete<Entities.QueueItem>(QueueItem);
#endif
		}

		private static string ExtractForwarderEmailAddress(Case caseItem)
		{
			string searchMail = string.Empty;
			var pos = caseItem.Description.ToLower().IndexOf("from:");
			if (pos > 0)
				searchMail = caseItem.Description.Substring(pos + 5, caseItem.Description.IndexOf('\r', pos + 6) - (pos + 5));

			searchMail = searchMail.Trim(']', '\r', '\n', ' ', '<', '>');
			var subs = searchMail.Split(' ', '[', ':', '<');
			if (subs.Length > 0)
				searchMail = subs[subs.Length - 1].Trim(']', '\r', '\n', ' ', '<', '>');

			return searchMail;
		}

		private void CreateCaseByContact(CodeActivityContext context, ICrmHelper crmHelper, Case caseItem, Entities.Email email, Entities.Contact contact)
		{
			caseItem.ContactId = new CrmEntityReference(Entities.Contact.LogicalName, contact.FullName, contact.Id);
			Entities.Account account = null;
			if (contact.ParentCustomerId != null)
			{
				//	Get Account
				account = crmHelper.GetEntity<Entities.Account>(contact.ParentCustomerId.ReferencedEntityId);
			}

			CreateCase(context, crmHelper, account, contact, caseItem, email);
		}

		private void CreateCase(CodeActivityContext context, ICrmHelper crmHelper, Entities.Account account, Entities.Contact contact, Entities.Case caseItem, Entities.Email emailItem)
		{
			var caseHelper = new CaseHelper(crmHelper);
			var caseOwner = emailItem.OwnerId;
			var defaultUser = IsValid(context, DefaultUserReference) ? DefaultUserReference.Get(context).Id : Guid.Empty;
			if (defaultUser != Guid.Empty)
				caseOwner = new CrmEntityReference("systemuser", "systemuserid", defaultUser);
			if (!caseHelper.CreateCase(account, contact, caseItem, caseOwner))
				throw new InvalidPluginExecutionException("Unable to create case, Contact and Account can't both be NULL");

			var defaultQueue = IsValid(context, DefaultQueueReference) ? DefaultQueueReference.Get(context).Id : Guid.Empty;
			if (defaultQueue != Guid.Empty)
			{
				var queueHelper = new QueueHelper(crmHelper);
				queueHelper.Assign(caseItem, defaultQueue);
			}
			//This code was moved to the time where the actual case is created. 
			//var caseOrigin = CrmHelper.GetOptionSetValueForText<Case>("incident_caseorigincode", "Email");
			//var caseOrigin = CrmHelper.GetOptionSetValueForText("incident_caseorigincode", "Email");
			//if (caseOrigin.HasValue)
			//	caseItem.CaseOriginCode = caseOrigin.Value;

			var attachments = crmHelper.GetEntityList<Entities.EMailAttachment>(new List<CrmQuery> { new CrmQuery { Key = "activityid", Value = emailItem.Id, FilterType = FilterType.And, Operator = FilterOperator.Equal, } });
			foreach (var attachment in attachments)
			{
				var note = new Note
				{
					DocumentBody = attachment.Body,
					FileName = attachment.FileName,
					FileSize = attachment.FileSize,
					MimeType = attachment.MimeType,
					NoteText = attachment.FileName,
					ObjectId = new CrmEntityReference(Case.LogicalName, caseItem.Title, caseItem.Id),
					Subject = attachment.FileName,
					OwnerId = caseItem.OwnerId,
					IsDocument = true,
				};

				crmHelper.Insert(note);
			}

			emailItem.UpdatedAttributes.Clear();
			emailItem.RegardingObjectId = new CrmEntityReference(Entities.Case.LogicalName, caseItem.Title, caseItem.Id);

			crmHelper.Update(emailItem);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void TestActivity(Guid organizationId, Guid emailId)
		{
			//var _crmHelper = CrmHelperFactory.Create(organizationId);
			//CreateCaseFromEmail(emailId);
			throw new NotImplementedException("Changed due to changes in base class");
		}
	}
}
