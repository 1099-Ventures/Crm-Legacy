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
	[CrmEntity("contractdetail")]
	public class ContractLine : CrmEntity<ContractLine> // 1011 - contractdetailid - title
	{
		private CrmEntityReference _ProductId;
		[CrmField("productid")]
		public CrmEntityReference ProductId { get { return _ProductId; } set { _ProductId=value; AddUpdatedAttribute("productid", value); } } // Lookup - ProductId - ApplicationRequired

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private int _TotalAllotments;
		[CrmField("totalallotments", IsRequired = true)]
		public int TotalAllotments { get { return _TotalAllotments; } set { _TotalAllotments=value; AddUpdatedAttribute("totalallotments", value); } } // Integer - TotalAllotments - SystemRequired

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private float _DiscountPercentage;
		[CrmField("discountpercentage")]
		public float DiscountPercentage { get { return _DiscountPercentage; } set { _DiscountPercentage=value; AddUpdatedAttribute("discountpercentage", value); } } // Decimal - DiscountPercentage - None

		private string _Title;
		[CrmField("title")]
		public string Title { get { return _Title; } set { _Title=value; AddUpdatedAttribute("title", value); } } // String - Title - ApplicationRequired

		private CrmEntityReference _CustomerId;
		[CrmField("customerid", IsRequired = true)]
		public CrmEntityReference CustomerId { get { return _CustomerId; } set { _CustomerId=value; AddUpdatedAttribute("customerid", value); } } // Customer - CustomerId - SystemRequired

		private int _RateType;
		[CrmField("azuro_ratetype", IsPicklist = true)]
		public int RateType { get { return _RateType; } set { _RateType=value; AddUpdatedAttribute("azuro_ratetype", value); } } // Picklist - azuro_RateType - None

		private decimal _Price;
		[CrmField("price", IsRequired = true)]
		public decimal Price { get { return _Price; } set { _Price=value; AddUpdatedAttribute("price", value); } } // Money - Price - SystemRequired

		private CrmEntityReference _ContractId;
		[CrmField("contractid", IsRequired = true)]
		public CrmEntityReference ContractId { get { return _ContractId; } set { _ContractId=value; AddUpdatedAttribute("contractid", value); } } // Lookup - ContractId - SystemRequired

		private string _CustomerIdType;
		[CrmField("customeridtype")]
		public string CustomerIdType { get { return _CustomerIdType; } set { _CustomerIdType=value; AddUpdatedAttribute("customeridtype", value); } } // EntityName - CustomerIdType - ApplicationRequired

		private int _LineItemOrder;
		[CrmField("lineitemorder")]
		public int LineItemOrder { get { return _LineItemOrder; } set { _LineItemOrder=value; AddUpdatedAttribute("lineitemorder", value); } } // Integer - LineItemOrder - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private CrmEntityReference _UoMScheduleId;
		[CrmField("uomscheduleid")]
		public CrmEntityReference UoMScheduleId { get { return _UoMScheduleId; } set { _UoMScheduleId=value; AddUpdatedAttribute("uomscheduleid", value); } } // Lookup - UoMScheduleId - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private CrmEntityReference _ServiceAddress;
		[CrmField("serviceaddress")]
		public CrmEntityReference ServiceAddress { get { return _ServiceAddress; } set { _ServiceAddress=value; AddUpdatedAttribute("serviceaddress", value); } } // Lookup - ServiceAddress - None

		private CrmEntityReference _ContactId;
		[CrmField("contactid")]
		public CrmEntityReference ContactId { get { return _ContactId; } set { _ContactId=value; AddUpdatedAttribute("contactid", value); } } // Lookup - ContactId - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private DateTime _ExpiresOn;
		[CrmField("expireson", IsRequired = true)]
		public DateTime ExpiresOn { get { return _ExpiresOn; } set { _ExpiresOn=value; AddUpdatedAttribute("expireson", value); } } // DateTime - ExpiresOn - SystemRequired

		private int _AllotmentsOverage;
		[CrmField("allotmentsoverage")]
		public int AllotmentsOverage { get { return _AllotmentsOverage; } set { _AllotmentsOverage=value; AddUpdatedAttribute("allotmentsoverage", value); } } // Integer - AllotmentsOverage - None

		private string _CustomerIdYomiName;
		[CrmField("customeridyominame")]
		public string CustomerIdYomiName { get { return _CustomerIdYomiName; } set { _CustomerIdYomiName=value; AddUpdatedAttribute("customeridyominame", value); } } // String - CustomerIdYomiName - ApplicationRequired

		private int _ContractStateCode;
		[CrmField("contractstatecode", IsRequired = true, IsPicklist = true)]
		public int ContractStateCode { get { return _ContractStateCode; } set { _ContractStateCode=value; AddUpdatedAttribute("contractstatecode", value); } } // Picklist - ContractStateCode - SystemRequired

		private string _UoMScheduleIdName;
		[CrmField("uomscheduleidname", IsRequired = true)]
		public string UoMScheduleIdName { get { return _UoMScheduleIdName; } set { _UoMScheduleIdName=value; AddUpdatedAttribute("uomscheduleidname", value); } } // String - UoMScheduleIdName - SystemRequired

		private DateTime _ActiveOn;
		[CrmField("activeon", IsRequired = true)]
		public DateTime ActiveOn { get { return _ActiveOn; } set { _ActiveOn=value; AddUpdatedAttribute("activeon", value); } } // DateTime - ActiveOn - SystemRequired

		private string _ContractIdName;
		[CrmField("contractidname")]
		public string ContractIdName { get { return _ContractIdName; } set { _ContractIdName=value; AddUpdatedAttribute("contractidname", value); } } // String - ContractIdName - None

		private Guid _id;
		[CrmField("contractdetailid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - ContractDetailId - SystemRequired

		private int _AllotmentsUsed;
		[CrmField("allotmentsused")]
		public int AllotmentsUsed { get { return _AllotmentsUsed; } set { _AllotmentsUsed=value; AddUpdatedAttribute("allotmentsused", value); } } // Integer - AllotmentsUsed - None

		private int _MeanTimetoRepair;
		[CrmField("azuro_meantimetorepair", IsPicklist = true)]
		public int MeanTimetoRepair { get { return _MeanTimetoRepair; } set { _MeanTimetoRepair=value; AddUpdatedAttribute("azuro_meantimetorepair", value); } } // Picklist - azuro_MeanTimetoRepair - Recommended

		private string _UoMIdName;
		[CrmField("uomidname", IsRequired = true)]
		public string UoMIdName { get { return _UoMIdName; } set { _UoMIdName=value; AddUpdatedAttribute("uomidname", value); } } // String - UoMIdName - SystemRequired

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private CrmEntityReference _UoMId;
		[CrmField("uomid")]
		public CrmEntityReference UoMId { get { return _UoMId; } set { _UoMId=value; AddUpdatedAttribute("uomid", value); } } // Lookup - UoMId - ApplicationRequired

		private int _AllotmentsRemaining;
		[CrmField("allotmentsremaining")]
		public int AllotmentsRemaining { get { return _AllotmentsRemaining; } set { _AllotmentsRemaining=value; AddUpdatedAttribute("allotmentsremaining", value); } } // Integer - AllotmentsRemaining - None

		private decimal _Discount;
		[CrmField("discount")]
		public decimal Discount { get { return _Discount; } set { _Discount=value; AddUpdatedAttribute("discount", value); } } // Money - Discount - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid")]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - ApplicationRequired

		private string _EffectivityCalendar;
		[CrmField("effectivitycalendar")]
		public string EffectivityCalendar { get { return _EffectivityCalendar; } set { _EffectivityCalendar=value; AddUpdatedAttribute("effectivitycalendar", value); } } // String - EffectivityCalendar - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private int _InitialQuantity;
		[CrmField("initialquantity")]
		public int InitialQuantity { get { return _InitialQuantity; } set { _InitialQuantity=value; AddUpdatedAttribute("initialquantity", value); } } // Integer - InitialQuantity - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private string _CustomerIdName;
		[CrmField("customeridname")]
		public string CustomerIdName { get { return _CustomerIdName; } set { _CustomerIdName=value; AddUpdatedAttribute("customeridname", value); } } // String - CustomerIdName - ApplicationRequired

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private int _ServiceContractUnitsCode;
		[CrmField("servicecontractunitscode", IsPicklist = true)]
		public int ServiceContractUnitsCode { get { return _ServiceContractUnitsCode; } set { _ServiceContractUnitsCode=value; AddUpdatedAttribute("servicecontractunitscode", value); } } // Picklist - ServiceContractUnitsCode - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - None

		private decimal _Net;
		[CrmField("net")]
		public decimal Net { get { return _Net; } set { _Net=value; AddUpdatedAttribute("net", value); } } // Money - Net - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private CrmEntityReference _AccountId;
		[CrmField("accountid")]
		public CrmEntityReference AccountId { get { return _AccountId; } set { _AccountId=value; AddUpdatedAttribute("accountid", value); } } // Lookup - AccountId - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private Guid _OwningUser;
		[CrmField("owninguser")]
		public Guid OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Uniqueidentifier - OwningUser - ApplicationRequired

		private int _MeanTimetoRespond;
		[CrmField("azuro_meantimetorespond", IsPicklist = true)]
		public int MeanTimetoRespond { get { return _MeanTimetoRespond; } set { _MeanTimetoRespond=value; AddUpdatedAttribute("azuro_meantimetorespond", value); } } // Picklist - azuro_MeanTimetoRespond - Recommended

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber=value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private Guid _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public Guid OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Uniqueidentifier - OwningBusinessUnit - ApplicationRequired

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private string _ProductIdName;
		[CrmField("productidname")]
		public string ProductIdName { get { return _ProductIdName; } set { _ProductIdName=value; AddUpdatedAttribute("productidname", value); } } // String - ProductIdName - None

		private int _StatusCode;
		[CrmField("statuscode", IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - None

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private decimal _Rate;
		[CrmField("rate")]
		public decimal Rate { get { return _Rate; } set { _Rate=value; AddUpdatedAttribute("rate", value); } } // Money - Rate - None

		private string _ServiceAddressName;
		[CrmField("serviceaddressname")]
		public string ServiceAddressName { get { return _ServiceAddressName; } set { _ServiceAddressName=value; AddUpdatedAttribute("serviceaddressname", value); } } // String - ServiceAddressName - None

		private string _ProductSerialNumber;
		[CrmField("productserialnumber")]
		public string ProductSerialNumber { get { return _ProductSerialNumber; } set { _ProductSerialNumber=value; AddUpdatedAttribute("productserialnumber", value); } } // String - ProductSerialNumber - None

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _TransactionCurrencyIdName;
		[CrmField("transactioncurrencyidname")]
		public string TransactionCurrencyIdName { get { return _TransactionCurrencyIdName; } set { _TransactionCurrencyIdName=value; AddUpdatedAttribute("transactioncurrencyidname", value); } } // String - TransactionCurrencyIdName - None

	}
}
