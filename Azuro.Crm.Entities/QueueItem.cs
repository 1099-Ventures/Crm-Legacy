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
	[CrmEntity("queueitem")]
	public class QueueItem : CrmEntity<QueueItem> // 2029 - queueitemid - title
	{
		private string _WorkerIdType;
		[CrmField("workeridtype")]
		public string WorkerIdType { get { return _WorkerIdType; } set { _WorkerIdType=value; AddUpdatedAttribute("workeridtype", value); } } // EntityName - WorkerIdType - ApplicationRequired

		private int _Status;
		[CrmField("status")]
		public int Status { get { return _Status; } set { _Status=value; AddUpdatedAttribute("status", value); } } // Integer - Status - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private string _Sender;
		[CrmField("sender")]
		public string Sender { get { return _Sender; } set { _Sender=value; AddUpdatedAttribute("sender", value); } } // String - Sender - None

		private string _TransactionCurrencyIdName;
		[CrmField("transactioncurrencyidname")]
		public string TransactionCurrencyIdName { get { return _TransactionCurrencyIdName; } set { _TransactionCurrencyIdName=value; AddUpdatedAttribute("transactioncurrencyidname", value); } } // String - TransactionCurrencyIdName - None

		private string _ObjectIdTypeCode;
		[CrmField("objectidtypecode")]
		public string ObjectIdTypeCode { get { return _ObjectIdTypeCode; } set { _ObjectIdTypeCode=value; AddUpdatedAttribute("objectidtypecode", value); } } // EntityName - ObjectIdTypeCode - None

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - ApplicationRequired

		private CrmEntityReference _OrganizationId;
		[CrmField("organizationid", IsRequired = true)]
		public CrmEntityReference OrganizationId { get { return _OrganizationId; } set { _OrganizationId=value; AddUpdatedAttribute("organizationid", value); } } // Lookup - OrganizationId - SystemRequired

		private int _StatusCode;
		[CrmField("statuscode", IsRequired = true, IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - SystemRequired

		private Guid _id;
		[CrmField("queueitemid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - QueueItemId - SystemRequired

		private string _WorkerIdName;
		[CrmField("workeridname")]
		public string WorkerIdName { get { return _WorkerIdName; } set { _WorkerIdName=value; AddUpdatedAttribute("workeridname", value); } } // String - WorkerIdName - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private CrmEntityReference _WorkerId;
		[CrmField("workerid")]
		public CrmEntityReference WorkerId { get { return _WorkerId; } set { _WorkerId=value; AddUpdatedAttribute("workerid", value); } } // Lookup - WorkerId - None

		private string _QueueIdName;
		[CrmField("queueidname", IsRequired = true)]
		public string QueueIdName { get { return _QueueIdName; } set { _QueueIdName=value; AddUpdatedAttribute("queueidname", value); } } // String - QueueIdName - SystemRequired

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - ApplicationRequired

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid")]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - ApplicationRequired

		private string _WorkerIdYomiName;
		[CrmField("workeridyominame")]
		public string WorkerIdYomiName { get { return _WorkerIdYomiName; } set { _WorkerIdYomiName=value; AddUpdatedAttribute("workeridyominame", value); } } // String - WorkerIdYomiName - None

		private CrmEntityReference _ObjectId;
		[CrmField("objectid")]
		public CrmEntityReference ObjectId { get { return _ObjectId; } set { _ObjectId=value; AddUpdatedAttribute("objectid", value); } } // Lookup - ObjectId - ApplicationRequired

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private string _OrganizationIdName;
		[CrmField("organizationidname", IsRequired = true)]
		public string OrganizationIdName { get { return _OrganizationIdName; } set { _OrganizationIdName=value; AddUpdatedAttribute("organizationidname", value); } } // String - OrganizationIdName - SystemRequired

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber=value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private string _Title;
		[CrmField("title")]
		public string Title { get { return _Title; } set { _Title=value; AddUpdatedAttribute("title", value); } } // String - Title - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private CrmEntityReference _QueueId;
		[CrmField("queueid")]
		public CrmEntityReference QueueId { get { return _QueueId; } set { _QueueId=value; AddUpdatedAttribute("queueid", value); } } // Lookup - QueueId - ApplicationRequired

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private string _ObjectIdName;
		[CrmField("objectidname")]
		public string ObjectIdName { get { return _ObjectIdName; } set { _ObjectIdName=value; AddUpdatedAttribute("objectidname", value); } } // String - ObjectIdName - None

		private DateTime? _EnteredOn;
		[CrmField("enteredon")]
		public DateTime? EnteredOn { get { return _EnteredOn; } set { _EnteredOn=value; AddUpdatedAttribute("enteredon", value); } } // DateTime - EnteredOn - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _ToRecipients;
		[CrmField("torecipients")]
		public string ToRecipients { get { return _ToRecipients; } set { _ToRecipients=value; AddUpdatedAttribute("torecipients", value); } } // String - ToRecipients - None

		private int _Priority;
		[CrmField("priority")]
		public int Priority { get { return _Priority; } set { _Priority=value; AddUpdatedAttribute("priority", value); } } // Integer - Priority - None

		private int _State;
		[CrmField("state")]
		public int State { get { return _State; } set { _State=value; AddUpdatedAttribute("state", value); } } // Integer - State - None

		private DateTime? _WorkerIdModifiedOn;
		[CrmField("workeridmodifiedon")]
		public DateTime? WorkerIdModifiedOn { get { return _WorkerIdModifiedOn; } set { _WorkerIdModifiedOn=value; AddUpdatedAttribute("workeridmodifiedon", value); } } // DateTime - WorkerIdModifiedOn - None

		private int _ObjectTypeCode;
		[CrmField("objecttypecode", IsPicklist = true)]
		public int ObjectTypeCode { get { return _ObjectTypeCode; } set { _ObjectTypeCode=value; AddUpdatedAttribute("objecttypecode", value); } } // Picklist - ObjectTypeCode - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

	}
}
