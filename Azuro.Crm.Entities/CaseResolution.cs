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
	[CrmEntity("incidentresolution")]
	public class CaseResolution : CrmEntity<CaseResolution> // 4206 - activityid - subject
	{
		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private int _StateCode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int StateCode { get { return _StateCode; } set { _StateCode=value; AddUpdatedAttribute("statecode", value); } } // State - StateCode - SystemRequired

		private string _OwnerIdName;
		[CrmField("owneridname", IsRequired = true)]
		public string OwnerIdName { get { return _OwnerIdName; } set { _OwnerIdName=value; AddUpdatedAttribute("owneridname", value); } } // String - OwnerIdName - SystemRequired

		private int _StatusCode;
		[CrmField("statuscode", IsPicklist = true)]
		public int StatusCode { get { return _StatusCode; } set { _StatusCode=value; AddUpdatedAttribute("statuscode", value); } } // Status - StatusCode - None

		private int _TimeSpent;
		[CrmField("timespent")]
		public int TimeSpent { get { return _TimeSpent; } set { _TimeSpent=value; AddUpdatedAttribute("timespent", value); } } // Integer - TimeSpent - None

		private bool _IsBilled;
		[CrmField("isbilled")]
		public bool IsBilled { get { return _IsBilled; } set { _IsBilled=value; AddUpdatedAttribute("isbilled", value); } } // Boolean - IsBilled - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid", IsRequired = true)]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - SystemRequired

		private string _IncidentIdType;
		[CrmField("incidentidtype")]
		public string IncidentIdType { get { return _IncidentIdType; } set { _IncidentIdType=value; AddUpdatedAttribute("incidentidtype", value); } } // EntityName - IncidentIdType - None

		private CrmEntityReference _ServiceId;
		[CrmField("serviceid")]
		public CrmEntityReference ServiceId { get { return _ServiceId; } set { _ServiceId=value; AddUpdatedAttribute("serviceid", value); } } // Lookup - ServiceId - None

		private string _Subcategory;
		[CrmField("subcategory")]
		public string Subcategory { get { return _Subcategory; } set { _Subcategory=value; AddUpdatedAttribute("subcategory", value); } } // String - Subcategory - None

		private string _Subject;
		[CrmField("subject")]
		public string Subject { get { return _Subject; } set { _Subject=value; AddUpdatedAttribute("subject", value); } } // String - Subject - ApplicationRequired

		private bool _IsRegularActivity;
		[CrmField("isregularactivity", IsRequired = true)]
		public bool IsRegularActivity { get { return _IsRegularActivity; } set { _IsRegularActivity=value; AddUpdatedAttribute("isregularactivity", value); } } // Boolean - IsRegularActivity - SystemRequired

		private CrmEntityReference _OwningTeam;
		[CrmField("owningteam")]
		public CrmEntityReference OwningTeam { get { return _OwningTeam; } set { _OwningTeam=value; AddUpdatedAttribute("owningteam", value); } } // Lookup - OwningTeam - None

		private string _Description;
		[CrmField("description")]
		public string Description { get { return _Description; } set { _Description=value; AddUpdatedAttribute("description", value); } } // Memo - Description - None

		private string _Category;
		[CrmField("category")]
		public string Category { get { return _Category; } set { _Category=value; AddUpdatedAttribute("category", value); } } // String - Category - None

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - None

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private int _TimeZoneRuleVersionNumber;
		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get { return _TimeZoneRuleVersionNumber; } set { _TimeZoneRuleVersionNumber=value; AddUpdatedAttribute("timezoneruleversionnumber", value); } } // Integer - TimeZoneRuleVersionNumber - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private DateTime? _ScheduledStart;
		[CrmField("scheduledstart")]
		public DateTime? ScheduledStart { get { return _ScheduledStart; } set { _ScheduledStart=value; AddUpdatedAttribute("scheduledstart", value); } } // DateTime - ScheduledStart - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private string _IncidentIdName;
		[CrmField("incidentidname")]
		public string IncidentIdName { get { return _IncidentIdName; } set { _IncidentIdName=value; AddUpdatedAttribute("incidentidname", value); } } // String - IncidentIdName - None

		private string _OwnerIdYomiName;
		[CrmField("owneridyominame", IsRequired = true)]
		public string OwnerIdYomiName { get { return _OwnerIdYomiName; } set { _OwnerIdYomiName=value; AddUpdatedAttribute("owneridyominame", value); } } // String - OwnerIdYomiName - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private CrmEntityReference _IncidentId;
		[CrmField("incidentid")]
		public CrmEntityReference IncidentId { get { return _IncidentId; } set { _IncidentId=value; AddUpdatedAttribute("incidentid", value); } } // Lookup - IncidentId - None

		private DateTime? _ActualStart;
		[CrmField("actualstart")]
		public DateTime? ActualStart { get { return _ActualStart; } set { _ActualStart=value; AddUpdatedAttribute("actualstart", value); } } // DateTime - ActualStart - None

		private int _ScheduledDurationMinutes;
		[CrmField("scheduleddurationminutes")]
		public int ScheduledDurationMinutes { get { return _ScheduledDurationMinutes; } set { _ScheduledDurationMinutes=value; AddUpdatedAttribute("scheduleddurationminutes", value); } } // Integer - ScheduledDurationMinutes - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private DateTime? _ScheduledEnd;
		[CrmField("scheduledend")]
		public DateTime? ScheduledEnd { get { return _ScheduledEnd; } set { _ScheduledEnd=value; AddUpdatedAttribute("scheduledend", value); } } // DateTime - ScheduledEnd - None

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - None

		private int _ActualDurationMinutes;
		[CrmField("actualdurationminutes")]
		public int ActualDurationMinutes { get { return _ActualDurationMinutes; } set { _ActualDurationMinutes=value; AddUpdatedAttribute("actualdurationminutes", value); } } // Integer - ActualDurationMinutes - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private string _ActivityTypeCode;
		[CrmField("activitytypecode", IsRequired = true)]
		public string ActivityTypeCode { get { return _ActivityTypeCode; } set { _ActivityTypeCode=value; AddUpdatedAttribute("activitytypecode", value); } } // EntityName - ActivityTypeCode - SystemRequired

		private Guid _id;
		[CrmField("activityid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - ActivityId - SystemRequired

		private bool _IsWorkflowCreated;
		[CrmField("isworkflowcreated")]
		public bool IsWorkflowCreated { get { return _IsWorkflowCreated; } set { _IsWorkflowCreated=value; AddUpdatedAttribute("isworkflowcreated", value); } } // Boolean - IsWorkflowCreated - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private DateTime? _ActualEnd;
		[CrmField("actualend")]
		public DateTime? ActualEnd { get { return _ActualEnd; } set { _ActualEnd=value; AddUpdatedAttribute("actualend", value); } } // DateTime - ActualEnd - None

	}
}
