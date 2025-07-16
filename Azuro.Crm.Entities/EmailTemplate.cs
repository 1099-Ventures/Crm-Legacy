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
	[CrmEntity("template")]
	public class EmailTemplate : CrmEntity<EmailTemplate> // 2010 - templateid - title
	{
		private string _CreatedOnBehalfByYomiName;
		[CrmField("createdonbehalfbyyominame")]
		public string CreatedOnBehalfByYomiName { get { return _CreatedOnBehalfByYomiName; } set { _CreatedOnBehalfByYomiName=value; AddUpdatedAttribute("createdonbehalfbyyominame", value); } } // String - CreatedOnBehalfByYomiName - None

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - None

		private string _ModifiedByYomiName;
		[CrmField("modifiedbyyominame")]
		public string ModifiedByYomiName { get { return _ModifiedByYomiName; } set { _ModifiedByYomiName=value; AddUpdatedAttribute("modifiedbyyominame", value); } } // String - ModifiedByYomiName - None

		private CrmEntityReference _ModifiedOnBehalfBy;
		[CrmField("modifiedonbehalfby")]
		public CrmEntityReference ModifiedOnBehalfBy { get { return _ModifiedOnBehalfBy; } set { _ModifiedOnBehalfBy=value; AddUpdatedAttribute("modifiedonbehalfby", value); } } // Lookup - ModifiedOnBehalfBy - None

		private string _OwnerIdName;
		[CrmField("owneridname", IsRequired = true)]
		public string OwnerIdName { get { return _OwnerIdName; } set { _OwnerIdName=value; AddUpdatedAttribute("owneridname", value); } } // String - OwnerIdName - SystemRequired

		private string _Description;
		[CrmField("description")]
		public string Description { get { return _Description; } set { _Description=value; AddUpdatedAttribute("description", value); } } // Memo - Description - None

		private Guid _SupportingSolutionId;
		[CrmField("supportingsolutionid")]
		public Guid SupportingSolutionId { get { return _SupportingSolutionId; } set { _SupportingSolutionId=value; AddUpdatedAttribute("supportingsolutionid", value); } } // Uniqueidentifier - SupportingSolutionId - None

		private CrmEntityReference _CreatedOnBehalfBy;
		[CrmField("createdonbehalfby")]
		public CrmEntityReference CreatedOnBehalfBy { get { return _CreatedOnBehalfBy; } set { _CreatedOnBehalfBy=value; AddUpdatedAttribute("createdonbehalfby", value); } } // Lookup - CreatedOnBehalfBy - None

		private string _CreatedByName;
		[CrmField("createdbyname")]
		public string CreatedByName { get { return _CreatedByName; } set { _CreatedByName=value; AddUpdatedAttribute("createdbyname", value); } } // String - CreatedByName - None

		private string _Body;
		[CrmField("body")]
		public string Body { get { return _Body; } set { _Body=value; AddUpdatedAttribute("body", value); } } // Memo - Body - None

		private CrmEntityReference _ModifiedBy;
		[CrmField("modifiedby")]
		public CrmEntityReference ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy=value; AddUpdatedAttribute("modifiedby", value); } } // Lookup - ModifiedBy - None

		private DateTime _OverwriteTime;
		[CrmField("overwritetime", IsRequired = true)]
		public DateTime OverwriteTime { get { return _OverwriteTime; } set { _OverwriteTime=value; AddUpdatedAttribute("overwritetime", value); } } // DateTime - OverwriteTime - SystemRequired

		private int _ImportSequenceNumber;
		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get { return _ImportSequenceNumber; } set { _ImportSequenceNumber=value; AddUpdatedAttribute("importsequencenumber", value); } } // Integer - ImportSequenceNumber - None

		private string _Subject;
		[CrmField("subject")]
		public string Subject { get { return _Subject; } set { _Subject=value; AddUpdatedAttribute("subject", value); } } // Memo - Subject - ApplicationRequired

		private int _ComponentState;
		[CrmField("componentstate", IsRequired = true, IsPicklist = true)]
		public int ComponentState { get { return _ComponentState; } set { _ComponentState=value; AddUpdatedAttribute("componentstate", value); } } // Picklist - ComponentState - SystemRequired

		private CrmEntityReference _OwningTeam;
		[CrmField("owningteam")]
		public CrmEntityReference OwningTeam { get { return _OwningTeam; } set { _OwningTeam=value; AddUpdatedAttribute("owningteam", value); } } // Lookup - OwningTeam - None

		private bool _IsPersonal;
		[CrmField("ispersonal")]
		public bool IsPersonal { get { return _IsPersonal; } set { _IsPersonal=value; AddUpdatedAttribute("ispersonal", value); } } // Boolean - IsPersonal - None

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

		private string _MimeType;
		[CrmField("mimetype")]
		public string MimeType { get { return _MimeType; } set { _MimeType=value; AddUpdatedAttribute("mimetype", value); } } // String - MimeType - None

		private CrmEntityReference _CreatedBy;
		[CrmField("createdby")]
		public CrmEntityReference CreatedBy { get { return _CreatedBy; } set { _CreatedBy=value; AddUpdatedAttribute("createdby", value); } } // Lookup - CreatedBy - None

		private Guid _TemplateIdUnique;
		[CrmField("templateidunique", IsRequired = true)]
		public Guid TemplateIdUnique { get { return _TemplateIdUnique; } set { _TemplateIdUnique=value; AddUpdatedAttribute("templateidunique", value); } } // Uniqueidentifier - TemplateIdUnique - SystemRequired

		private string _SubjectPresentationXml;
		[CrmField("subjectpresentationxml")]
		public string SubjectPresentationXml { get { return _SubjectPresentationXml; } set { _SubjectPresentationXml=value; AddUpdatedAttribute("subjectpresentationxml", value); } } // Memo - SubjectPresentationXml - None

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private string _PresentationXml;
		[CrmField("presentationxml")]
		public string PresentationXml { get { return _PresentationXml; } set { _PresentationXml=value; AddUpdatedAttribute("presentationxml", value); } } // Memo - PresentationXml - None

		private int _LanguageCode;
		[CrmField("languagecode")]
		public int LanguageCode { get { return _LanguageCode; } set { _LanguageCode=value; AddUpdatedAttribute("languagecode", value); } } // Integer - LanguageCode - ApplicationRequired

		private Guid _SolutionId;
		[CrmField("solutionid", IsRequired = true)]
		public Guid SolutionId { get { return _SolutionId; } set { _SolutionId=value; AddUpdatedAttribute("solutionid", value); } } // Uniqueidentifier - SolutionId - SystemRequired

		private string _OwnerIdYomiName;
		[CrmField("owneridyominame", IsRequired = true)]
		public string OwnerIdYomiName { get { return _OwnerIdYomiName; } set { _OwnerIdYomiName=value; AddUpdatedAttribute("owneridyominame", value); } } // String - OwnerIdYomiName - SystemRequired

		private DateTime? _ModifiedOn;
		[CrmField("modifiedon")]
		public DateTime? ModifiedOn { get { return _ModifiedOn; } set { _ModifiedOn=value; AddUpdatedAttribute("modifiedon", value); } } // DateTime - ModifiedOn - None

		private string _Title;
		[CrmField("title")]
		public string Title { get { return _Title; } set { _Title=value; AddUpdatedAttribute("title", value); } } // String - Title - ApplicationRequired

		private string _ModifiedOnBehalfByYomiName;
		[CrmField("modifiedonbehalfbyyominame")]
		public string ModifiedOnBehalfByYomiName { get { return _ModifiedOnBehalfByYomiName; } set { _ModifiedOnBehalfByYomiName=value; AddUpdatedAttribute("modifiedonbehalfbyyominame", value); } } // String - ModifiedOnBehalfByYomiName - None

		private DateTime? _CreatedOn;
		[CrmField("createdon")]
		public DateTime? CreatedOn { get { return _CreatedOn; } set { _CreatedOn=value; AddUpdatedAttribute("createdon", value); } } // DateTime - CreatedOn - None

		private string _TemplateTypeCode;
		[CrmField("templatetypecode", IsRequired = true)]
		public string TemplateTypeCode { get { return _TemplateTypeCode; } set { _TemplateTypeCode=value; AddUpdatedAttribute("templatetypecode", value); } } // EntityName - TemplateTypeCode - SystemRequired

		private int _GenerationTypeCode;
		[CrmField("generationtypecode")]
		public int GenerationTypeCode { get { return _GenerationTypeCode; } set { _GenerationTypeCode=value; AddUpdatedAttribute("generationtypecode", value); } } // Integer - GenerationTypeCode - None

		private string _CreatedOnBehalfByName;
		[CrmField("createdonbehalfbyname")]
		public string CreatedOnBehalfByName { get { return _CreatedOnBehalfByName; } set { _CreatedOnBehalfByName=value; AddUpdatedAttribute("createdonbehalfbyname", value); } } // String - CreatedOnBehalfByName - None

		private Guid _id;
		[CrmField("templateid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - TemplateId - SystemRequired

		private string _ModifiedOnBehalfByName;
		[CrmField("modifiedonbehalfbyname")]
		public string ModifiedOnBehalfByName { get { return _ModifiedOnBehalfByName; } set { _ModifiedOnBehalfByName=value; AddUpdatedAttribute("modifiedonbehalfbyname", value); } } // String - ModifiedOnBehalfByName - None

		private bool _IsManaged;
		[CrmField("ismanaged", IsRequired = true)]
		public bool IsManaged { get { return _IsManaged; } set { _IsManaged=value; AddUpdatedAttribute("ismanaged", value); } } // Boolean - IsManaged - SystemRequired

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid", IsRequired = true)]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - SystemRequired

	}
}
