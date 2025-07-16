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
	[CrmEntity("azuro_notificationcommunication")]
	public class NotificationCommunication : CrmEntity<NotificationCommunication> // 10011 - azuro_notificationcommunicationid - azuro_name
	{
		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame", IsRequired = true)]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName = value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - SystemRequired

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy = value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame", IsRequired = true)]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName = value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - SystemRequired

		private int _statecode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int statecode { get { return _statecode; } set { _statecode = value; AddUpdatedAttribute("statecode", value); } } // State - statecode - SystemRequired

		private string _OwnerIdName;
		[CrmField("owneridname", IsRequired = true)]
		public string OwnerIdName { get { return _OwnerIdName; } set { _OwnerIdName = value; AddUpdatedAttribute("owneridname", value); } } // String - OwnerIdName - SystemRequired

		private string _Message;
		[CrmField("azuro_message")]
		public string Message { get { return _Message; } set { _Message = value; AddUpdatedAttribute("azuro_message", value); } } // Memo - azuro_Message - None

		private int _statuscode;
		[CrmField("statuscode", IsPicklist = true)]
		public int statuscode { get { return _statuscode; } set { _statuscode = value; AddUpdatedAttribute("statuscode", value); } } // Status - statuscode - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy = value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private bool _SendtoOwner;
		[CrmField("azuro_sendtoowner")]
		public bool SendtoOwner { get { return _SendtoOwner; } set { _SendtoOwner = value; AddUpdatedAttribute("azuro_sendtoowner", value); } } // Boolean - azuro_SendtoOwner - None

		private bool _SendfromOwner;
		[CrmField("azuro_sendfromowner")]
		public bool SendfromOwner { get { return _SendfromOwner; } set { _SendfromOwner = value; AddUpdatedAttribute("azuro_sendfromowner", value); } }

		private CrmEntityReference _SendFrom;
		[CrmField("azuro_sendfrom")]
		public CrmEntityReference SendFrom { get { return _SendFrom; } set { _SendFrom = value; AddUpdatedAttribute("azuro_sendfrom", value); } }

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser = value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber = value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private Guid _id;
		[CrmField("azuro_notificationcommunicationid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - azuro_notificationcommunicationId - SystemRequired

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode = value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private string _EmailTemplateId;
		[CrmField("azuro_emailtemplateid")]
		public string EmailTemplateId { get { return _EmailTemplateId; } set { _EmailTemplateId = value; AddUpdatedAttribute("azuro_emailtemplateid", value); } } // String - azuro_EmailTemplateId - None

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit = value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - None

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName = value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private CrmEntityReference _OwningTeam;
		[CrmField("owningteam")]
		public CrmEntityReference OwningTeam { get { return _OwningTeam; } set { _OwningTeam = value; AddUpdatedAttribute("owningteam", value); } } // Lookup - OwningTeam - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy = value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy = value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber = value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private int _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public int OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType = value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private string _Subject;
		[CrmField("azuro_subject")]
		public string Subject { get { return _Subject; } set { _Subject = value; AddUpdatedAttribute("azuro_subject", value); } } // String - azuro_Subject - ApplicationRequired

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber = value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private int _Event;
		[CrmField("azuro_event", IsPicklist = true)]
		public int Event { get { return _Event; } set { _Event = value; AddUpdatedAttribute("azuro_event", value); } } // Picklist - azuro_Event - ApplicationRequired

		private string _OwnerIdYomiName;
		[CrmField("owneridyominame", IsRequired = true)]
		public string OwnerIdYomiName { get { return _OwnerIdYomiName; } set { _OwnerIdYomiName = value; AddUpdatedAttribute("owneridyominame", value); } } // String - OwnerIdYomiName - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn = value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame", IsRequired = true)]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName = value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - SystemRequired

		private int _Channel;
		[CrmField("azuro_channel", IsPicklist = true)]
		public int Channel { get { return _Channel; } set { _Channel = value; AddUpdatedAttribute("azuro_channel", value); } } // Picklist - azuro_Channel - ApplicationRequired

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame", IsRequired = true)]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName = value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - SystemRequired

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName = value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn = value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName = value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _Name;
		[CrmField("azuro_name")]
		public string Name { get { return _Name; } set { _Name = value; AddUpdatedAttribute("azuro_name", value); } } // String - azuro_name - ApplicationRequired

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName = value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private bool _SendtoRequestor;
		[CrmField("azuro_sendtorequestor")]
		public bool SendtoRequestor { get { return _SendtoRequestor; } set { _SendtoRequestor = value; AddUpdatedAttribute("azuro_sendtorequestor", value); } } // Boolean - azuro_SendtoRequestor - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid", IsRequired = true)]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId = value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - SystemRequired

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn = value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private bool? _ApplicableToAllCaseOrigins;
		[CrmField("azuro_applicabletoallcaseorigins")]
		public bool? ApplicableToAllCaseOrigins { get { return _ApplicableToAllCaseOrigins; } set { _ApplicableToAllCaseOrigins = value; AddUpdatedAttribute("azuro_applicabletoallcaseorigins", value); } } // bool - ApplicableToAllCaseOrigins - None

		private string _ApplicableCaseOrigins;
		[CrmField("azuro_applicablecaseoriginmulti")]
		public string ApplicableCaseOrigins { get { return _ApplicableCaseOrigins; } set { _ApplicableCaseOrigins = value; AddUpdatedAttribute("azuro_applicablecaseoriginmulti", value); } } // string - ApplicableCaseOrigins - None

		private bool? _ValidForAllSeverities;
		[CrmField("azuro_validforallseverities")]
		public bool? ValidForAllSeverities { get { return _ValidForAllSeverities; } set { _ValidForAllSeverities = value; AddUpdatedAttribute("azuro_validforallseverities", value); } } // bool - ValidForAllSeverities - None

		private int? _Severity;
		[CrmField("azuro_severity")]
		public int? Severity { get { return _Severity; } set { _Severity = value; AddUpdatedAttribute("azuro_severity", value); } } // int? - Severity - None
	}
}
