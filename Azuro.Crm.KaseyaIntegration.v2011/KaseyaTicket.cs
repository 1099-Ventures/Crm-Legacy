using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Azuro.Crm.KaseyaIntegration
{
	[KaseyaEntity("Ticket")]
	public class KaseyaTicket : KaseyaEntity<KaseyaTicket>
	{
		[KaseyaField("TicketID", "ticketnumber", IsIdentity = true)]
		public string TicketID { get; set; }

		[KaseyaField("Assignee", "responsiblecontactid")]
		public string Assignee { get; set; }

		[KaseyaField("CreatedBy", "createdby")]
		public string CreatedBy { get; set; }

		[KaseyaField("CreationDate", "createdon")]
		public string CreationDate { get; set; }

		[KaseyaField("DueDate")]
		public string DueDate { get; set; }

		[KaseyaField("groupName")]
		public string GroupName { get; set; }

		[KaseyaField("machName", "")]
		public string machName { get; set; }

		[KaseyaField("TicketSummary", "title")]
		public string TicketSummary { get; set; }

		[KaseyaField("TransactionID")]
		public decimal TransactionID { get; set; }

		[KaseyaField("UserName", "")]
		public string UserName { get; set; }

		[KaseyaField("UserEmail")]
		public string UserEmail { get; set; }

		[KaseyaField("UserPhone")]
		public string UserPhone { get; set; }

		[KaseyaField("Category", "productid")]
		public string Category { get; set; }

		[KaseyaField("Status")]
		public string Status { get; set; }

		[KaseyaField("Priority", "severitycode")]
		public string Priority { get; set; }

		[KaseyaField("SLA Type", "contractid")]
		public string SLAType { get; set; }

		[KaseyaField("Dispatch Tech")]
		public string DispatchTech { get; set; }

		[KaseyaField("Approval")]
		public string Approval { get; set; }

		[KaseyaField("Hours Worked")]
		public string HoursWorked { get; set; }
	}
}
