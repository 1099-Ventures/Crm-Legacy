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
	[CrmEntity("contract")]
	public class Contract : CrmEntity<Contract> // 1010 - contractid - title
	{
		private int _AllotmentTypeCode;
		[CrmField("allotmenttypecode", IsPicklist = true)]
		public int AllotmentTypeCode { get { return _AllotmentTypeCode; } set { _AllotmentTypeCode=value; AddUpdatedAttribute("allotmenttypecode", value); } } // Picklist - AllotmentTypeCode - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private CrmEntityReference _OriginatingContract;
		[CrmField("originatingcontract")]
		public CrmEntityReference OriginatingContract { get { return _OriginatingContract; } set { _OriginatingContract=value; AddUpdatedAttribute("originatingcontract", value); } } // Lookup - OriginatingContract - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private CrmEntityReference _ContractTemplateId;
		[CrmField("contracttemplateid", IsRequired = true)]
		public CrmEntityReference ContractTemplateId { get { return _ContractTemplateId; } set { _ContractTemplateId=value; AddUpdatedAttribute("contracttemplateid", value); } } // Lookup - ContractTemplateId - SystemRequired

		private DateTime? _CancelOn;
		[CrmField("cancelon")]
		public DateTime? CancelOn { get { return _CancelOn; } set { _CancelOn=value; AddUpdatedAttribute("cancelon", value); } } // DateTime - CancelOn - None

		private string _OriginatingContractName;
		[CrmField("originatingcontractname")]
		public string OriginatingContractName { get { return _OriginatingContractName; } set { _OriginatingContractName=value; AddUpdatedAttribute("originatingcontractname", value); } } // String - OriginatingContractName - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private CrmEntityReference _ContactId;
		[CrmField("contactid")]
		public CrmEntityReference ContactId { get { return _ContactId; } set { _ContactId=value; AddUpdatedAttribute("contactid", value); } } // Lookup - ContactId - None

		private string _Title;
		[CrmField("title")]
		public string Title { get { return _Title; } set { _Title=value; AddUpdatedAttribute("title", value); } } // String - Title - ApplicationRequired

		private bool _UseDiscountAsPercentage;
		[CrmField("usediscountaspercentage")]
		public bool UseDiscountAsPercentage { get { return _UseDiscountAsPercentage; } set { _UseDiscountAsPercentage=value; AddUpdatedAttribute("usediscountaspercentage", value); } } // Boolean - UseDiscountAsPercentage - None

		private CrmEntityReference _CustomerId;
		[CrmField("customerid", IsRequired = true)]
		public CrmEntityReference CustomerId { get { return _CustomerId; } set { _CustomerId=value; AddUpdatedAttribute("customerid", value); } } // Customer - CustomerId - SystemRequired

		private Guid _id;
		[CrmField("contractid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - ContractId - SystemRequired

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private int _DefaultSeverity;
		[CrmField("azuro_defaultseverity", IsPicklist = true)]
		public int DefaultSeverity { get { return _DefaultSeverity; } set { _DefaultSeverity=value; AddUpdatedAttribute("azuro_defaultseverity", value); } } // Picklist - azuro_DefaultSeverity - Recommended

		private decimal _NetPrice;
		[CrmField("netprice")]
		public decimal NetPrice { get { return _NetPrice; } set { _NetPrice=value; AddUpdatedAttribute("netprice", value); } } // Money - NetPrice - None

		private string _OwnerIdName;
		[CrmField("owneridname", IsRequired = true)]
		public string OwnerIdName { get { return _OwnerIdName; } set { _OwnerIdName=value; AddUpdatedAttribute("owneridname", value); } } // String - OwnerIdName - SystemRequired

		private string _azuro_escalationgroupname;
		[CrmField("azuro_escalationgroupname")]
		public string azuro_escalationgroupname { get { return _azuro_escalationgroupname; } set { _azuro_escalationgroupname=value; AddUpdatedAttribute("azuro_escalationgroupname", value); } } // String - azuro_EscalationGroupName - None

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber=value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private string _BillingCustomerIdName;
		[CrmField("billingcustomeridname")]
		public string BillingCustomerIdName { get { return _BillingCustomerIdName; } set { _BillingCustomerIdName=value; AddUpdatedAttribute("billingcustomeridname", value); } } // String - BillingCustomerIdName - ApplicationRequired

		private CrmEntityReference _AccountId;
		[CrmField("accountid")]
		public CrmEntityReference AccountId { get { return _AccountId; } set { _AccountId=value; AddUpdatedAttribute("accountid", value); } } // Lookup - AccountId - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private string _ContractLanguage;
		[CrmField("contractlanguage")]
		public string ContractLanguage { get { return _ContractLanguage; } set { _ContractLanguage=value; AddUpdatedAttribute("contractlanguage", value); } } // Memo - ContractLanguage - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private int _ContractServiceLevelCode;
		[CrmField("contractservicelevelcode", IsPicklist = true)]
		public int ContractServiceLevelCode { get { return _ContractServiceLevelCode; } set { _ContractServiceLevelCode=value; AddUpdatedAttribute("contractservicelevelcode", value); } } // Picklist - ContractServiceLevelCode - None

		private string _BillingAccountIdName;
		[CrmField("billingaccountidname")]
		public string BillingAccountIdName { get { return _BillingAccountIdName; } set { _BillingAccountIdName=value; AddUpdatedAttribute("billingaccountidname", value); } } // String - BillingAccountIdName - None

		private CrmEntityReference _OwningTeam;
		[CrmField("owningteam")]
		public CrmEntityReference OwningTeam { get { return _OwningTeam; } set { _OwningTeam=value; AddUpdatedAttribute("owningteam", value); } } // Lookup - OwningTeam - None

		private string _BillingCustomerIdYomiName;
		[CrmField("billingcustomeridyominame")]
		public string BillingCustomerIdYomiName { get { return _BillingCustomerIdYomiName; } set { _BillingCustomerIdYomiName=value; AddUpdatedAttribute("billingcustomeridyominame", value); } } // String - BillingCustomerIdYomiName - ApplicationRequired

		private string _ContactIdName;
		[CrmField("contactidname")]
		public string ContactIdName { get { return _ContactIdName; } set { _ContactIdName=value; AddUpdatedAttribute("contactidname", value); } } // String - ContactIdName - None

		private string _BillingAccountIdYomiName;
		[CrmField("billingaccountidyominame")]
		public string BillingAccountIdYomiName { get { return _BillingAccountIdYomiName; } set { _BillingAccountIdYomiName=value; AddUpdatedAttribute("billingaccountidyominame", value); } } // String - BillingAccountIdYomiName - None

		private DateTime _ExpiresOn;
		[CrmField("expireson", IsRequired = true)]
		public DateTime ExpiresOn { get { return _ExpiresOn; } set { _ExpiresOn=value; AddUpdatedAttribute("expireson", value); } } // DateTime - ExpiresOn - SystemRequired

		private string _azuro_contactname;
		[CrmField("azuro_contactname")]
		public string azuro_contactname { get { return _azuro_contactname; } set { _azuro_contactname=value; AddUpdatedAttribute("azuro_contactname", value); } } // String - azuro_ContactName - None

		private string _TransactionCurrencyIdName;
		[CrmField("transactioncurrencyidname")]
		public string TransactionCurrencyIdName { get { return _TransactionCurrencyIdName; } set { _TransactionCurrencyIdName=value; AddUpdatedAttribute("transactioncurrencyidname", value); } } // String - TransactionCurrencyIdName - None

		private string _AccountIdYomiName;
		[CrmField("accountidyominame")]
		public string AccountIdYomiName { get { return _AccountIdYomiName; } set { _AccountIdYomiName=value; AddUpdatedAttribute("accountidyominame", value); } } // String - AccountIdYomiName - None

		private CrmEntityReference _BillingContactId;
		[CrmField("billingcontactid")]
		public CrmEntityReference BillingContactId { get { return _BillingContactId; } set { _BillingContactId=value; AddUpdatedAttribute("billingcontactid", value); } } // Lookup - BillingContactId - None

		private string _CustomerIdYomiName;
		[CrmField("customeridyominame")]
		public string CustomerIdYomiName { get { return _CustomerIdYomiName; } set { _CustomerIdYomiName=value; AddUpdatedAttribute("customeridyominame", value); } } // String - CustomerIdYomiName - ApplicationRequired

		private CrmEntityReference _BillingAccountId;
		[CrmField("billingaccountid")]
		public CrmEntityReference BillingAccountId { get { return _BillingAccountId; } set { _BillingAccountId=value; AddUpdatedAttribute("billingaccountid", value); } } // Lookup - BillingAccountId - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private DateTime _ActiveOn;
		[CrmField("activeon", IsRequired = true)]
		public DateTime ActiveOn { get { return _ActiveOn; } set { _ActiveOn=value; AddUpdatedAttribute("activeon", value); } } // DateTime - ActiveOn - SystemRequired

		private string _azuro_contactyominame;
		[CrmField("azuro_contactyominame")]
		public string azuro_contactyominame { get { return _azuro_contactyominame; } set { _azuro_contactyominame=value; AddUpdatedAttribute("azuro_contactyominame", value); } } // String - azuro_ContactYomiName - None

		private DateTime? _BillingStartOn;
		[CrmField("billingstarton")]
		public DateTime? BillingStartOn { get { return _BillingStartOn; } set { _BillingStartOn=value; AddUpdatedAttribute("billingstarton", value); } } // DateTime - BillingStartOn - None

		private CrmEntityReference _BillToAddress;
		[CrmField("billtoaddress")]
		public CrmEntityReference BillToAddress { get { return _BillToAddress; } set { _BillToAddress=value; AddUpdatedAttribute("billtoaddress", value); } } // Lookup - BillToAddress - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private int _AllotmentQty;
		[CrmField("azuro_qty")]
		public int AllotmentQty { get { return _AllotmentQty; } set { _AllotmentQty=value; AddUpdatedAttribute("azuro_qty", value); } } // Integer - azuro_Qty - Recommended

		private string _ContractTemplateAbbreviation;
		[CrmField("contracttemplateabbreviation", IsRequired = true)]
		public string ContractTemplateAbbreviation { get { return _ContractTemplateAbbreviation; } set { _ContractTemplateAbbreviation=value; AddUpdatedAttribute("contracttemplateabbreviation", value); } } // String - ContractTemplateAbbreviation - SystemRequired

		private string _BillingContactIdYomiName;
		[CrmField("billingcontactidyominame")]
		public string BillingContactIdYomiName { get { return _BillingContactIdYomiName; } set { _BillingContactIdYomiName=value; AddUpdatedAttribute("billingcontactidyominame", value); } } // String - BillingContactIdYomiName - None

		private bool _Rollover;
		[CrmField("azuro_rollover")]
		public bool Rollover { get { return _Rollover; } set { _Rollover=value; AddUpdatedAttribute("azuro_rollover", value); } } // Boolean - azuro_Rollover - None

		private CrmEntityReference _ContractManager;
		[CrmField("azuro_contact")]
		public CrmEntityReference ContractManager { get { return _ContractManager; } set { _ContractManager=value; AddUpdatedAttribute("azuro_contact", value); } } // Lookup - azuro_Contact - Recommended

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

		private int _Duration;
		[CrmField("duration")]
		public int Duration { get { return _Duration; } set { _Duration=value; AddUpdatedAttribute("duration", value); } } // Integer - Duration - None

		private string _CustomerIdName;
		[CrmField("customeridname")]
		public string CustomerIdName { get { return _CustomerIdName; } set { _CustomerIdName=value; AddUpdatedAttribute("customeridname", value); } } // String - CustomerIdName - ApplicationRequired

		private decimal _TotalDiscount;
		[CrmField("totaldiscount")]
		public decimal TotalDiscount { get { return _TotalDiscount; } set { _TotalDiscount=value; AddUpdatedAttribute("totaldiscount", value); } } // Money - TotalDiscount - None

		private int _RolloverExpiry;
		[CrmField("azuro_rolloverexpiry", IsPicklist = true)]
		public int RolloverExpiry { get { return _RolloverExpiry; } set { _RolloverExpiry=value; AddUpdatedAttribute("azuro_rolloverexpiry", value); } } // Picklist - azuro_RolloverExpiry - None

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private string _BillingCustomerIdType;
		[CrmField("billingcustomeridtype")]
		public string BillingCustomerIdType { get { return _BillingCustomerIdType; } set { _BillingCustomerIdType=value; AddUpdatedAttribute("billingcustomeridtype", value); } } // EntityName - BillingCustomerIdType - ApplicationRequired

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private string _EffectivityCalendar;
		[CrmField("effectivitycalendar")]
		public string EffectivityCalendar { get { return _EffectivityCalendar; } set { _EffectivityCalendar=value; AddUpdatedAttribute("effectivitycalendar", value); } } // String - EffectivityCalendar - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - ApplicationRequired

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private string _ContractTemplateIdName;
		[CrmField("contracttemplateidname", IsRequired = true)]
		public string ContractTemplateIdName { get { return _ContractTemplateIdName; } set { _ContractTemplateIdName=value; AddUpdatedAttribute("contracttemplateidname", value); } } // String - ContractTemplateIdName - SystemRequired

		private CrmEntityReference _EscalationGroup;
		[CrmField("azuro_escalationgroup")]
		public CrmEntityReference EscalationGroup { get { return _EscalationGroup; } set { _EscalationGroup=value; AddUpdatedAttribute("azuro_escalationgroup", value); } } // Lookup - azuro_EscalationGroup - None

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - None

		private float _RolloverPercentage;
		[CrmField("azuro_rolloverperc")]
		public float RolloverPercentage { get { return _RolloverPercentage; } set { _RolloverPercentage=value; AddUpdatedAttribute("azuro_rolloverperc", value); } } // Decimal - azuro_Rolloverperc - None

		private int _ContractType;
		[CrmField("azuro_contracttype", IsPicklist = true)]
		public int ContractType { get { return _ContractType; } set { _ContractType=value; AddUpdatedAttribute("azuro_contracttype", value); } } // Picklist - azuro_ContractType - None

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - None

		private string _BillToAddressName;
		[CrmField("billtoaddressname")]
		public string BillToAddressName { get { return _BillToAddressName; } set { _BillToAddressName=value; AddUpdatedAttribute("billtoaddressname", value); } } // String - BillToAddressName - None

		private int _BillingFrequencyCode;
		[CrmField("billingfrequencycode", IsPicklist = true)]
		public int BillingFrequencyCode { get { return _BillingFrequencyCode; } set { _BillingFrequencyCode=value; AddUpdatedAttribute("billingfrequencycode", value); } } // Picklist - BillingFrequencyCode - None

		private int _StatusCode;
		[CrmField("statuscode", IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private DateTime? _BillingEndOn;
		[CrmField("billingendon")]
		public DateTime? BillingEndOn { get { return _BillingEndOn; } set { _BillingEndOn=value; AddUpdatedAttribute("billingendon", value); } } // DateTime - BillingEndOn - None

		private CrmEntityReference _BillingCustomerId;
		[CrmField("billingcustomerid", IsRequired = true)]
		public CrmEntityReference BillingCustomerId { get { return _BillingCustomerId; } set { _BillingCustomerId=value; AddUpdatedAttribute("billingcustomerid", value); } } // Customer - BillingCustomerId - SystemRequired

		private string _AccountIdName;
		[CrmField("accountidname")]
		public string AccountIdName { get { return _AccountIdName; } set { _AccountIdName=value; AddUpdatedAttribute("accountidname", value); } } // String - AccountIdName - None

		private CrmEntityReference _ServiceAddress;
		[CrmField("serviceaddress")]
		public CrmEntityReference ServiceAddress { get { return _ServiceAddress; } set { _ServiceAddress=value; AddUpdatedAttribute("serviceaddress", value); } } // Lookup - ServiceAddress - None

		private string _CustomerIdType;
		[CrmField("customeridtype")]
		public string CustomerIdType { get { return _CustomerIdType; } set { _CustomerIdType=value; AddUpdatedAttribute("customeridtype", value); } } // EntityName - CustomerIdType - ApplicationRequired

		private decimal _TotalPrice;
		[CrmField("totalprice")]
		public decimal TotalPrice { get { return _TotalPrice; } set { _TotalPrice=value; AddUpdatedAttribute("totalprice", value); } } // Money - TotalPrice - None

		private string _ContractNumber;
		[CrmField("contractnumber")]
		public string ContractNumber { get { return _ContractNumber; } set { _ContractNumber=value; AddUpdatedAttribute("contractnumber", value); } } // String - ContractNumber - None

		private string _ServiceAddressName;
		[CrmField("serviceaddressname")]
		public string ServiceAddressName { get { return _ServiceAddressName; } set { _ServiceAddressName=value; AddUpdatedAttribute("serviceaddressname", value); } } // String - ServiceAddressName - None

		private string _BillingContactIdName;
		[CrmField("billingcontactidname")]
		public string BillingContactIdName { get { return _BillingContactIdName; } set { _BillingContactIdName=value; AddUpdatedAttribute("billingcontactidname", value); } } // String - BillingContactIdName - None

	}
}
