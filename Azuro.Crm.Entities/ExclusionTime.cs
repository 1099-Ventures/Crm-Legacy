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
	[CrmEntity("azuro_exclusiontime")]
	public class ExclusionTime : CrmEntity<ExclusionTime> // 10000 - azuro_exclusiontimeid - azuro_name
	{
		[CrmField("organizationid")]
		public CrmEntityReference OrganizationId { get; set; } // Lookup - OrganizationId - None

		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get; set; } // Lookup - ModifiedOnBehalfBy - None

		[CrmField("azuro_dontsms")]
		public bool Dontsms { get; set; } // Boolean - azuro_Dontsms - None

		[CrmField("statecode", IsRequired=true)]
		public int Status { get; set; } // State - statecode - SystemRequired

		[CrmField("azuro_description")]
		public string Description { get; set; } // Memo - azuro_Description - None

		[CrmField("statecodename")]
		public string statecodename { get; set; } // Virtual - statecodeName - None

		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get; set; } // Lookup - CreatedOnBehalfBy - None

		[CrmField("azuro_timetoname")]
		public string azuro_timetoname { get; set; } // Virtual - azuro_timetoName - None

		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get; set; } // Lookup - ModifiedBy - None

		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get; set; } // Integer - ImportSequenceNumber - None

		[CrmField("azuro_timefromname")]
		public string azuro_timefromname { get; set; } // Virtual - azuro_timefromName - None

		[CrmField("azuro_timeto")]
		public int Timeto { get; set; } // Picklist - azuro_Timeto - ApplicationRequired

		[CrmField("createdonbehalfbyyominame", IsRequired=true)]
		public string createdonbehalfbyyominame { get; set; } // String - CreatedOnBehalfByYomiName - SystemRequired

		[CrmField("azuro_exclusiontimeid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - azuro_exclusiontimeId - SystemRequired

		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get; set; } // Integer - UTCConversionTimeZoneCode - None

		[CrmField("createdbyyominame", IsRequired=true)]
		public string createdbyyominame { get; set; } // String - CreatedByYomiName - SystemRequired

		[CrmField("statuscode")]
		public int StatusReason { get; set; } // Status - statuscode - None

		[CrmField("modifiedbyname")]
		public string modifiedbyname { get; set; } // String - ModifiedByName - None

		[CrmField("versionnumber")]
		public long versionnumber { get; set; } // BigInt - VersionNumber - None

		[CrmField("modifiedbyyominame", IsRequired=true)]
		public string modifiedbyyominame { get; set; } // String - ModifiedByYomiName - SystemRequired

		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get; set; } // Lookup - CreatedBy - None

		[CrmField("azuro_dontsmsname")]
		public string azuro_dontsmsname { get; set; } // Virtual - azuro_dontsmsName - None

		[CrmField("statuscodename")]
		public string statuscodename { get; set; } // Virtual - statuscodeName - None

		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get; set; } // Integer - TimeZoneRuleVersionNumber - None

		[CrmField("organizationidname", IsRequired=true)]
		public string organizationidname { get; set; } // String - OrganizationIdName - SystemRequired

		[CrmField("modifiedon")]
		public DateTime ModifiedOn { get; set; } // DateTime - ModifiedOn - None

		[CrmField("modifiedonbehalfbyyominame", IsRequired=true)]
		public string modifiedonbehalfbyyominame { get; set; } // String - ModifiedOnBehalfByYomiName - SystemRequired

		[CrmField("createdbyname")]
		public string createdbyname { get; set; } // String - CreatedByName - None

		[CrmField("createdon")]
		public DateTime CreatedOn { get; set; } // DateTime - CreatedOn - None

		[CrmField("azuro_timefrom")]
		public int TimeFrom { get; set; } // Picklist - azuro_TimeFrom - ApplicationRequired

		[CrmField("createdonbehalfbyname")]
		public string createdonbehalfbyname { get; set; } // String - CreatedOnBehalfByName - None

		[CrmField("azuro_name")]
		public string Name { get; set; } // String - azuro_name - ApplicationRequired

		[CrmField("modifiedonbehalfbyname")]
		public string modifiedonbehalfbyname { get; set; } // String - ModifiedOnBehalfByName - None

		[CrmField("overriddencreatedon")]
		public DateTime RecordCreatedOn { get; set; } // DateTime - OverriddenCreatedOn - None

	}
}
