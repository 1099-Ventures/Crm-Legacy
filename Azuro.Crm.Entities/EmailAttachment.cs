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
	[CrmEntity("activitymimeattachment")]
	public class EMailAttachment : CrmEntity<EMailAttachment> // 1001 - activitymimeattachmentid - filename
	{
		private int _ComponentState;
		[CrmField("componentstate", IsRequired = true, IsPicklist = true)]
		public int ComponentState { get { return _ComponentState; } set { _ComponentState=value; AddUpdatedAttribute("componentstate", value); } } // Picklist - ComponentState - SystemRequired

		private Guid _id;
		[CrmField("activitymimeattachmentid", true)]
		public Guid Id { get; set; } // Uniqueidentifier - ActivityMimeAttachmentId - SystemRequired

		private CrmEntityReference _ActivityId;
		[CrmField("activityid")]
		public CrmEntityReference ActivityId { get { return _ActivityId; } set { _ActivityId=value; AddUpdatedAttribute("activityid", value); } } // Lookup - ActivityId - None

		private Guid _SupportingSolutionId;
		[CrmField("supportingsolutionid")]
		public Guid SupportingSolutionId { get { return _SupportingSolutionId; } set { _SupportingSolutionId=value; AddUpdatedAttribute("supportingsolutionid", value); } } // Uniqueidentifier - SupportingSolutionId - None

		private CrmEntityReference _ObjectId;
		[CrmField("objectid", IsRequired = true)]
		public CrmEntityReference ObjectId { get { return _ObjectId; } set { _ObjectId=value; AddUpdatedAttribute("objectid", value); } } // Lookup - ObjectId - SystemRequired

		private string _FileName;
		[CrmField("filename")]
		public string FileName { get { return _FileName; } set { _FileName=value; AddUpdatedAttribute("filename", value); } } // String - FileName - None

		private int _AttachmentNumber;
		[CrmField("attachmentnumber", IsRequired = true)]
		public int AttachmentNumber { get { return _AttachmentNumber; } set { _AttachmentNumber=value; AddUpdatedAttribute("attachmentnumber", value); } } // Integer - AttachmentNumber - SystemRequired

		private long _VersionNumber;
		[CrmField("versionnumber")]
		public long VersionNumber { get { return _VersionNumber; } set { _VersionNumber=value; AddUpdatedAttribute("versionnumber", value); } } // BigInt - VersionNumber - None

		private string _ObjectTypeCode;
		[CrmField("objecttypecode", IsRequired = true)]
		public string ObjectTypeCode { get { return _ObjectTypeCode; } set { _ObjectTypeCode=value; AddUpdatedAttribute("objecttypecode", value); } } // EntityName - ObjectTypeCode - SystemRequired

		private string _Subject;
		[CrmField("subject")]
		public string Subject { get { return _Subject; } set { _Subject=value; AddUpdatedAttribute("subject", value); } } // String - Subject - None

		private CrmEntityReference _OwningUser;
		[CrmField("owninguser")]
		public CrmEntityReference OwningUser { get { return _OwningUser; } set { _OwningUser=value; AddUpdatedAttribute("owninguser", value); } } // Lookup - OwningUser - None

		private DateTime _OverwriteTime;
		[CrmField("overwritetime", IsRequired = true)]
		public DateTime OverwriteTime { get { return _OverwriteTime; } set { _OverwriteTime=value; AddUpdatedAttribute("overwritetime", value); } } // DateTime - OverwriteTime - SystemRequired

		private CrmEntityReference _OwningBusinessUnit;
		[CrmField("owningbusinessunit")]
		public CrmEntityReference OwningBusinessUnit { get { return _OwningBusinessUnit; } set { _OwningBusinessUnit=value; AddUpdatedAttribute("owningbusinessunit", value); } } // Lookup - OwningBusinessUnit - None

		private Guid _ActivityMimeAttachmentIdUnique;
		[CrmField("activitymimeattachmentidunique", IsRequired = true)]
		public Guid ActivityMimeAttachmentIdUnique { get { return _ActivityMimeAttachmentIdUnique; } set { _ActivityMimeAttachmentIdUnique=value; AddUpdatedAttribute("activitymimeattachmentidunique", value); } } // Uniqueidentifier - ActivityMimeAttachmentIdUnique - SystemRequired

		private string _OwnerIdType;
		[CrmField("owneridtype", IsRequired = true)]
		public string OwnerIdType { get { return _OwnerIdType; } set { _OwnerIdType=value; AddUpdatedAttribute("owneridtype", value); } } // EntityName - OwnerIdType - SystemRequired

		private string _Body;
		[CrmField("body")]
		public string Body { get { return _Body; } set { _Body=value; AddUpdatedAttribute("body", value); } } // String - Body - None

		private Guid _SolutionId;
		[CrmField("solutionid", IsRequired = true)]
		public Guid SolutionId { get { return _SolutionId; } set { _SolutionId=value; AddUpdatedAttribute("solutionid", value); } } // Uniqueidentifier - SolutionId - SystemRequired

		private string _MimeType;
		[CrmField("mimetype")]
		public string MimeType { get { return _MimeType; } set { _MimeType=value; AddUpdatedAttribute("mimetype", value); } } // String - MimeType - None

		private int _FileSize;
		[CrmField("filesize")]
		public int FileSize { get { return _FileSize; } set { _FileSize=value; AddUpdatedAttribute("filesize", value); } } // Integer - FileSize - None

		private CrmEntityReference _OwnerId;
		[CrmField("ownerid")]
		public CrmEntityReference OwnerId { get { return _OwnerId; } set { _OwnerId=value; AddUpdatedAttribute("ownerid", value); } } // Owner - OwnerId - ApplicationRequired

		private CrmEntityReference _AttachmentId;
		[CrmField("attachmentid")]
		public CrmEntityReference AttachmentId { get { return _AttachmentId; } set { _AttachmentId=value; AddUpdatedAttribute("attachmentid", value); } } // Lookup - AttachmentId - ApplicationRequired

		private bool _IsManaged;
		[CrmField("ismanaged", IsRequired = true)]
		public bool IsManaged { get { return _IsManaged; } set { _IsManaged=value; AddUpdatedAttribute("ismanaged", value); } } // Boolean - IsManaged - SystemRequired

	}
}
