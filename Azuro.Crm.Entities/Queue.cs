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
	[CrmEntity("queue")]
	public class Queue : CrmEntity<Queue> // 2020 - queueid - name
	{
		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private int _EmailRouterAccessApproval;
		[CrmField("emailrouteraccessapproval", IsRequired = true, IsPicklist = true)]
		public int EmailRouterAccessApproval { get { return _EmailRouterAccessApproval; } set { _EmailRouterAccessApproval=value; AddUpdatedAttribute("emailrouteraccessapproval", value); } } // Picklist - EmailRouterAccessApproval - SystemRequired

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private string _EmailPassword;
		[CrmField("emailpassword")]
		public string EmailPassword { get { return _EmailPassword; } set { _EmailPassword=value; AddUpdatedAttribute("emailpassword", value); } } // String - EmailPassword - None

		private string _OwnerIdName;
		[CrmField("owneridname", IsRequired = true)]
		public string OwnerIdName { get { return _OwnerIdName; } set { _OwnerIdName=value; AddUpdatedAttribute("owneridname", value); } } // String - OwnerIdName - SystemRequired

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private string _PrimaryUserIdName;
		[CrmField("primaryuseridname")]
		public string PrimaryUserIdName { get { return _PrimaryUserIdName; } set { _PrimaryUserIdName=value; AddUpdatedAttribute("primaryuseridname", value); } } // String - PrimaryUserIdName - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private int _QueueTypeCode;
		[CrmField("queuetypecode", IsPicklist = true)]
		public int QueueTypeCode { get { return _QueueTypeCode; } set { _QueueTypeCode=value; AddUpdatedAttribute("queuetypecode", value); } } // Picklist - QueueTypeCode - None

		private CrmEntityReference _OwningTeam;
		[CrmField("owningteam")]
		public CrmEntityReference OwningTeam { get { return _OwningTeam; } set { _OwningTeam=value; AddUpdatedAttribute("owningteam", value); } } // Lookup - OwningTeam - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private string _OrganizationIdName;
		[CrmField("organizationidname", IsRequired = true)]
		public string OrganizationIdName { get { return _OrganizationIdName; } set { _OrganizationIdName=value; AddUpdatedAttribute("organizationidname", value); } } // String - OrganizationIdName - SystemRequired

		private bool _AllowEmailCredentials;
		[CrmField("allowemailcredentials", IsRequired = true)]
		public bool AllowEmailCredentials { get { return _AllowEmailCredentials; } set { _AllowEmailCredentials=value; AddUpdatedAttribute("allowemailcredentials", value); } } // Boolean - AllowEmailCredentials - SystemRequired

		private string _EMailAddress;
		[CrmField("emailaddress")]
		public string EMailAddress { get { return _EMailAddress; } set { _EMailAddress=value; AddUpdatedAttribute("emailaddress", value); } } // String - EMailAddress - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private bool _IsFaxQueue;
		[CrmField("isfaxqueue")]
		public bool IsFaxQueue { get { return _IsFaxQueue; } set { _IsFaxQueue=value; AddUpdatedAttribute("isfaxqueue", value); } } // Boolean - IsFaxQueue - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid", IsRequired = true)]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _OwnerIdYomiName;
		[CrmField("owneridyominame", IsRequired = true)]
		public string OwnerIdYomiName { get { return _OwnerIdYomiName; } set { _OwnerIdYomiName=value; AddUpdatedAttribute("owneridyominame", value); } } // String - OwnerIdYomiName - SystemRequired

		private bool _IgnoreUnsolicitedEmail;
		[CrmField("ignoreunsolicitedemail")]
		public bool IgnoreUnsolicitedEmail { get { return _IgnoreUnsolicitedEmail; } set { _IgnoreUnsolicitedEmail=value; AddUpdatedAttribute("ignoreunsolicitedemail", value); } } // Boolean - IgnoreUnsolicitedEmail - None

		private string _Description;
		[CrmField("description")]
		public string Description { get { return _Description; } set { _Description=value; AddUpdatedAttribute("description", value); } } // Memo - Description - None

		private Guid _id;
		[CrmField("queueid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - QueueId - SystemRequired

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private string _PrimaryUserIdYomiName;
		[CrmField("primaryuseridyominame")]
		public string PrimaryUserIdYomiName { get { return _PrimaryUserIdYomiName; } set { _PrimaryUserIdYomiName=value; AddUpdatedAttribute("primaryuseridyominame", value); } } // String - PrimaryUserIdYomiName - None

		private string _BusinessUnitIdName;
		[CrmField("businessunitidname")]
		public string BusinessUnitIdName { get { return _BusinessUnitIdName; } set { _BusinessUnitIdName=value; AddUpdatedAttribute("businessunitidname", value); } } // String - BusinessUnitIdName - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private int _IncomingEmailDeliveryMethod;
		[CrmField("incomingemaildeliverymethod", IsRequired = true, IsPicklist = true)]
		public int IncomingEmailDeliveryMethod { get { return _IncomingEmailDeliveryMethod; } set { _IncomingEmailDeliveryMethod=value; AddUpdatedAttribute("incomingemaildeliverymethod", value); } } // Picklist - IncomingEmailDeliveryMethod - SystemRequired

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - None

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - None

		private int _IncomingEmailFilteringMethod;
		[CrmField("incomingemailfilteringmethod", IsRequired = true, IsPicklist = true)]
		public int IncomingEmailFilteringMethod { get { return _IncomingEmailFilteringMethod; } set { _IncomingEmailFilteringMethod=value; AddUpdatedAttribute("incomingemailfilteringmethod", value); } } // Picklist - IncomingEmailFilteringMethod - SystemRequired

		private string _Name;
		[CrmField("name")]
		public string Name { get { return _Name; } set { _Name=value; AddUpdatedAttribute("name", value); } } // String - Name - ApplicationRequired

		private int _StatusCode;
		[CrmField("statuscode", IsRequired = true, IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - SystemRequired

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private CrmEntityReference _OrganizationId;
		[CrmField("organizationid", IsRequired = true)]
		public CrmEntityReference OrganizationId { get { return _OrganizationId; } set { _OrganizationId=value; AddUpdatedAttribute("organizationid", value); } } // Lookup - OrganizationId - SystemRequired

		private int _OutgoingEmailDeliveryMethod;
		[CrmField("outgoingemaildeliverymethod", IsRequired = true, IsPicklist = true)]
		public int OutgoingEmailDeliveryMethod { get { return _OutgoingEmailDeliveryMethod; } set { _OutgoingEmailDeliveryMethod=value; AddUpdatedAttribute("outgoingemaildeliverymethod", value); } } // Picklist - OutgoingEmailDeliveryMethod - SystemRequired

		private CrmEntityReference _BusinessUnitId;
		[CrmField("businessunitid")]
		public CrmEntityReference BusinessUnitId { get { return _BusinessUnitId; } set { _BusinessUnitId=value; AddUpdatedAttribute("businessunitid", value); } } // Lookup - BusinessUnitId - None

		private string _EmailUsername;
		[CrmField("emailusername")]
		public string EmailUsername { get { return _EmailUsername; } set { _EmailUsername=value; AddUpdatedAttribute("emailusername", value); } } // String - EmailUsername - None

		private CrmEntityReference _PrimaryUserId;
		[CrmField("primaryuserid")]
		public CrmEntityReference PrimaryUserId { get { return _PrimaryUserId; } set { _PrimaryUserId=value; AddUpdatedAttribute("primaryuserid", value); } } // Lookup - PrimaryUserId - None

		private string _TransactionCurrencyIdName;
		[CrmField("transactioncurrencyidname")]
		public string TransactionCurrencyIdName { get { return _TransactionCurrencyIdName; } set { _TransactionCurrencyIdName=value; AddUpdatedAttribute("transactioncurrencyidname", value); } } // String - TransactionCurrencyIdName - None

	}
}
