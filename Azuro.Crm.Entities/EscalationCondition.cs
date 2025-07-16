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
	[CrmEntity("azuro_escalationcondition")]
	public class EscalationCondition : CrmEntity<EscalationCondition> // 10001 - azuro_escalationconditionid - azuro_name
	{
		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame", IsRequired = true)]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - SystemRequired

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame", IsRequired = true)]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - SystemRequired

		private int _statecode;
		[CrmField("statecode", IsRequired = true, IsPicklist = true)]
		public int statecode { get { return _statecode; } set { _statecode=value; AddUpdatedAttribute("statecode", value); } } // State - statecode - SystemRequired

		private CrmEntityReference _EscalationGroup;
		[CrmField("azuro_escalationgroupid")]
		public CrmEntityReference EscalationGroup { get { return _EscalationGroup; } set { _EscalationGroup=value; AddUpdatedAttribute("azuro_escalationgroupid", value); } } // Lookup - azuro_EscalationGroupId - None

		private int _OriginalSeverity;
		[CrmField("azuro_currentseverity", IsPicklist = true)]
		public int OriginalSeverity { get { return _OriginalSeverity; } set { _OriginalSeverity=value; AddUpdatedAttribute("azuro_currentseverity", value); } } // Picklist - azuro_CurrentSeverity - ApplicationRequired

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private Guid _id;
		[CrmField("azuro_escalationconditionid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - azuro_escalationconditionId - SystemRequired

		private string _azuro_escalationgroupidname;
		[CrmField("azuro_escalationgroupidname")]
		public string azuro_escalationgroupidname { get { return _azuro_escalationgroupidname; } set { _azuro_escalationgroupidname=value; AddUpdatedAttribute("azuro_escalationgroupidname", value); } } // String - azuro_EscalationGroupIdName - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _OrganizationIdName;
		[CrmField("organizationidname", IsRequired = true)]
		public string OrganizationIdName { get { return _OrganizationIdName; } set { _OrganizationIdName=value; AddUpdatedAttribute("organizationidname", value); } } // String - OrganizationIdName - SystemRequired

		private int _EscalationSeverity;
		[CrmField("azuro_escalationseverity", IsPicklist = true)]
		public int EscalationSeverity { get { return _EscalationSeverity; } set { _EscalationSeverity=value; AddUpdatedAttribute("azuro_escalationseverity", value); } } // Picklist - azuro_EscalationSeverity - ApplicationRequired

		private int _UTCConversionTimeZoneCode;
		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get { return _UTCConversionTimeZoneCode; } set { _UTCConversionTimeZoneCode=value; AddUpdatedAttribute("utcconversiontimezonecode", value); } } // Integer - UTCConversionTimeZoneCode - None

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame", IsRequired = true)]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - SystemRequired

		private int _statuscode;
		[CrmField("statuscode", IsPicklist = true)]
		public int statuscode { get { return _statuscode; } set { _statuscode=value; AddUpdatedAttribute("statuscode", value); } } // Status - statuscode - None

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

		private int _MeanTimeToRespond;
		[CrmField("azuro_meantimetorespond")]
		public int MeanTimeToRespond { get { return _MeanTimeToRespond; } set { _MeanTimeToRespond=value; AddUpdatedAttribute("azuro_meantimetorespond", value); } } // Integer - azuro_MeanTimeToRespond - ApplicationRequired

		private int _MeanTimeToRepair;
		[CrmField("azuro_meantimetorepair")]
		public int MeanTimeToRepair { get { return _MeanTimeToRepair; } set { _MeanTimeToRepair=value; AddUpdatedAttribute("azuro_meantimetorepair", value); } } // Integer - azuro_MeanTimeToRepair - ApplicationRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private CrmEntityReference _OrganizationId;
		[CrmField("organizationid")]
		public CrmEntityReference OrganizationId { get { return _OrganizationId; } set { _OrganizationId=value; AddUpdatedAttribute("organizationid", value); } } // Lookup - OrganizationId - None

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame", IsRequired = true)]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - SystemRequired

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _Name;
		[CrmField("azuro_name")]
		public string Name { get { return _Name; } set { _Name=value; AddUpdatedAttribute("azuro_name", value); } } // String - azuro_name - ApplicationRequired

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private bool? _ApplicableToAllCaseOrigins;
		[CrmField("azuro_applicabletoallcaseorigins")]
		public bool? ApplicableToAllCaseOrigins { get { return _ApplicableToAllCaseOrigins; } set { _ApplicableToAllCaseOrigins = value; AddUpdatedAttribute("azuro_applicabletoallcaseorigins", value); } } // bool - ApplicableToAllCaseOrigins - None

		private string _ApplicableCaseOrigins;
		[CrmField("azuro_applicablecaseoriginmulti")]
		public string ApplicableCaseOrigins { get { return _ApplicableCaseOrigins; } set { _ApplicableCaseOrigins = value; AddUpdatedAttribute("azuro_applicablecaseoriginmulti", value); } } // string - ApplicableCaseOrigins - None
	}
}
