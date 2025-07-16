using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Azuro.MSMQ;
using System.Configuration;
using Azuro.Crm.SmsMessages;
using Azuro.Logging;

namespace Azuro.Sms.ProviderAck
{
	public partial class ClickatellStatus : System.Web.UI.Page
	{
		private string MsmqQueueName { get { return ConfigurationManager.AppSettings["MsmqQueueName"] ?? @".\private$\SmsResponseQueue"; } }

		private static readonly Dictionary<int, Tuple<MessageStatus, string, string>> Messages = new Dictionary<int, Tuple<MessageStatus, string, string>>
							{
								{  -1, new Tuple<MessageStatus, string, string> (  MessageStatus.Unknown, "Unknown Message", "The Gateway Error Code is Unknown.") },
								{ 001, new Tuple<MessageStatus, string, string> (  MessageStatus.Unknown, "Unknown Message", "The message ID is incorrect or reporting is delayed.") },
								{ 002, new Tuple<MessageStatus, string, string> (  MessageStatus.Processing, "Message queued", "The message could not be delivered and has been queued for attempted redelivery.") },
								{ 003, new Tuple<MessageStatus, string, string> (  MessageStatus.Processing, "Delivered to gateway", "Delivered to the upstream gateway or network (delivered to the recipient).") },
								{ 004, new Tuple<MessageStatus, string, string> (  MessageStatus.Success, "Received by recipient", "Confirmation of receipt on the handset of the recipient.") },
								{ 005, new Tuple<MessageStatus, string, string> (  MessageStatus.Failed, "Error with message", "There was an error with the message, probably caused by the content of the message itself.") },
								{ 006, new Tuple<MessageStatus, string, string> (  MessageStatus.Failed, "User cancelled message delivery", "The message was terminated by a user (stop message command) or by our staff.") },
								{ 007, new Tuple<MessageStatus, string, string> (  MessageStatus.Failed, "Error delivering message", "An error occurred delivering the message to the handset.") },
								{ 008, new Tuple<MessageStatus, string, string> (  MessageStatus.Processing, "OK", "Message received by gateway.") },
								{ 009, new Tuple<MessageStatus, string, string> (  MessageStatus.Failed, "Routing error", "The routing gateway or network has had an error routing the message.") },
								{ 010, new Tuple<MessageStatus, string, string> (  MessageStatus.Failed, "Message expired", "Message has expired before we were able to deliver it to the upstream gateway. No charge applies.") },
								{ 011, new Tuple<MessageStatus, string, string> (  MessageStatus.Processing, "Message queued for later delivery", "Message has been queued at the gateway for delivery at a later time (delayed delivery).") },
								{ 012, new Tuple<MessageStatus, string, string> (  MessageStatus.Failed, "Out of credit", "The message cannot be delivered due to a lack of funds in your account. Please re-purchase credits.") },
								{ 014, new Tuple<MessageStatus, string, string> (  MessageStatus.Failed, "Maximum MT limit exceeded", "The allowable amount for MT messaging has been exceeded.") },
							};

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (Request.QueryString.Count == 0)
				{
					Log.Warn("No query parameters");
					return;
				}

				//	Process GET variables
				//	api_id, apiMsgId, cliMsgId, to, timestamp, from, status and charge
				Log.Trace("Processing Query String: [{0}]", Request.QueryString);
				decimal charge = 0.0M;
				decimal.TryParse(Request.QueryString["charge"], out charge);
				long timestamp = 0L;
				long.TryParse(Request.QueryString["timestamp"], out timestamp);
				SmsResponseMessage msg = new SmsResponseMessage
				{
					ProviderReceiverType = this.GetType().FullName,
					ProviderId = Request.QueryString["api_id"],
					ProviderMsgId = Request.QueryString["apiMsgId"],
					ClientMsgId = Request.QueryString["cliMsgId"],
					To = Request.QueryString["to"],
					From = Request.QueryString["from"],
					Charge = charge,
					ProviderTimeStamp = timestamp != 0 ? new DateTime(timestamp) : DateTime.MinValue,
					ProviderStatus = Request.QueryString["status"],
					ProviderStatusMessage = CrackStatusMesssage(Request.QueryString["status"]),
					Status = CrackStatus(Request.QueryString["status"]),
				};

				Log.Trace("Inserting the QueueItem");
				//	Pop it into the Queue - Use 1 queue and let the msgId sort it out on the processor side?
				QueueHelper.Insert(MsmqQueueName, msg);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Exception occurred");
			}
		}

		private string CrackStatusMesssage(string status)
		{
			string scode = Request.QueryString["status"];
			int code = int.Parse(scode);

			if (Messages.ContainsKey(code))
			{
				return Messages[code].Item3;
			}

			return string.Format("Status [{0}] - Message Unknown", code);
		}

		private MessageStatus CrackStatus(string status)
		{
			string scode = Request.QueryString["status"];
			int code = int.Parse(scode);

			if (Messages.ContainsKey(code))
			{
				return Messages[code].Item1;
			}

			return MessageStatus.Unknown;
		}
	}
}