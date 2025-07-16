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
	[CrmEntity("systemuser")]
	public class User : CrmEntity<User> // 8 - systemuserid - fullname
	{
		private string _Address1_City;
		[CrmField("address1_city")]
		public string Address1_City { get { return _Address1_City; } set { _Address1_City=value; AddUpdatedAttribute("address1_city", value); } } // String - Address1_City - None

		private string _Address1_Line1;
		[CrmField("address1_line1")]
		public string Address1_Line1 { get { return _Address1_Line1; } set { _Address1_Line1=value; AddUpdatedAttribute("address1_line1", value); } } // String - Address1_Line1 - None

		private string _InternalEMailAddress;
		[CrmField("internalemailaddress")]
		public string InternalEMailAddress { get { return _InternalEMailAddress; } set { _InternalEMailAddress=value; AddUpdatedAttribute("internalemailaddress", value); } } // String - InternalEMailAddress - None

		private string _SiteIdName;
		[CrmField("siteidname", IsRequired = true)]
		public string SiteIdName { get { return _SiteIdName; } set { _SiteIdName=value; AddUpdatedAttribute("siteidname", value); } } // String - SiteIdName - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private float _Address1_Longitude;
		[CrmField("address1_longitude")]
		public float Address1_Longitude { get { return _Address1_Longitude; } set { _Address1_Longitude=value; AddUpdatedAttribute("address1_longitude", value); } } // Double - Address1_Longitude - None

		private string _YomiFullName;
		[CrmField("yomifullname")]
		public string YomiFullName { get { return _YomiFullName; } set { _YomiFullName=value; AddUpdatedAttribute("yomifullname", value); } } // String - YomiFullName - None

		private int _Address1_AddressTypeCode;
		[CrmField("address1_addresstypecode", IsPicklist = true)]
		public int Address1_AddressTypeCode { get { return _Address1_AddressTypeCode; } set { _Address1_AddressTypeCode=value; AddUpdatedAttribute("address1_addresstypecode", value); } } // Picklist - Address1_AddressTypeCode - None

		private string _ParentSystemUserIdName;
		[CrmField("parentsystemuseridname")]
		public string ParentSystemUserIdName { get { return _ParentSystemUserIdName; } set { _ParentSystemUserIdName=value; AddUpdatedAttribute("parentsystemuseridname", value); } } // String - ParentSystemUserIdName - None

		private string _Address2_County;
		[CrmField("address2_county")]
		public string Address2_County { get { return _Address2_County; } set { _Address2_County=value; AddUpdatedAttribute("address2_county", value); } } // String - Address2_County - None

		private string _TransactionCurrencyIdName;
		[CrmField("transactioncurrencyidname")]
		public string TransactionCurrencyIdName { get { return _TransactionCurrencyIdName; } set { _TransactionCurrencyIdName=value; AddUpdatedAttribute("transactioncurrencyidname", value); } } // String - TransactionCurrencyIdName - None

		private Guid _ActiveDirectoryGuid;
		[CrmField("activedirectoryguid")]
		public Guid ActiveDirectoryGuid { get { return _ActiveDirectoryGuid; } set { _ActiveDirectoryGuid=value; AddUpdatedAttribute("activedirectoryguid", value); } } // Uniqueidentifier - ActiveDirectoryGuid - None

		private int _PassportHi;
		[CrmField("passporthi")]
		public int PassportHi { get { return _PassportHi; } set { _PassportHi=value; AddUpdatedAttribute("passporthi", value); } } // Integer - PassportHi - None

		private string _Address1_Line3;
		[CrmField("address1_line3")]
		public string Address1_Line3 { get { return _Address1_Line3; } set { _Address1_Line3=value; AddUpdatedAttribute("address1_line3", value); } } // String - Address1_Line3 - None

		private string _HomePhone;
		[CrmField("homephone")]
		public string HomePhone { get { return _HomePhone; } set { _HomePhone=value; AddUpdatedAttribute("homephone", value); } } // String - HomePhone - None

		private string _ParentSystemUserIdYomiName;
		[CrmField("parentsystemuseridyominame")]
		public string ParentSystemUserIdYomiName { get { return _ParentSystemUserIdYomiName; } set { _ParentSystemUserIdYomiName=value; AddUpdatedAttribute("parentsystemuseridyominame", value); } } // String - ParentSystemUserIdYomiName - None

		private string _Address2_StateOrProvince;
		[CrmField("address2_stateorprovince")]
		public string Address2_StateOrProvince { get { return _Address2_StateOrProvince; } set { _Address2_StateOrProvince=value; AddUpdatedAttribute("address2_stateorprovince", value); } } // String - Address2_StateOrProvince - None

		private CrmEntityReference _TerritoryId;
		[CrmField("territoryid")]
		public CrmEntityReference TerritoryId { get { return _TerritoryId; } set { _TerritoryId=value; AddUpdatedAttribute("territoryid", value); } } // Lookup - TerritoryId - None

		private string _Address2_Country;
		[CrmField("address2_country")]
		public string Address2_Country { get { return _Address2_Country; } set { _Address2_Country=value; AddUpdatedAttribute("address2_country", value); } } // String - Address2_Country - None

		private string _Address2_Line2;
		[CrmField("address2_line2")]
		public string Address2_Line2 { get { return _Address2_Line2; } set { _Address2_Line2=value; AddUpdatedAttribute("address2_line2", value); } } // String - Address2_Line2 - None

		private string _DisabledReason;
		[CrmField("disabledreason")]
		public string DisabledReason { get { return _DisabledReason; } set { _DisabledReason=value; AddUpdatedAttribute("disabledreason", value); } } // String - DisabledReason - None

		private int _Address1_ShippingMethodCode;
		[CrmField("address1_shippingmethodcode", IsPicklist = true)]
		public int Address1_ShippingMethodCode { get { return _Address1_ShippingMethodCode; } set { _Address1_ShippingMethodCode=value; AddUpdatedAttribute("address1_shippingmethodcode", value); } } // Picklist - Address1_ShippingMethodCode - None

		private int _PreferredEmailCode;
		[CrmField("preferredemailcode", IsPicklist = true)]
		public int PreferredEmailCode { get { return _PreferredEmailCode; } set { _PreferredEmailCode=value; AddUpdatedAttribute("preferredemailcode", value); } } // Picklist - PreferredEmailCode - None

		private string _LastName;
		[CrmField("lastname")]
		public string LastName { get { return _LastName; } set { _LastName=value; AddUpdatedAttribute("lastname", value); } } // String - LastName - ApplicationRequired

		private CrmEntityReference _CalendarId;
		[CrmField("calendarid")]
		public CrmEntityReference CalendarId { get { return _CalendarId; } set { _CalendarId=value; AddUpdatedAttribute("calendarid", value); } } // Lookup - CalendarId - None

		private bool _SetupUser;
		[CrmField("setupuser", IsRequired = true)]
		public bool SetupUser { get { return _SetupUser; } set { _SetupUser=value; AddUpdatedAttribute("setupuser", value); } } // Boolean - SetupUser - SystemRequired

		private int _Address1_UTCOffset;
		[CrmField("address1_utcoffset")]
		public int Address1_UTCOffset { get { return _Address1_UTCOffset; } set { _Address1_UTCOffset=value; AddUpdatedAttribute("address1_utcoffset", value); } } // Integer - Address1_UTCOffset - None

		private bool _IsLicensed;
		[CrmField("islicensed", IsRequired = true)]
		public bool IsLicensed { get { return _IsLicensed; } set { _IsLicensed=value; AddUpdatedAttribute("islicensed", value); } } // Boolean - IsLicensed - SystemRequired

		private bool _IsActiveDirectoryUser;
		[CrmField("isactivedirectoryuser", IsRequired = true)]
		public bool IsActiveDirectoryUser { get { return _IsActiveDirectoryUser; } set { _IsActiveDirectoryUser=value; AddUpdatedAttribute("isactivedirectoryuser", value); } } // Boolean - IsActiveDirectoryUser - SystemRequired

		private float _ExchangeRate;
		[CrmField("exchangerate")]
		public float ExchangeRate { get { return _ExchangeRate; } set { _ExchangeRate=value; AddUpdatedAttribute("exchangerate", value); } } // Decimal - ExchangeRate - None

		private string _Address2_City;
		[CrmField("address2_city")]
		public string Address2_City { get { return _Address2_City; } set { _Address2_City=value; AddUpdatedAttribute("address2_city", value); } } // String - Address2_City - None

		private int _PassportLo;
		[CrmField("passportlo")]
		public int PassportLo { get { return _PassportLo; } set { _PassportLo=value; AddUpdatedAttribute("passportlo", value); } } // Integer - PassportLo - None

		private string _Title;
		[CrmField("title")]
		public string Title { get { return _Title; } set { _Title=value; AddUpdatedAttribute("title", value); } } // String - Title - None

		private float _Address2_Latitude;
		[CrmField("address2_latitude")]
		public float Address2_Latitude { get { return _Address2_Latitude; } set { _Address2_Latitude=value; AddUpdatedAttribute("address2_latitude", value); } } // Double - Address2_Latitude - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _WindowsLiveID;
		[CrmField("windowsliveid")]
		public string WindowsLiveID { get { return _WindowsLiveID; } set { _WindowsLiveID=value; AddUpdatedAttribute("windowsliveid", value); } } // String - WindowsLiveID - None

		private CrmEntityReference _QueueId;
		[CrmField("queueid")]
		public CrmEntityReference QueueId { get { return _QueueId; } set { _QueueId=value; AddUpdatedAttribute("queueid", value); } } // Lookup - QueueId - None

		private string _FirstName;
		[CrmField("firstname")]
		public string FirstName { get { return _FirstName; } set { _FirstName=value; AddUpdatedAttribute("firstname", value); } } // String - FirstName - ApplicationRequired

		private string _Address2_PostalCode;
		[CrmField("address2_postalcode")]
		public string Address2_PostalCode { get { return _Address2_PostalCode; } set { _Address2_PostalCode=value; AddUpdatedAttribute("address2_postalcode", value); } } // String - Address2_PostalCode - None

		private bool _DisplayInServiceViews;
		[CrmField("displayinserviceviews")]
		public bool DisplayInServiceViews { get { return _DisplayInServiceViews; } set { _DisplayInServiceViews=value; AddUpdatedAttribute("displayinserviceviews", value); } } // Boolean - DisplayInServiceViews - None

		private string _EmployeeId;
		[CrmField("employeeid")]
		public string EmployeeId { get { return _EmployeeId; } set { _EmployeeId=value; AddUpdatedAttribute("employeeid", value); } } // String - EmployeeId - None

		private CrmEntityReference _ParentSystemUserId;
		[CrmField("parentsystemuserid")]
		public CrmEntityReference ParentSystemUserId { get { return _ParentSystemUserId; } set { _ParentSystemUserId=value; AddUpdatedAttribute("parentsystemuserid", value); } } // Lookup - ParentSystemUserId - None

		private string _GovernmentId;
		[CrmField("governmentid")]
		public string GovernmentId { get { return _GovernmentId; } set { _GovernmentId=value; AddUpdatedAttribute("governmentid", value); } } // String - GovernmentId - None

		private string _Address2_Line3;
		[CrmField("address2_line3")]
		public string Address2_Line3 { get { return _Address2_Line3; } set { _Address2_Line3=value; AddUpdatedAttribute("address2_line3", value); } } // String - Address2_Line3 - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private int _OutgoingEmailDeliveryMethod;
		[CrmField("outgoingemaildeliverymethod", IsRequired = true, IsPicklist = true)]
		public int OutgoingEmailDeliveryMethod { get { return _OutgoingEmailDeliveryMethod; } set { _OutgoingEmailDeliveryMethod=value; AddUpdatedAttribute("outgoingemaildeliverymethod", value); } } // Picklist - OutgoingEmailDeliveryMethod - SystemRequired

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber=value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private string _Address1_County;
		[CrmField("address1_county")]
		public string Address1_County { get { return _Address1_County; } set { _Address1_County=value; AddUpdatedAttribute("address1_county", value); } } // String - Address1_County - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private string _Skills;
		[CrmField("skills")]
		public string Skills { get { return _Skills; } set { _Skills=value; AddUpdatedAttribute("skills", value); } } // String - Skills - None

		private int _PreferredPhoneCode;
		[CrmField("preferredphonecode", IsPicklist = true)]
		public int PreferredPhoneCode { get { return _PreferredPhoneCode; } set { _PreferredPhoneCode=value; AddUpdatedAttribute("preferredphonecode", value); } } // Picklist - PreferredPhoneCode - None

		private string _Address2_PostOfficeBox;
		[CrmField("address2_postofficebox")]
		public string Address2_PostOfficeBox { get { return _Address2_PostOfficeBox; } set { _Address2_PostOfficeBox=value; AddUpdatedAttribute("address2_postofficebox", value); } } // String - Address2_PostOfficeBox - None

		private string _Address2_Telephone1;
		[CrmField("address2_telephone1")]
		public string Address2_Telephone1 { get { return _Address2_Telephone1; } set { _Address2_Telephone1=value; AddUpdatedAttribute("address2_telephone1", value); } } // String - Address2_Telephone1 - None

		private string _Address2_Telephone2;
		[CrmField("address2_telephone2")]
		public string Address2_Telephone2 { get { return _Address2_Telephone2; } set { _Address2_Telephone2=value; AddUpdatedAttribute("address2_telephone2", value); } } // String - Address2_Telephone2 - None

		private string _Address2_Telephone3;
		[CrmField("address2_telephone3")]
		public string Address2_Telephone3 { get { return _Address2_Telephone3; } set { _Address2_Telephone3=value; AddUpdatedAttribute("address2_telephone3", value); } } // String - Address2_Telephone3 - None

		private string _MobilePhone;
		[CrmField("mobilephone")]
		public string MobilePhone { get { return _MobilePhone; } set { _MobilePhone=value; AddUpdatedAttribute("mobilephone", value); } } // String - MobilePhone - None

		private CrmEntityReference _SiteId;
		[CrmField("siteid")]
		public CrmEntityReference SiteId { get { return _SiteId; } set { _SiteId=value; AddUpdatedAttribute("siteid", value); } } // Lookup - SiteId - None

		private int _EmailRouterAccessApproval;
		[CrmField("emailrouteraccessapproval", IsRequired = true, IsPicklist = true)]
		public int EmailRouterAccessApproval { get { return _EmailRouterAccessApproval; } set { _EmailRouterAccessApproval=value; AddUpdatedAttribute("emailrouteraccessapproval", value); } } // Picklist - EmailRouterAccessApproval - SystemRequired

		private string _TerritoryIdName;
		[CrmField("territoryidname", IsRequired = true)]
		public string TerritoryIdName { get { return _TerritoryIdName; } set { _TerritoryIdName=value; AddUpdatedAttribute("territoryidname", value); } } // String - TerritoryIdName - SystemRequired

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private Guid _Address2_AddressId;
		[CrmField("address2_addressid")]
		public Guid Address2_AddressId { get { return _Address2_AddressId; } set { _Address2_AddressId=value; AddUpdatedAttribute("address2_addressid", value); } } // Uniqueidentifier - Address2_AddressId - None

		private Guid _id;
		[CrmField("systemuserid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - SystemUserId - SystemRequired

		private string _MiddleName;
		[CrmField("middlename")]
		public string MiddleName { get { return _MiddleName; } set { _MiddleName=value; AddUpdatedAttribute("middlename", value); } } // String - MiddleName - None

		private string _YomiLastName;
		[CrmField("yomilastname")]
		public string YomiLastName { get { return _YomiLastName; } set { _YomiLastName=value; AddUpdatedAttribute("yomilastname", value); } } // String - YomiLastName - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private string _Address1_Country;
		[CrmField("address1_country")]
		public string Address1_Country { get { return _Address1_Country; } set { _Address1_Country=value; AddUpdatedAttribute("address1_country", value); } } // String - Address1_Country - None

		private string _MobileAlertEMail;
		[CrmField("mobilealertemail")]
		public string MobileAlertEMail { get { return _MobileAlertEMail; } set { _MobileAlertEMail=value; AddUpdatedAttribute("mobilealertemail", value); } } // String - MobileAlertEMail - None

		private string _BusinessUnitIdName;
		[CrmField("businessunitidname", IsRequired = true)]
		public string BusinessUnitIdName { get { return _BusinessUnitIdName; } set { _BusinessUnitIdName=value; AddUpdatedAttribute("businessunitidname", value); } } // String - BusinessUnitIdName - SystemRequired

		private string _Address1_StateOrProvince;
		[CrmField("address1_stateorprovince")]
		public string Address1_StateOrProvince { get { return _Address1_StateOrProvince; } set { _Address1_StateOrProvince=value; AddUpdatedAttribute("address1_stateorprovince", value); } } // String - Address1_StateOrProvince - None

		private int _PreferredAddressCode;
		[CrmField("preferredaddresscode", IsPicklist = true)]
		public int PreferredAddressCode { get { return _PreferredAddressCode; } set { _PreferredAddressCode=value; AddUpdatedAttribute("preferredaddresscode", value); } } // Picklist - PreferredAddressCode - None

		private int _IncomingEmailDeliveryMethod;
		[CrmField("incomingemaildeliverymethod", IsRequired = true, IsPicklist = true)]
		public int IncomingEmailDeliveryMethod { get { return _IncomingEmailDeliveryMethod; } set { _IncomingEmailDeliveryMethod=value; AddUpdatedAttribute("incomingemaildeliverymethod", value); } } // Picklist - IncomingEmailDeliveryMethod - SystemRequired

		private string _NickName;
		[CrmField("nickname")]
		public string NickName { get { return _NickName; } set { _NickName=value; AddUpdatedAttribute("nickname", value); } } // String - NickName - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private string _JobTitle;
		[CrmField("jobtitle")]
		public string JobTitle { get { return _JobTitle; } set { _JobTitle=value; AddUpdatedAttribute("jobtitle", value); } } // String - JobTitle - None

		private CrmEntityReference _TransactionCurrencyId;
		[CrmField("transactioncurrencyid")]
		public CrmEntityReference TransactionCurrencyId { get { return _TransactionCurrencyId; } set { _TransactionCurrencyId=value; AddUpdatedAttribute("transactioncurrencyid", value); } } // Lookup - TransactionCurrencyId - None

		private string _Address1_Telephone1;
		[CrmField("address1_telephone1")]
		public string Address1_Telephone1 { get { return _Address1_Telephone1; } set { _Address1_Telephone1=value; AddUpdatedAttribute("address1_telephone1", value); } } // String - Address1_Telephone1 - None

		private string _Address1_Telephone2;
		[CrmField("address1_telephone2")]
		public string Address1_Telephone2 { get { return _Address1_Telephone2; } set { _Address1_Telephone2=value; AddUpdatedAttribute("address1_telephone2", value); } } // String - Address1_Telephone2 - None

		private string _Address1_Telephone3;
		[CrmField("address1_telephone3")]
		public string Address1_Telephone3 { get { return _Address1_Telephone3; } set { _Address1_Telephone3=value; AddUpdatedAttribute("address1_telephone3", value); } } // String - Address1_Telephone3 - None

		private string _Address1_PostOfficeBox;
		[CrmField("address1_postofficebox")]
		public string Address1_PostOfficeBox { get { return _Address1_PostOfficeBox; } set { _Address1_PostOfficeBox=value; AddUpdatedAttribute("address1_postofficebox", value); } } // String - Address1_PostOfficeBox - None

		private Guid _OrganizationId;
		[CrmField("organizationid", IsRequired = true)]
		public Guid OrganizationId { get { return _OrganizationId; } set { _OrganizationId=value; AddUpdatedAttribute("organizationid", value); } } // Uniqueidentifier - OrganizationId - SystemRequired

		private string _YammerEmailAddress;
		[CrmField("yammeremailaddress")]
		public string YammerEmailAddress { get { return _YammerEmailAddress; } set { _YammerEmailAddress=value; AddUpdatedAttribute("yammeremailaddress", value); } } // String - YammerEmailAddress - None

		private int _CALType;
		[CrmField("caltype", IsRequired = true, IsPicklist = true)]
		public int CALType { get { return _CALType; } set { _CALType=value; AddUpdatedAttribute("caltype", value); } } // Picklist - CALType - SystemRequired

		private Guid _Address1_AddressId;
		[CrmField("address1_addressid")]
		public Guid Address1_AddressId { get { return _Address1_AddressId; } set { _Address1_AddressId=value; AddUpdatedAttribute("address1_addressid", value); } } // Uniqueidentifier - Address1_AddressId - None

		private string _YomiMiddleName;
		[CrmField("yomimiddlename")]
		public string YomiMiddleName { get { return _YomiMiddleName; } set { _YomiMiddleName=value; AddUpdatedAttribute("yomimiddlename", value); } } // String - YomiMiddleName - None

		private int _Address2_UTCOffset;
		[CrmField("address2_utcoffset")]
		public int Address2_UTCOffset { get { return _Address2_UTCOffset; } set { _Address2_UTCOffset=value; AddUpdatedAttribute("address2_utcoffset", value); } } // Integer - Address2_UTCOffset - None

		private string _PhotoUrl;
		[CrmField("photourl")]
		public string PhotoUrl { get { return _PhotoUrl; } set { _PhotoUrl=value; AddUpdatedAttribute("photourl", value); } } // String - PhotoUrl - None

		private int _AccessMode;
		[CrmField("accessmode", IsRequired = true, IsPicklist = true)]
		public int AccessMode { get { return _AccessMode; } set { _AccessMode=value; AddUpdatedAttribute("accessmode", value); } } // Picklist - AccessMode - SystemRequired

		private string _PersonalEMailAddress;
		[CrmField("personalemailaddress")]
		public string PersonalEMailAddress { get { return _PersonalEMailAddress; } set { _PersonalEMailAddress=value; AddUpdatedAttribute("personalemailaddress", value); } } // String - PersonalEMailAddress - None

		private bool _IsIntegrationUser;
		[CrmField("isintegrationuser", IsRequired = true)]
		public bool IsIntegrationUser { get { return _IsIntegrationUser; } set { _IsIntegrationUser=value; AddUpdatedAttribute("isintegrationuser", value); } } // Boolean - IsIntegrationUser - SystemRequired

		private float _Address2_Longitude;
		[CrmField("address2_longitude")]
		public float Address2_Longitude { get { return _Address2_Longitude; } set { _Address2_Longitude=value; AddUpdatedAttribute("address2_longitude", value); } } // Double - Address2_Longitude - None

		private string _Address2_Fax;
		[CrmField("address2_fax")]
		public string Address2_Fax { get { return _Address2_Fax; } set { _Address2_Fax=value; AddUpdatedAttribute("address2_fax", value); } } // String - Address2_Fax - None

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private string _YammerUserId;
		[CrmField("yammeruserid")]
		public string YammerUserId { get { return _YammerUserId; } set { _YammerUserId=value; AddUpdatedAttribute("yammeruserid", value); } } // String - YammerUserId - None

		private string _DomainName;
		[CrmField("domainname", IsRequired = true)]
		public string DomainName { get { return _DomainName; } set { _DomainName=value; AddUpdatedAttribute("domainname", value); } } // String - DomainName - SystemRequired

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private string _Address1_Line2;
		[CrmField("address1_line2")]
		public string Address1_Line2 { get { return _Address1_Line2; } set { _Address1_Line2=value; AddUpdatedAttribute("address1_line2", value); } } // String - Address1_Line2 - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private int _Address2_AddressTypeCode;
		[CrmField("address2_addresstypecode", IsPicklist = true)]
		public int Address2_AddressTypeCode { get { return _Address2_AddressTypeCode; } set { _Address2_AddressTypeCode=value; AddUpdatedAttribute("address2_addresstypecode", value); } } // Picklist - Address2_AddressTypeCode - None

		private bool _DefaultFiltersPopulated;
		[CrmField("defaultfilterspopulated", IsRequired = true)]
		public bool DefaultFiltersPopulated { get { return _DefaultFiltersPopulated; } set { _DefaultFiltersPopulated=value; AddUpdatedAttribute("defaultfilterspopulated", value); } } // Boolean - DefaultFiltersPopulated - SystemRequired

		private string _Salutation;
		[CrmField("salutation")]
		public string Salutation { get { return _Salutation; } set { _Salutation=value; AddUpdatedAttribute("salutation", value); } } // String - Salutation - None

		private string _Address1_PostalCode;
		[CrmField("address1_postalcode")]
		public string Address1_PostalCode { get { return _Address1_PostalCode; } set { _Address1_PostalCode=value; AddUpdatedAttribute("address1_postalcode", value); } } // String - Address1_PostalCode - None

		private string _Address2_UPSZone;
		[CrmField("address2_upszone")]
		public string Address2_UPSZone { get { return _Address2_UPSZone; } set { _Address2_UPSZone=value; AddUpdatedAttribute("address2_upszone", value); } } // String - Address2_UPSZone - None

		private bool _IsDisabled;
		[CrmField("isdisabled")]
		public bool IsDisabled { get { return _IsDisabled; } set { _IsDisabled=value; AddUpdatedAttribute("isdisabled", value); } } // Boolean - IsDisabled - None

		private string _Address2_Name;
		[CrmField("address2_name")]
		public string Address2_Name { get { return _Address2_Name; } set { _Address2_Name=value; AddUpdatedAttribute("address2_name", value); } } // String - Address2_Name - None

		private string _YomiFirstName;
		[CrmField("yomifirstname")]
		public string YomiFirstName { get { return _YomiFirstName; } set { _YomiFirstName=value; AddUpdatedAttribute("yomifirstname", value); } } // String - YomiFirstName - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private int _InviteStatusCode;
		[CrmField("invitestatuscode", IsPicklist = true)]
		public int InviteStatusCode { get { return _InviteStatusCode; } set { _InviteStatusCode=value; AddUpdatedAttribute("invitestatuscode", value); } } // Picklist - InviteStatusCode - ApplicationRequired

		private string _OrganizationIdName;
		[CrmField("organizationidname", IsRequired = true)]
		public string OrganizationIdName { get { return _OrganizationIdName; } set { _OrganizationIdName=value; AddUpdatedAttribute("organizationidname", value); } } // String - OrganizationIdName - SystemRequired

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private string _QueueIdName;
		[CrmField("queueidname")]
		public string QueueIdName { get { return _QueueIdName; } set { _QueueIdName=value; AddUpdatedAttribute("queueidname", value); } } // String - QueueIdName - None

		private bool _IsSyncWithDirectory;
		[CrmField("issyncwithdirectory", IsRequired = true)]
		public bool IsSyncWithDirectory { get { return _IsSyncWithDirectory; } set { _IsSyncWithDirectory=value; AddUpdatedAttribute("issyncwithdirectory", value); } } // Boolean - IsSyncWithDirectory - SystemRequired

		private string _Address1_Name;
		[CrmField("address1_name")]
		public string Address1_Name { get { return _Address1_Name; } set { _Address1_Name=value; AddUpdatedAttribute("address1_name", value); } } // String - Address1_Name - None

		private string _Address1_Fax;
		[CrmField("address1_fax")]
		public string Address1_Fax { get { return _Address1_Fax; } set { _Address1_Fax=value; AddUpdatedAttribute("address1_fax", value); } } // String - Address1_Fax - None

		private float _Address1_Latitude;
		[CrmField("address1_latitude")]
		public float Address1_Latitude { get { return _Address1_Latitude; } set { _Address1_Latitude=value; AddUpdatedAttribute("address1_latitude", value); } } // Double - Address1_Latitude - None

		private int _Address2_ShippingMethodCode;
		[CrmField("address2_shippingmethodcode", IsPicklist = true)]
		public int Address2_ShippingMethodCode { get { return _Address2_ShippingMethodCode; } set { _Address2_ShippingMethodCode=value; AddUpdatedAttribute("address2_shippingmethodcode", value); } } // Picklist - Address2_ShippingMethodCode - None

		private CrmEntityReference _BusinessUnitId;
		[CrmField("businessunitid", IsRequired = true)]
		public CrmEntityReference BusinessUnitId { get { return _BusinessUnitId; } set { _BusinessUnitId=value; AddUpdatedAttribute("businessunitid", value); } } // Lookup - BusinessUnitId - SystemRequired

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private string _Address2_Line1;
		[CrmField("address2_line1")]
		public string Address2_Line1 { get { return _Address2_Line1; } set { _Address2_Line1=value; AddUpdatedAttribute("address2_line1", value); } } // String - Address2_Line1 - None

		private string _Address1_UPSZone;
		[CrmField("address1_upszone")]
		public string Address1_UPSZone { get { return _Address1_UPSZone; } set { _Address1_UPSZone=value; AddUpdatedAttribute("address1_upszone", value); } } // String - Address1_UPSZone - None

		private string _FullName;
		[CrmField("fullname")]
		public string FullName { get { return _FullName; } set { _FullName=value; AddUpdatedAttribute("fullname", value); } } // String - FullName - None

	}
}
