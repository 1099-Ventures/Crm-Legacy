using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Azuro.Common;
using Azuro.Crm.Integration;
using System.IO;

namespace Azuro.Crm.Entities
{
	[CrmEntity("email")]
	public class Email : CrmEntity<Email> // 4202 - activityid - subject
	{
		private string _SubmittedBy;
		[CrmField("submittedby")]
		public string SubmittedBy { get { return _SubmittedBy; } set { _SubmittedBy=value; AddUpdatedAttribute("submittedby", value); } } // String - SubmittedBy - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private bool _IsBilled;
		[CrmField("isbilled")]
		public bool IsBilled { get { return _IsBilled; } set { _IsBilled=value; AddUpdatedAttribute("isbilled", value); } } // Boolean - IsBilled - None

		private List<CrmEntity> _from;
		[CrmField("from")]
		public List<CrmEntity> from { get { return _from; } set { _from=value; AddUpdatedAttribute("from", value); } } // PartyList - from - None

		private bool _IsWorkflowCreated;
		[CrmField("isworkflowcreated")]
		public bool IsWorkflowCreated { get { return _IsWorkflowCreated; } set { _IsWorkflowCreated=value; AddUpdatedAttribute("isworkflowcreated", value); } } // Boolean - IsWorkflowCreated - None

