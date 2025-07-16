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
	[CrmEntity("product")]
	public class Product : CrmEntity<Product> // 1024 - productid - name
	{
		private bool _IsKit;
		[CrmField("iskit")]
		public bool IsKit { get { return _IsKit; } set { _IsKit=value; AddUpdatedAttribute("iskit", value); } } // Boolean - IsKit - None

		private Guid _id;
		[CrmField("productid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - ProductId - SystemRequired

		private string _DefaultUoMScheduleIdName;
		[CrmField("defaultuomscheduleidname", IsRequired = true)]
		public string DefaultUoMScheduleIdName { get { return _DefaultUoMScheduleIdName; } set { _DefaultUoMScheduleIdName=value; AddUpdatedAttribute("defaultuomscheduleidname", value); } } // String - DefaultUoMScheduleIdName - SystemRequired

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private int _ProductTypeCode;
		[CrmField("producttypecode", IsPicklist = true)]
		public int ProductTypeCode { get { return _ProductTypeCode; } set { _ProductTypeCode=value; AddUpdatedAttribute("producttypecode", value); } } // Picklist - ProductTypeCode - None

		private int _QuantityDecimal;
		[CrmField("quantitydecimal")]
		public int QuantityDecimal { get { return _QuantityDecimal; } set { _QuantityDecimal=value; AddUpdatedAttribute("quantitydecimal", value); } } // Integer - QuantityDecimal - ApplicationRequired

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private decimal _Price;
		[CrmField("price")]
		public decimal Price { get { return _Price; } set { _Price=value; AddUpdatedAttribute("price", value); } } // Money - Price - None

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private string _VendorName;
		[CrmField("vendorname")]
		public string VendorName { get { return _VendorName; } set { _VendorName=value; AddUpdatedAttribute("vendorname", value); } } // String - VendorName - None

		private string _Description;
		[CrmField("description")]
		public string Description { get { return _Description; } set { _Description=value; AddUpdatedAttribute("description", value); } } // Memo - Description - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private string _ProductUrl;
		[CrmField("producturl")]
		public string ProductUrl { get { return _ProductUrl; } set { _ProductUrl=value; AddUpdatedAttribute("producturl", value); } } // String - ProductUrl - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private CrmEntityReference _DefaultUoMScheduleId;
		[CrmField("defaultuomscheduleid")]
		public CrmEntityReference DefaultUoMScheduleId { get { return _DefaultUoMScheduleId; } set { _DefaultUoMScheduleId=value; AddUpdatedAttribute("defaultuomscheduleid", value); } } // Lookup - DefaultUoMScheduleId - ApplicationRequired

		private string _DefaultUoMIdName;
		[CrmField("defaultuomidname", IsRequired = true)]
		public string DefaultUoMIdName { get { return _DefaultUoMIdName; } set { _DefaultUoMIdName=value; AddUpdatedAttribute("defaultuomidname", value); } } // String - DefaultUoMIdName - SystemRequired

		private string _Size;
		[CrmField("size")]
		public string Size { get { return _Size; } set { _Size=value; AddUpdatedAttribute("size", value); } } // String - Size - None

		private string _OrganizationIdName;
		[CrmField("organizationidname", IsRequired = true)]
		public string OrganizationIdName { get { return _OrganizationIdName; } set { _OrganizationIdName=value; AddUpdatedAttribute("organizationidname", value); } } // String - OrganizationIdName - SystemRequired

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private CrmEntityReference _SubjectId;
		[CrmField("subjectid")]
		public CrmEntityReference SubjectId { get { return _SubjectId; } set { _SubjectId=value; AddUpdatedAttribute("subjectid", value); } } // Lookup - SubjectId - None

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private string _VendorPartNumber;
		[CrmField("vendorpartnumber")]
		public string VendorPartNumber { get { return _VendorPartNumber; } set { _VendorPartNumber=value; AddUpdatedAttribute("vendorpartnumber", value); } } // String - VendorPartNumber - None

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private decimal _CurrentCost;
		[CrmField("currentcost")]
		public decimal CurrentCost { get { return _CurrentCost; } set { _CurrentCost=value; AddUpdatedAttribute("currentcost", value); } } // Money - CurrentCost - None

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private CrmEntityReference _PriceLevelId;
		[CrmField("pricelevelid")]
		public CrmEntityReference PriceLevelId { get { return _PriceLevelId; } set { _PriceLevelId=value; AddUpdatedAttribute("pricelevelid", value); } } // Lookup - PriceLevelId - Recommended

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private CrmEntityReference _DefaultUoMId;
		[CrmField("defaultuomid")]
		public CrmEntityReference DefaultUoMId { get { return _DefaultUoMId; } set { _DefaultUoMId=value; AddUpdatedAttribute("defaultuomid", value); } } // Lookup - DefaultUoMId - ApplicationRequired

		private float _StockVolume;
		[CrmField("stockvolume")]
		public float StockVolume { get { return _StockVolume; } set { _StockVolume=value; AddUpdatedAttribute("stockvolume", value); } } // Decimal - StockVolume - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - None

		private bool _IsStockItem;
		[CrmField("isstockitem")]
		public bool IsStockItem { get { return _IsStockItem; } set { _IsStockItem=value; AddUpdatedAttribute("isstockitem", value); } } // Boolean - IsStockItem - None

		private float _QuantityOnHand;
		[CrmField("quantityonhand")]
		public float QuantityOnHand { get { return _QuantityOnHand; } set { _QuantityOnHand=value; AddUpdatedAttribute("quantityonhand", value); } } // Decimal - QuantityOnHand - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private string _PriceLevelIdName;
		[CrmField("pricelevelidname", IsRequired = true)]
		public string PriceLevelIdName { get { return _PriceLevelIdName; } set { _PriceLevelIdName=value; AddUpdatedAttribute("pricelevelidname", value); } } // String - PriceLevelIdName - SystemRequired

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _SubjectIdName;
		[CrmField("subjectidname")]
		public string SubjectIdName { get { return _SubjectIdName; } set { _SubjectIdName=value; AddUpdatedAttribute("subjectidname", value); } } // String - SubjectIdName - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private string _TransactionCurrencyIdName;
		[CrmField("transactioncurrencyidname")]
		public string TransactionCurrencyIdName { get { return _TransactionCurrencyIdName; } set { _TransactionCurrencyIdName=value; AddUpdatedAttribute("transactioncurrencyidname", value); } } // String - TransactionCurrencyIdName - None

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber=value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private string _SupplierName;
		[CrmField("suppliername")]
		public string SupplierName { get { return _SupplierName; } set { _SupplierName=value; AddUpdatedAttribute("suppliername", value); } } // String - SupplierName - None

		private string _Name;
		[CrmField("name")]
		public string Name { get { return _Name; } set { _Name=value; AddUpdatedAttribute("name", value); } } // String - Name - ApplicationRequired

		private float _StockWeight;
		[CrmField("stockweight")]
		public float StockWeight { get { return _StockWeight; } set { _StockWeight=value; AddUpdatedAttribute("stockweight", value); } } // Decimal - StockWeight - None

		private int _StatusCode;
		[CrmField("statuscode", IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private CrmEntityReference _OrganizationId;
		[CrmField("organizationid", IsRequired = true)]
		public CrmEntityReference OrganizationId { get { return _OrganizationId; } set { _OrganizationId=value; AddUpdatedAttribute("organizationid", value); } } // Lookup - OrganizationId - SystemRequired

		private string _ProductNumber;
		[CrmField("productnumber", IsRequired = true)]
		public string ProductNumber { get { return _ProductNumber; } set { _ProductNumber=value; AddUpdatedAttribute("productnumber", value); } } // String - ProductNumber - SystemRequired

		private decimal _StandardCost;
		[CrmField("standardcost")]
		public decimal StandardCost { get { return _StandardCost; } set { _StandardCost=value; AddUpdatedAttribute("standardcost", value); } } // Money - StandardCost - None

	}
}
