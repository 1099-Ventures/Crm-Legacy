using Azuro.Crm.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;

namespace Azuro.Crm.Workflow
{
	public class UpdateCaseFromNewEmailActivity : ACrmCodeActivity
	{
		[Input("Email input")]
		[ReferenceTarget("email")]
		public InArgument<EntityReference> EmailReference { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			if (!IsValid(context, EmailReference))
				throw new InvalidPluginExecutionException("No email provided as input");

			UpdateCaseFromEmail(context, EmailReference.Get(context).Id);
		}

		private void UpdateCaseFromEmail(CodeActivityContext context, Guid id)
		{
			var crmHelper = GetCrmHelper(context);
			var email = crmHelper.GetEntity<Entities.Email>(id);

			if (email != null)
			{
				var attachments = crmHelper.GetEntityList<Entities.EMailAttachment>(new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("regardingid", email.Id) });
				foreach (var attachment in attachments)
				{
					var note = new Entities.Note
					{
						DocumentBody = attachment.Body,
						FileName = attachment.FileName,
						FileSize = attachment.FileSize,
						MimeType = attachment.MimeType,
						NoteText = attachment.FileName,
						ObjectId = email.RegardingObjectId,
						Subject = attachment.FileName,
						OwnerId = email.OwnerId,
						IsDocument = true,
					};

					crmHelper.Insert(note);
				}

				if (email.RegardingObjectId != null && email.RegardingObjectId.ReferencedEntityId != Guid.Empty)
				{
					var note = new Entities.Note();
					note.Subject = email.Subject;
					note.NoteText = Azuro.Common.Formatting.HtmlFormatter.StripHtml(email.Description);
					note.ObjectId = email.RegardingObjectId;

					crmHelper.Insert(note);
				}
			}
		}
	}
}
