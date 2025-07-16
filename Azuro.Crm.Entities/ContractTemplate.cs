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
	[CrmEntity("contracttemplate")]
	public class ContractTemplate : CrmEntity<ContractTemplate> // 2011 - contracttemplateid - name
	{
		private Guid _id;
		[CrmField("contracttemplateid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - ContractTemplateId - SystemRequired

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private int _AllotmentTypeCode;
		[CrmField("allotmenttypecode", IsPicklist = true)]
		public int AllotmentTypeCode { get { return _AllotmentTypeCode; } set { _AllotmentTypeCode=value; AddUpdatedAttribute("allotmenttypecode", value); } } // Picklist - AllotmentTypeCode - None

		private string _Description;
		[CrmField("description")]
		public string Description { get { return _Description; } set { _Description=value; AddUpdatedAttribute("description", value); } } // Memo - Description - None

		private Guid _SupportingSolutionId;
		[CrmField("supportingsolutionid")]
		public Guid SupportingSolutionId { get { return _SupportingSolutionId; } set { _SupportingSolutionId=value; AddUpdatedAttribute("supportingsolutionid", value); } } // Uniqueidentifier - SupportingSolutionId - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private int _BillingFrequencyCode;
		[CrmField("billingfrequencycode", IsPicklist = true)]
		public int BillingFrequencyCode { get { return _BillingFrequencyCode; } set { _BillingFrequencyCode=value; AddUpdatedAttribute("billingfrequencycode", value); } } // Picklist - BillingFrequencyCode - None

		private bool _UseDiscountAsPercentage;
		[CrmField("usediscountaspercentage")]
		public bool UseDiscountAsPercentage { get { return _UseDiscountAsPercentage; } set { _UseDiscountAsPercentage=value; AddUpdatedAttribute("usediscountaspercentage", value); } } // Boolean - UseDiscountAsPercentage - None

		private string _EffectivityCalendar;
		[CrmField("effectivitycalendar")]
		public string EffectivityCalendar { get { return _EffectivityCalendar; } set { _EffectivityCalendar=value; AddUpdatedAttribute("effectivitycalendar", value); } } // String - EffectivityCalendar - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _OrganizationIdName;
		[CrmField("organizationidname", IsRequired = true)]
		public string OrganizationIdName { get { return _OrganizationIdName; } set { _OrganizationIdName=value; AddUpdatedAttribute("organizationidname", value); } } // String - OrganizationIdName - SystemRequired

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private int _ComponentState;
		[CrmField("componentstate", IsRequired = true, IsPicklist = true)]
		public int ComponentState { get { return _ComponentState; } set { _ComponentState=value; AddUpdatedAttribute("componentstate", value); } } // Picklist - ComponentState - SystemRequired

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private int _ContractServiceLevelCode;
		[CrmField("contractservicelevelcode", IsPicklist = true)]
		public int ContractServiceLevelCode { get { return _ContractServiceLevelCode; } set { _ContractServiceLevelCode=value; AddUpdatedAttribute("contractservicelevelcode", value); } } // Picklist - ContractServiceLevelCode - None

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

		private bool _IsManaged;
		[CrmField("ismanaged", IsRequired = true)]
		public bool IsManaged { get { return _IsManaged; } set { _IsManaged=value; AddUpdatedAttribute("ismanaged", value); } } // Boolean - IsManaged - SystemRequired

		private DateTime _OverwriteTime;
		[CrmField("overwritetime", IsRequired = true)]
		public DateTime OverwriteTime { get { return _OverwriteTime; } set { _OverwriteTime=value; AddUpdatedAttribute("overwritetime", value); } } // DateTime - OverwriteTime - SystemRequired

		private string _Name;
		[CrmField("name", IsRequired = true)]
		public string Name { get { return _Name; } set { _Name=value; AddUpdatedAttribute("name", value); } } // String - Name - SystemRequired

		private Guid _SolutionId;
		[CrmField("solutionid", IsRequired = true)]
		public Guid SolutionId { get { return _SolutionId; } set { _SolutionId=value; AddUpdatedAttribute("solutionid", value); } } // Uniqueidentifier - SolutionId - SystemRequired

		private Guid _ContractTemplateIdUnique;
		[CrmField("contracttemplateidunique", IsRequired = true)]
		public Guid ContractTemplateIdUnique { get { return _ContractTemplateIdUnique; } set { _ContractTemplateIdUnique=value; AddUpdatedAttribute("contracttemplateidunique", value); } } // Uniqueidentifier - ContractTemplateIdUnique - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private CrmEntityReference _OrganizationId;
		[CrmField("organizationid", IsRequired = true)]
		public CrmEntityReference OrganizationId { get { return _OrganizationId; } set { _OrganizationId=value; AddUpdatedAttribute("organizationid", value); } } // Lookup - OrganizationId - SystemRequired

		private string _Abbreviation;
		[CrmField("abbreviation", IsRequired = true)]
		public string Abbreviation { get { return _Abbreviation; } set { _Abbreviation=value; AddUpdatedAttribute("abbreviation", value); } } // String - Abbreviation - SystemRequired

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

	}
}
