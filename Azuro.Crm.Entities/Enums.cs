using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.Entities
{
	public enum SmsProvider
	{
		Clickatell = 100000000,
		Integrat = 100000001,
	};

	public enum SmsStatus
	{
		Created = 100000000,
		Sent = 100000001,
		Delivered = 100000002,
		Send = 100000003,
		Failed = 100000004,
	};

	public enum SmsActivityStatus
	{
		Open = 0,
		Completed = 1,
		Canceled = 2,
		Scheduled = 3,
	};

	public enum SmsStatusReason
	{
		Open = 1,
		Completed = 2,
		Canceled = 3,
		Scheduled = 4,
	};

	public enum SmsPriority
	{
		Low = 0,
		Medium = 1,
		High = 2,
	};

	//public enum SmsDirection : byte
	//{
	//    Incoming = 0,
	//    Outgoing = 1,
	//};

	public enum SmsCampaignStatusReason
	{
		Proposed = 100000000,
		Pending = 100000001,
		Executed = 100000004,
		Closed = 100000002,
		Cancelled = 100000003,
	};

	public enum AllowMultipartMessages
	{
		[System.ComponentModel.DefaultValue(0)]
		None = 100000000,
		[System.ComponentModel.DefaultValue(1)]
		One = 100000001,
		[System.ComponentModel.DefaultValue(2)]
		Two = 100000002,
		[System.ComponentModel.DefaultValue(3)]
		Three = 100000003,
	};

	public enum NotificationEventType
	{
		Creation = 334070000,
		Escalation = 334070002,
		Reassignment = 334070003,
		Assignment = 334070006,
		Resolution = 334070005,
		StatusChange = 334070004,
		Updated = 334070001,
	};

	public enum CommunicationChannel
	{
		Email = 334070000,
		Sms = 334070001,
	};
}
