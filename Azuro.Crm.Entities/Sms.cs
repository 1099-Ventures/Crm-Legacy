using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.Entities
{
	[CrmEntity("azuro_sms")]
	public class Sms : CrmEntity<Sms>
	{
		private Guid _id;
		[CrmField("activityid", true)]
		public Guid Id { get { return _id; } set { _id = value; AddUpdatedAttribute("activityid", value); } }

		private string _mobilePhone;
		[CrmField("azuro_mobilephone")]
		public string MobilePhone { get { return _mobilePhone; } set { _mobilePhone = value; AddUpdatedAttribute("azuro_mobilephone", value); } }

		private string _message;
		[CrmField("azuro_message")]
		public string Message { get { return _message; } set { _message = value; AddUpdatedAttribute("azuro_message", value); } }

		private SmsProvider _provider;
		[CrmField("azuro_provider")]
		public SmsProvider Provider { get { return _provider; } set { _provider = value; AddUpdatedAttribute("azuro_provider", value); } }

		private string _providerMessageId;
		[CrmField("azuro_providermessageid")]
		public string ProviderMessageId { get { return _providerMessageId; } set { _providerMessageId = value; AddUpdatedAttribute("azuro_providermessageid", value); } }

		private DateTime? _sentDate;
		[CrmField("azuro_sentdate")]
		public DateTime? SentDate { get { return _sentDate; } set { _sentDate = value; AddUpdatedAttribute("azuro_sentdate", value); } }

		private DateTime? _startDate;
		[CrmField("scheduledstart")]
		public DateTime? StartDate { get { return _startDate; } set { _startDate = value; AddUpdatedAttribute("scheduledstart", value); } }

		private DateTime? _dueDate;
		[CrmField("scheduledend")]
		public DateTime? DueDate { get { return _dueDate; } set { _dueDate = value; AddUpdatedAttribute("scheduledend", value); } }

		private bool _direction;
		[CrmField("azuro_direction")]
		public bool Direction { get { return _direction; } set { _direction = value; AddUpdatedAttribute("azuro_direction", value); } }

		private string _subject;
		[CrmField("subject")]
		public string Subject { get { return _subject; } set { _subject = value; AddUpdatedAttribute("subject", value); } }

		private SmsStatus _status;
		[CrmField("azuro_status")]
		public SmsStatus Status { get { return _status; } set { _status = value; AddUpdatedAttribute("azuro_status", value); } }

		private SmsActivityStatus _activityStatus;
		[CrmField("statecode")]
		public SmsActivityStatus ActivityStatus { get { return _activityStatus; } set { _activityStatus = value; AddUpdatedAttribute("statecode", value); } }

		private SmsStatusReason _statusReason;
		[CrmField("statuscode")]
		public SmsStatusReason StatusReason { get { return _statusReason; } set { _statusReason = value; AddUpdatedAttribute("statuscode", value); } }

		private SmsPriority _priority;
		[CrmField("prioritycode")]
		public SmsPriority Priority { get { return _priority; } set { _priority = value; AddUpdatedAttribute("prioritycode", value); } }

		private bool _isBilled;
		[CrmField("isbilled")]
		public bool IsBilled { get { return _isBilled; } set { _isBilled = value; AddUpdatedAttribute("isbilled", value); } }

		private string _providerErrorMessage;
		[CrmField("azuro_smserrormessage")]
		public string ProviderErrorMessage { get { return _providerErrorMessage; } set { _providerErrorMessage = value; AddUpdatedAttribute("azuro_smserrormessage", value); } }

		private CrmEntityReference _ownerId;
		[CrmField("ownerid")]
		public CrmEntityReference OwnerId { get { return _ownerId; } set { _ownerId = value; AddUpdatedAttribute("ownerid", value); } }

		private CrmEntityReference _regardingId;
		[CrmField("regardingid")]
		public CrmEntityReference RegardingId { get { return _regardingId; } set { _regardingId = value; AddUpdatedAttribute("regardingid", value); } }
	}
}
