USE [AzuroSMS]
GO

/****** Object:  Table [dbo].[SmsSendLog]    Script Date: 05/26/2012 01:37:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SmsSendLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[ActivityId] [uniqueidentifier] NOT NULL,
	[MobileNumber] [nvarchar](20) NULL,
	[DateSent] [datetime] NULL,
	[DateDelivered] [datetime] NULL,
	[Provider] [nvarchar](50) NULL,
	[ProviderId] [nvarchar](50) NULL,
	[ProviderStatus] [nvarchar](10) NULL,
	[ProviderStatusMessage] [nvarchar](100) NULL,
	[Message] [nvarchar](500) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateChanged] [datetime] NOT NULL,
 CONSTRAINT [PK_SmsSendLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


