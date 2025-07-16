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
	[CrmEntity("incident")]
	public class Case : CrmEntity<Case> // 112 - incidentid - title
	{
		private int _Severity;
		[CrmField("azuro_severity", IsPicklist = true)]
		public int Severity { get { return _Severity; } set { _Severity=value; AddUpdatedAttribute("azuro_severity", value); } } // Picklist - azuro_Severity - ApplicationRequired

		private int _NextSeverity;
		[CrmField("azuro_nextseveritylevel", IsPicklist = true)]
		public int NextSeverity { get { return _NextSeverity; } set { _NextSeverity = value; AddUpdatedAttribute("azuro_nextseveritylevel", value); } } // Picklist - azuro_NextSeverity

		private CrmEntityReference _ProductId;
		[CrmField("productid")]
		public CrmEntityReference ProductId { get { return _ProductId; } set { _ProductId=value; AddUpdatedAttribute("productid", value); } } // Lookup - ProductId - None

		private DateTime? _ReassignedDate;
		[CrmField("azuro_reassigneddate")]
		public DateTime? ReassignedDate { get { return _ReassignedDate; } set { _ReassignedDate=value; AddUpdatedAttribute("azuro_reassigneddate", value); } } // DateTime - azuro_ReassignedDate - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private CrmEntityReference _PreviousOwner;
		[CrmField("azuro_previousowner")]
		public CrmEntityReference PreviousOwner { get { return _PreviousOwner; } set { _PreviousOwner=value; AddUpdatedAttribute("azuro_previousowner", value); } } // Lookup - azuro_PreviousOwner - None

		private CrmEntityReference _ResponsibleContactId;
		[CrmField("responsiblecontactid")]
		public CrmEntityReference ResponsibleContactId { get { return _ResponsibleContactId; } set { _ResponsibleContactId=value; AddUpdatedAttribute("responsiblecontactid", value); } } // Lookup - ResponsibleContactId - None

		private string _azuro_previousownername;
		[CrmField("azuro_previousownername")]
		public string azuro_previousownername { get { return _azuro_previousownername; } set { _azuro_previousownername=value; AddUpdatedAttribute("azuro_previousownername", value); } } // String - azuro_PreviousOwnerName - None

		private DateTime? _ResolutionDate;
		[CrmField("azuro_resolutiondate")]
		public DateTime? ResolutionDate { get { return _ResolutionDate; } set { _ResolutionDate=value; AddUpdatedAttribute("azuro_resolutiondate", value); } } // DateTime - azuro_ResolutionDate - None

		private int _IncidentStageCode;
		[CrmField("incidentstagecode", IsPicklist = true)]
		public int IncidentStageCode { get { return _IncidentStageCode; } set { _IncidentStageCode=value; AddUpdatedAttribute("incidentstagecode", value); } } // Picklist - IncidentStageCode - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private bool _IsDecrementing;
		[CrmField("isdecrementing")]
		public bool IsDecrementing { get { return _IsDecrementing; } set { _IsDecrementing=value; AddUpdatedAttribute("isdecrementing", value); } } // Boolean - IsDecrementing - None

		private CrmEntityReference _CustomerId;
		[CrmField("customerid", IsRequired = true)]
		public CrmEntityReference CustomerId { get { return _CustomerId; } set { _CustomerId=value; AddUpdatedAttribute("customerid", value); } } // Customer - CustomerId - SystemRequired

		private int _CustomerSatisfactionCode;
		[CrmField("customersatisfactioncode", IsPicklist = true)]
		public int CustomerSatisfactionCode { get { return _CustomerSatisfactionCode; } set { _CustomerSatisfactionCode=value; AddUpdatedAttribute("customersatisfactioncode", value); } } // Picklist - CustomerSatisfactionCode - None

		private Guid _id;
		[CrmField("incidentid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - IncidentId - SystemRequired

		private string _ResponsibleContactIdYomiName;
		[CrmField("responsiblecontactidyominame")]
		public string ResponsibleContactIdYomiName { get { return _ResponsibleContactIdYomiName; } set { _ResponsibleContactIdYomiName=value; AddUpdatedAttribute("responsiblecontactidyominame", value); } } // String - ResponsibleContactIdYomiName - None

		private CrmEntityReference _ContractId;
		[CrmField("contractid")]
		public CrmEntityReference ContractId { get { return _ContractId; } set { _ContractId=value; AddUpdatedAttribute("contractid", value); } } // Lookup - ContractId - None

		private string _ResponsibleContactIdName;
		[CrmField("responsiblecontactidname")]
		public string ResponsibleContactIdName { get { return _ResponsibleContactIdName; } set { _ResponsibleContactIdName=value; AddUpdatedAttribute("responsiblecontactidname", value); } } // String - ResponsibleContactIdName - None

		private string _CustomerIdType;
		[CrmField("customeridtype")]
		public string CustomerIdType { get { return _CustomerIdType; } set { _CustomerIdType=value; AddUpdatedAttribute("customeridtype", value); } } // EntityName - CustomerIdType - ApplicationRequired

		private int _PriorityCode;
		[CrmField("prioritycode", IsPicklist = true)]
		public int PriorityCode { get { return _PriorityCode; } set { _PriorityCode=value; AddUpdatedAttribute("prioritycode", value); } } // Picklist - PriorityCode - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private DateTime? _FollowupBy;
		[CrmField("followupby")]
		public DateTime? FollowupBy { get { return _FollowupBy; } set { _FollowupBy=value; AddUpdatedAttribute("followupby", value); } } // DateTime - FollowupBy - None

		private CrmEntityReference _ContactId;
		[CrmField("contactid")]
		public CrmEntityReference ContactId { get { return _ContactId; } set { _ContactId=value; AddUpdatedAttribute("contactid", value); } } // Lookup - ContactId - None

		private int _CaseOriginCode;
		[CrmField("caseorigincode", IsPicklist = true)]
		public int CaseOriginCode { get { return _CaseOriginCode; } set { _CaseOriginCode=value; AddUpdatedAttribute("caseorigincode", value); } } // Picklist - CaseOriginCode - None

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private string _ResolutionDescription;
		[CrmField("azuro_resolutiondescription")]
		public string ResolutionDescription { get { return _ResolutionDescription; } set { _ResolutionDescription=value; AddUpdatedAttribute("azuro_resolutiondescription", value); } } // Memo - azuro_ResolutionDescription - None

		private int _ContractServiceLevelCode;
		[CrmField("contractservicelevelcode", IsPicklist = true)]
		public int ContractServiceLevelCode { get { return _ContractServiceLevelCode; } set { _ContractServiceLevelCode=value; AddUpdatedAttribute("contractservicelevelcode", value); } } // Picklist - ContractServiceLevelCode - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private CrmEntityReference _OwningTeam;
		[CrmField("owningteam")]
		public CrmEntityReference OwningTeam { get { return _OwningTeam; } set { _OwningTeam=value; AddUpdatedAttribute("owningteam", value); } } // Lookup - OwningTeam - None

		private string _AccountIdYomiName;
		[CrmField("accountidyominame")]
		public string AccountIdYomiName { get { return _AccountIdYomiName; } set { _AccountIdYomiName=value; AddUpdatedAttribute("accountidyominame", value); } } // String - AccountIdYomiName - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private string _CustomerIdYomiName;
		[CrmField("customeridyominame")]
		public string CustomerIdYomiName { get { return _CustomerIdYomiName; } set { _CustomerIdYomiName=value; AddUpdatedAttribute("customeridyominame", value); } } // String - CustomerIdYomiName - ApplicationRequired

		private string _ContractDetailIdName;
		[CrmField("contractdetailidname")]
		public string ContractDetailIdName { get { return _ContractDetailIdName; } set { _ContractDetailIdName=value; AddUpdatedAttribute("contractdetailidname", value); } } // String - ContractDetailIdName - None

		private string _azuro_reassignedbyname;
		[CrmField("azuro_reassignedbyname")]
		public string azuro_reassignedbyname { get { return _azuro_reassignedbyname; } set { _azuro_reassignedbyname=value; AddUpdatedAttribute("azuro_reassignedbyname", value); } } // String - azuro_ReassignedByName - None

		private string _ContractIdName;
		[CrmField("contractidname")]
		public string ContractIdName { get { return _ContractIdName; } set { _ContractIdName=value; AddUpdatedAttribute("contractidname", value); } } // String - ContractIdName - None

		private CrmEntityReference _ContractDetailId;
		[CrmField("contractdetailid")]
		public CrmEntityReference ContractDetailId { get { return _ContractDetailId; } set { _ContractDetailId=value; AddUpdatedAttribute("contractdetailid", value); } } // Lookup - ContractDetailId - None

		private int _OriginalSeverity;
		[CrmField("azuro_originalseverity", IsPicklist = true)]
		public int OriginalSeverity { get { return _OriginalSeverity; } set { _OriginalSeverity=value; AddUpdatedAttribute("azuro_originalseverity", value); } } // Picklist - azuro_OriginalSeverity - None

		private CrmEntityReference _ExistingCase;
		[CrmField("existingcase")]
		public CrmEntityReference ExistingCase { get { return _ExistingCase; } set { _ExistingCase=value; AddUpdatedAttribute("existingcase", value); } } // Lookup - ExistingCase - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private int _ServiceStage;
		[CrmField("servicestage", IsPicklist = true)]
		public int ServiceStage { get { return _ServiceStage; } set { _ServiceStage=value; AddUpdatedAttribute("servicestage", value); } } // Picklist - ServiceStage - None

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private string _azuro_reassignedbyyominame;
		[CrmField("azuro_reassignedbyyominame")]
		public string azuro_reassignedbyyominame { get { return _azuro_reassignedbyyominame; } set { _azuro_reassignedbyyominame=value; AddUpdatedAttribute("azuro_reassignedbyyominame", value); } } // String - azuro_ReassignedByYomiName - None

		private CrmEntityReference _KbArticleId;
		[CrmField("kbarticleid")]
		public CrmEntityReference KbArticleId { get { return _KbArticleId; } set { _KbArticleId=value; AddUpdatedAttribute("kbarticleid", value); } } // Lookup - KbArticleId - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid", IsRequired = true)]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - SystemRequired

		private string _ContactIdYomiName;
		[CrmField("contactidyominame")]
		public string ContactIdYomiName { get { return _ContactIdYomiName; } set { _ContactIdYomiName=value; AddUpdatedAttribute("contactidyominame", value); } } // String - ContactIdYomiName - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _OwnerIdYomiName;
		[CrmField("owneridyominame", IsRequired = true)]
		public string OwnerIdYomiName { get { return _OwnerIdYomiName; } set { _OwnerIdYomiName=value; AddUpdatedAttribute("owneridyominame", value); } } // String - OwnerIdYomiName - SystemRequired

		private string _ProductSerialNumber;
		[CrmField("productserialnumber")]
		public string ProductSerialNumber { get { return _ProductSerialNumber; } set { _ProductSerialNumber=value; AddUpdatedAttribute("productserialnumber", value); } } // String - ProductSerialNumber - None

		private string _KbArticleIdName;
		[CrmField("kbarticleidname")]
		public string KbArticleIdName { get { return _KbArticleIdName; } set { _KbArticleIdName=value; AddUpdatedAttribute("kbarticleidname", value); } } // String - KbArticleIdName - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private string _Description;
		[CrmField("description")]
		public string Description { get { return _Description; } set { _Description=value; AddUpdatedAttribute("description", value); } } // Memo - Description - None

		private string _ExternalSystemReference;
		[CrmField("azuro_externalsystemreference")]
		public string ExternalSystemReference { get { return _ExternalSystemReference; } set { _ExternalSystemReference=value; AddUpdatedAttribute("azuro_externalsystemreference", value); } } // String - azuro_ExternalSystemReference - None

		private string _CustomerIdName;
		[CrmField("customeridname")]
		public string CustomerIdName { get { return _CustomerIdName; } set { _CustomerIdName=value; AddUpdatedAttribute("customeridname", value); } } // String - CustomerIdName - ApplicationRequired

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private int _CaseTypeCode;
		[CrmField("casetypecode", IsPicklist = true)]
		public int CaseTypeCode { get { return _CaseTypeCode; } set { _CaseTypeCode=value; AddUpdatedAttribute("casetypecode", value); } } // Picklist - CaseTypeCode - None

		private int _ActualServiceUnits;
		[CrmField("actualserviceunits")]
		public int ActualServiceUnits { get { return _ActualServiceUnits; } set { _ActualServiceUnits=value; AddUpdatedAttribute("actualserviceunits", value); } } // Integer - ActualServiceUnits - None

		private DateTime? _EscalationDate;
		[CrmField("azuro_escalationdate")]
		public DateTime? EscalationDate { get { return _EscalationDate; } set { _EscalationDate=value; AddUpdatedAttribute("azuro_escalationdate", value); } } // DateTime - azuro_Escalationdate - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - None

		private string _SubjectIdName;
		[CrmField("subjectidname")]
		public string SubjectIdName { get { return _SubjectIdName; } set { _SubjectIdName=value; AddUpdatedAttribute("subjectidname", value); } } // String - SubjectIdName - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private CrmEntityReference _AccountId;
		[CrmField("accountid")]
		public CrmEntityReference AccountId { get { return _AccountId; } set { _AccountId=value; AddUpdatedAttribute("accountid", value); } } // Lookup - AccountId - None

		private string _OwnerIdName;
		[CrmField("owneridname", IsRequired = true)]
		public string OwnerIdName { get { return _OwnerIdName; } set { _OwnerIdName=value; AddUpdatedAttribute("owneridname", value); } } // String - OwnerIdName - SystemRequired

		private string _TicketNumber;
		[CrmField("ticketnumber")]
		public string TicketNumber { get { return _TicketNumber; } set { _TicketNumber=value; AddUpdatedAttribute("ticketnumber", value); } } // String - TicketNumber - None

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

		private string _ContactIdName;
		[CrmField("contactidname")]
		public string ContactIdName { get { return _ContactIdName; } set { _ContactIdName=value; AddUpdatedAttribute("contactidname", value); } } // String - ContactIdName - None

		private int _SeverityCode;
		[CrmField("severitycode", IsPicklist = true)]
		public int SeverityCode { get { return _SeverityCode; } set { _SeverityCode=value; AddUpdatedAttribute("severitycode", value); } } // Picklist - SeverityCode - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _Title;
		[CrmField("title")]
		public string Title { get { return _Title; } set { _Title=value; AddUpdatedAttribute("title", value); } } // String - Title - ApplicationRequired

		private string _ProductIdName;
		[CrmField("productidname")]
		public string ProductIdName { get { return _ProductIdName; } set { _ProductIdName=value; AddUpdatedAttribute("productidname", value); } } // String - ProductIdName - None

		private int _StatusCode;
		[CrmField("statuscode", IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - ApplicationRequired

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private string _AccountIdName;
		[CrmField("accountidname")]
		public string AccountIdName { get { return _AccountIdName; } set { _AccountIdName=value; AddUpdatedAttribute("accountidname", value); } } // String - AccountIdName - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private CrmEntityReference _ReassignedBy;
		[CrmField("azuro_reassignedby")]
		public CrmEntityReference ReassignedBy { get { return _ReassignedBy; } set { _ReassignedBy=value; AddUpdatedAttribute("azuro_reassignedby", value); } } // Lookup - azuro_ReassignedBy - None

		private int _BilledServiceUnits;
		[CrmField("billedserviceunits")]
		public int BilledServiceUnits { get { return _BilledServiceUnits; } set { _BilledServiceUnits=value; AddUpdatedAttribute("billedserviceunits", value); } } // Integer - BilledServiceUnits - None

		private string _azuro_previousowneryominame;
		[CrmField("azuro_previousowneryominame")]
		public string azuro_previousowneryominame { get { return _azuro_previousowneryominame; } set { _azuro_previousowneryominame=value; AddUpdatedAttribute("azuro_previousowneryominame", value); } } // String - azuro_PreviousOwnerYomiName - None

		private CrmEntityReference _SubjectId;
		[CrmField("subjectid")]
		public CrmEntityReference SubjectId { get { return _SubjectId; } set { _SubjectId=value; AddUpdatedAttribute("subjectid", value); } } // Lookup - SubjectId - None

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None
	}
}
