using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Crm.Integration;

namespace Azuro.Crm.Entities
{
	[CrmEntity("azuro_smscampaignactivity")]
	public class SmsCampaignActivity : CrmEntity<SmsCampaignActivity>
	{
		[CrmField("azuro_smscampaignactivityid", true)]
		public Guid Id { get; set; }

		[CrmField("createdby")]
		public string CreatedBy { get; set; }

		[CrmField("createdon")]
		public DateTime CreatedOn { get; set; }

		[CrmField("createdonbehalfby")]
		public string CreatedOnBehalfBy { get; set; }

		[CrmField("exchangerate")]
		public float ExchangeRate { get; set; }

		[CrmField("importsequencenumber")]
		public int ImportSequenceNumber { get; set; }

		[CrmField("modifiedby")]
		public string ModifiedBy { get; set; }

		[CrmField("modifiedon")]
		public DateTime ModifiedOn { get; set; }

		[CrmField("modifiedonbehalfby")]
		public string ModifiedOnBehalfBy { get; set; }

		[CrmField("azuro_actualcontent")]
		public decimal ActualContent { get; set; }

		[CrmField("azuro_actualend")]
		public DateTime ActualEnd { get; set; }

		[CrmField("azuro_actualstart")]
		public DateTime ActualStart { get; set; }

		[CrmField("azuro_budgetallocated")]
		public decimal BudgetAllocated { get; set; }

		[CrmField("azuro_campaignstatusreason")]
		public string CampaignStatusReason { get; set; }

		[CrmField("azuro_content")]
		public string Content { get; set; }

		[CrmField("azuro_createdsmsowneroptions")]
		public string CreatedSMSOwnerOptions { get; set; }

		[CrmField("azuro_name")]
		public string Name { get; set; }

		[CrmField("azuro_scheduledend")]
		public DateTime ScheduledEnd { get; set; }

		[CrmField("azuro_scheduledstart")]
		public DateTime ScheduledStart { get; set; }

		[CrmField("azuro_type")]
		public string Type { get; set; }

		[CrmField("azuro_campaignstatusreason")]
		public SmsCampaignStatusReason StatusReason { get; set; }

		[CrmField("overriddencreatedon")]
		public DateTime OverriddenCreatedOn { get; set; }

		[CrmField("ownerid")]
		public string OwnerId { get; set; }

		[CrmField("owneridname")]
		public string OwnerIdName { get; set; }

		[CrmField("owneridtype")]
		public int OwnerIdType { get; set; }

		[CrmField("owneridyominame")]
		public string OwnerIdYomiName { get; set; }

		[CrmField("owningbusinessunit")]
		public string OwningBusinessUnit { get; set; }

		[CrmField("timezoneruleversionnumber")]
		public int TimeZoneRuleVersionNumber { get; set; }

		[CrmField("utcconversiontimezonecode")]
		public int UTCConversionTimeZoneCode { get; set; }

		[CrmField("versionnumber")]
		public long VersionNumber { get; set; }
	}
}