		private Guid _id;
		[CrmField("activityid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - ActivityId - SystemRequired

		private string _Category;
		[CrmField("category")]
		public string Category { get { return _Category; } set { _Category=value; AddUpdatedAttribute("category", value); } } // String - Category - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private int _Notifications;
		[CrmField("notifications", IsPicklist = true)]
		public int Notifications { get { return _Notifications; } set { _Notifications=value; AddUpdatedAttribute("notifications", value); } } // Picklist - Notifications - None

		private bool _DeliveryReceiptRequested;
		[CrmField("deliveryreceiptrequested")]
		public bool DeliveryReceiptRequested { get { return _DeliveryReceiptRequested; } set { _DeliveryReceiptRequested=value; AddUpdatedAttribute("deliveryreceiptrequested", value); } } // Boolean - DeliveryReceiptRequested - None

		private string _OwnerIdName;
		[CrmField("owneridname", IsRequired = true)]
		public string OwnerIdName { get { return _OwnerIdName; } set { _OwnerIdName=value; AddUpdatedAttribute("owneridname", value); } } // String - OwnerIdName - SystemRequired

		private int _PriorityCode;
		[CrmField("prioritycode", IsPicklist = true)]
		public int PriorityCode { get { return _PriorityCode; } set { _PriorityCode=value; AddUpdatedAttribute("prioritycode", value); } } // Picklist - PriorityCode - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private int _DeliveryAttempts;
		[CrmField("deliveryattempts")]
		public int DeliveryAttempts { get { return _DeliveryAttempts; } set { _DeliveryAttempts=value; AddUpdatedAttribute("deliveryattempts", value); } } // Integer - DeliveryAttempts - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private DateTime? _ScheduledStart;
		[CrmField("scheduledstart")]
		public DateTime? ScheduledStart { get { return _ScheduledStart; } set { _ScheduledStart=value; AddUpdatedAttribute("scheduledstart", value); } } // DateTime - ScheduledStart - None

		private string _Sender;
		[CrmField("sender")]
		public string Sender { get { return _Sender; } set { _Sender=value; AddUpdatedAttribute("sender", value); } } // String - Sender - None

		private CrmEntityReference _RegardingObjectId;
		[CrmField("regardingobjectid")]
		public CrmEntityReference RegardingObjectId { get { return _RegardingObjectId; } set { _RegardingObjectId=value; AddUpdatedAttribute("regardingobjectid", value); } } // Lookup - RegardingObjectId - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private CrmEntityReference _OwningTeam;
		[CrmField("owningteam")]
		public CrmEntityReference OwningTeam { get { return _OwningTeam; } set { _OwningTeam=value; AddUpdatedAttribute("owningteam", value); } } // Lookup - OwningTeam - None

		private string _TrackingToken;
		[CrmField("trackingtoken")]
		public string TrackingToken { get { return _TrackingToken; } set { _TrackingToken=value; AddUpdatedAttribute("trackingtoken", value); } } // String - TrackingToken - None

		private List<CrmEntity> _to;
		[CrmField("to")]
		public List<CrmEntity> to { get { return _to; } set { _to=value; AddUpdatedAttribute("to", value); } } // PartyList - to - None

		private string _MessageId;
		[CrmField("messageid")]
		public string MessageId { get { return _MessageId; } set { _MessageId=value; AddUpdatedAttribute("messageid", value); } } // String - MessageId - None

		private DateTime? _ActualStart;
		[CrmField("actualstart")]
		public DateTime? ActualStart { get { return _ActualStart; } set { _ActualStart=value; AddUpdatedAttribute("actualstart", value); } } // DateTime - ActualStart - None

		private string _ToRecipients;
		[CrmField("torecipients")]
		public string ToRecipients { get { return _ToRecipients; } set { _ToRecipients=value; AddUpdatedAttribute("torecipients", value); } } // String - ToRecipients - None

		private string _Subject;
		[CrmField("subject")]
		public string Subject { get { return _Subject; } set { _Subject=value; AddUpdatedAttribute("subject", value); } } // String - Subject - None

		private List<CrmEntity> _bcc;
		[CrmField("bcc")]
		public List<CrmEntity> bcc { get { return _bcc; } set { _bcc=value; AddUpdatedAttribute("bcc", value); } } // PartyList - bcc - None

		private List<CrmEntity> _cc;
		[CrmField("cc")]
		public List<CrmEntity> cc { get { return _cc; } set { _cc=value; AddUpdatedAttribute("cc", value); } } // PartyList - cc - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _RegardingObjectIdName;
		[CrmField("regardingobjectidname")]
		public string RegardingObjectIdName { get { return _RegardingObjectIdName; } set { _RegardingObjectIdName=value; AddUpdatedAttribute("regardingobjectidname", value); } } // String - RegardingObjectIdName - None

		private bool _DirectionCode;
		[CrmField("directioncode")]
		public bool DirectionCode { get { return _DirectionCode; } set { _DirectionCode=value; AddUpdatedAttribute("directioncode", value); } } // Boolean - DirectionCode - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _ActivityTypeCode;
		[CrmField("activitytypecode", IsRequired = true)]
		public string ActivityTypeCode { get { return _ActivityTypeCode; } set { _ActivityTypeCode=value; AddUpdatedAttribute("activitytypecode", value); } } // EntityName - ActivityTypeCode - SystemRequired

		private bool _Compressed;
		[CrmField("compressed", IsRequired = true)]
		public bool Compressed { get { return _Compressed; } set { _Compressed=value; AddUpdatedAttribute("compressed", value); } } // Boolean - Compressed - SystemRequired

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid", IsRequired = true)]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _OwnerIdYomiName;
		[CrmField("owneridyominame", IsRequired = true)]
		public string OwnerIdYomiName { get { return _OwnerIdYomiName; } set { _OwnerIdYomiName=value; AddUpdatedAttribute("owneridyominame", value); } } // String - OwnerIdYomiName - SystemRequired

		private int _ScheduledDurationMinutes;
		[CrmField("scheduleddurationminutes")]
		public int ScheduledDurationMinutes { get { return _ScheduledDurationMinutes; } set { _ScheduledDurationMinutes=value; AddUpdatedAttribute("scheduleddurationminutes", value); } } // Integer - ScheduledDurationMinutes - None

		private string _RegardingObjectIdYomiName;
		[CrmField("regardingobjectidyominame")]
		public string RegardingObjectIdYomiName { get { return _RegardingObjectIdYomiName; } set { _RegardingObjectIdYomiName=value; AddUpdatedAttribute("regardingobjectidyominame", value); } } // String - RegardingObjectIdYomiName - None

		private string _Description;
		[CrmField("description")]
		public string Description { get { return _Description; } set { _Description=value; AddUpdatedAttribute("description", value); } } // Memo - Description - None

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private Guid _MessageIdDupCheck;
		[CrmField("messageiddupcheck", IsRequired = true)]
		public Guid MessageIdDupCheck { get { return _MessageIdDupCheck; } set { _MessageIdDupCheck=value; AddUpdatedAttribute("messageiddupcheck", value); } } // Uniqueidentifier - MessageIdDupCheck - SystemRequired

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private string _RegardingObjectTypeCode;
		[CrmField("regardingobjecttypecode")]
		public string RegardingObjectTypeCode { get { return _RegardingObjectTypeCode; } set { _RegardingObjectTypeCode=value; AddUpdatedAttribute("regardingobjecttypecode", value); } } // EntityName - RegardingObjectTypeCode - None

		private CrmEntityReference _ServiceId;
		[CrmField("serviceid")]
		public CrmEntityReference ServiceId { get { return _ServiceId; } set { _ServiceId=value; AddUpdatedAttribute("serviceid", value); } } // Lookup - ServiceId - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private string _Subcategory;
		[CrmField("subcategory")]
		public string Subcategory { get { return _Subcategory; } set { _Subcategory=value; AddUpdatedAttribute("subcategory", value); } } // String - Subcategory - None

		private string _MimeType;
		[CrmField("mimetype")]
		public string MimeType { get { return _MimeType; } set { _MimeType=value; AddUpdatedAttribute("mimetype", value); } } // String - MimeType - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - None

		private string _TransactionCurrencyIdName;
		[CrmField("transactioncurrencyidname")]
		public string TransactionCurrencyIdName { get { return _TransactionCurrencyIdName; } set { _TransactionCurrencyIdName=value; AddUpdatedAttribute("transactioncurrencyidname", value); } } // String - TransactionCurrencyIdName - None

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber=value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private DateTime? _ActualEnd;
		[CrmField("actualend")]
		public DateTime? ActualEnd { get { return _ActualEnd; } set { _ActualEnd=value; AddUpdatedAttribute("actualend", value); } } // DateTime - ActualEnd - None

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - None

		private bool _ReadReceiptRequested;
		[CrmField("readreceiptrequested")]
		public bool ReadReceiptRequested { get { return _ReadReceiptRequested; } set { _ReadReceiptRequested=value; AddUpdatedAttribute("readreceiptrequested", value); } } // Boolean - ReadReceiptRequested - None

		private bool _IsRegularActivity;
		[CrmField("isregularactivity", IsRequired = true)]
		public bool IsRegularActivity { get { return _IsRegularActivity; } set { _IsRegularActivity=value; AddUpdatedAttribute("isregularactivity", value); } } // Boolean - IsRegularActivity - SystemRequired

		private DateTime? _ScheduledEnd;
		[CrmField("scheduledend")]
		public DateTime? ScheduledEnd { get { return _ScheduledEnd; } set { _ScheduledEnd=value; AddUpdatedAttribute("scheduledend", value); } } // DateTime - ScheduledEnd - None

		private int _StatusCode;
		[CrmField("statuscode", IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - None

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private int _ActualDurationMinutes;
		[CrmField("actualdurationminutes")]
		public int ActualDurationMinutes { get { return _ActualDurationMinutes; } set { _ActualDurationMinutes=value; AddUpdatedAttribute("actualdurationminutes", value); } } // Integer - ActualDurationMinutes - None

	}
}
