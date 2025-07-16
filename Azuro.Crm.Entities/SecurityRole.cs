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
	[CrmEntity("role")]
	public class SecurityRole : CrmEntity<SecurityRole> // 1036 - roleid - name
	{
		private CrmEntityReference _RoleTemplateId;
		[CrmField("roletemplateid")]
		public CrmEntityReference RoleTemplateId { get { return _RoleTemplateId; } set { _RoleTemplateId=value; AddUpdatedAttribute("roletemplateid", value); } } // Lookup - RoleTemplateId - None

		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private Guid _OrganizationId;
		[CrmField("organizationid", IsRequired = true)]
		public Guid OrganizationId { get { return _OrganizationId; } set { _OrganizationId=value; AddUpdatedAttribute("organizationid", value); } } // Uniqueidentifier - OrganizationId - SystemRequired

		private Guid _SupportingSolutionId;
		[CrmField("supportingsolutionid")]
		public Guid SupportingSolutionId { get { return _SupportingSolutionId; } set { _SupportingSolutionId=value; AddUpdatedAttribute("supportingsolutionid", value); } } // Uniqueidentifier - SupportingSolutionId - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private string _Name;
		[CrmField("name", IsRequired = true)]
		public string Name { get { return _Name; } set { _Name=value; AddUpdatedAttribute("name", value); } } // String - Name - SystemRequired

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private CrmEntityReference _BusinessUnitId;
		[CrmField("businessunitid", IsRequired = true)]
		public CrmEntityReference BusinessUnitId { get { return _BusinessUnitId; } set { _BusinessUnitId=value; AddUpdatedAttribute("businessunitid", value); } } // Lookup - BusinessUnitId - SystemRequired

		private Guid _id;
		[CrmField("roleid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - RoleId - SystemRequired

		private int _ComponentState;
		[CrmField("componentstate", IsRequired = true, IsPicklist = true)]
		public int ComponentState { get { return _ComponentState; } set { _ComponentState=value; AddUpdatedAttribute("componentstate", value); } } // Picklist - ComponentState - SystemRequired

		private string _ParentRootRoleIdName;
		[CrmField("parentrootroleidname", IsRequired = true)]
		public string ParentRootRoleIdName { get { return _ParentRootRoleIdName; } set { _ParentRootRoleIdName=value; AddUpdatedAttribute("parentrootroleidname", value); } } // String - ParentRootRoleIdName - SystemRequired

		private string _CreatedByYomiName;
		[CrmField("createdbyyominame")]
		public string CreatedByYomiName { get { return _CreatedByYomiName; } set { _CreatedByYomiName=value; AddUpdatedAttribute("createdbyyominame", value); } } // String - CreatedByYomiName - None

		private Guid _RoleIdUnique;
		[CrmField("roleidunique", IsRequired = true)]
		public Guid RoleIdUnique { get { return _RoleIdUnique; } set { _RoleIdUnique=value; AddUpdatedAttribute("roleidunique", value); } } // Uniqueidentifier - RoleIdUnique - SystemRequired

		private string _ModifiedByName;
		[CrmField("modifiedbyname")]
		public string ModifiedByName { get { return _ModifiedByName; } set { _ModifiedByName=value; AddUpdatedAttribute("modifiedbyname", value); } } // String - ModifiedByName - None

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private string _ParentRoleIdName;
		[CrmField("parentroleidname", IsRequired = true)]
		public string ParentRoleIdName { get { return _ParentRoleIdName; } set { _ParentRoleIdName=value; AddUpdatedAttribute("parentroleidname", value); } } // String - ParentRoleIdName - SystemRequired

		private bool _IsManaged;
		[CrmField("ismanaged", IsRequired = true)]
		public bool IsManaged { get { return _IsManaged; } set { _IsManaged=value; AddUpdatedAttribute("ismanaged", value); } } // Boolean - IsManaged - SystemRequired

		private DateTime _OverwriteTime;
		[CrmField("overwritetime", IsRequired = true)]
		public DateTime OverwriteTime { get { return _OverwriteTime; } set { _OverwriteTime=value; AddUpdatedAttribute("overwritetime", value); } } // DateTime - OverwriteTime - SystemRequired

		private Guid _SolutionId;
		[CrmField("solutionid", IsRequired = true)]
		public Guid SolutionId { get { return _SolutionId; } set { _SolutionId=value; AddUpdatedAttribute("solutionid", value); } } // Uniqueidentifier - SolutionId - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _BusinessUnitIdName;
		[CrmField("businessunitidname", IsRequired = true)]
		public string BusinessUnitIdName { get { return _BusinessUnitIdName; } set { _BusinessUnitIdName=value; AddUpdatedAttribute("businessunitidname", value); } } // String - BusinessUnitIdName - SystemRequired

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _OrganizationIdName;
		[CrmField("organizationidname", IsRequired = true)]
		public string OrganizationIdName { get { return _OrganizationIdName; } set { _OrganizationIdName=value; AddUpdatedAttribute("organizationidname", value); } } // String - OrganizationIdName - SystemRequired

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private CrmEntityReference _ParentRootRoleId;
		[CrmField("parentrootroleid", IsRequired = true)]
		public CrmEntityReference ParentRootRoleId { get { return _ParentRootRoleId; } set { _ParentRootRoleId=value; AddUpdatedAttribute("parentrootroleid", value); } } // Lookup - ParentRootRoleId - SystemRequired

		private DateTime? _OverriddenCreatedOn;
		[CrmField("overriddencreatedon")]
		public DateTime? OverriddenCreatedOn { get { return _OverriddenCreatedOn; } set { _OverriddenCreatedOn=value; AddUpdatedAttribute("overriddencreatedon", value); } } // DateTime - OverriddenCreatedOn - None

		private CrmEntityReference _ParentRoleId;
		[CrmField("parentroleid")]
		public CrmEntityReference ParentRoleId { get { return _ParentRoleId; } set { _ParentRoleId=value; AddUpdatedAttribute("parentroleid", value); } } // Lookup - ParentRoleId - None

	}
}
