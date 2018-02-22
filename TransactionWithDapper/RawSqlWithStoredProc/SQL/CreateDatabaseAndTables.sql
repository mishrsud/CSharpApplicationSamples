CREATE DATABASE Organisation
GO

USE Organisation
GO

/****** Object:  Table [dbo].[Deal]    Script Date: 9/02/2018 15:35:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Deal](
	[DealId] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](18, 0) NULL,
	[DealUserId] [uniqueidentifier] NULL,
	[ReceivedDate] [datetime] NULL,
	[PaidDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[DealId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[VirtualAccount]    Script Date: 9/02/2018 15:35:41 ******/

CREATE TABLE [dbo].[VirtualAccount](
	[VirtualAccountId] [uniqueidentifier] NOT NULL,
	[VirtualAccountNumber] [varchar](20) NULL,
	[HeaderAccountId] [uniqueidentifier] NULL,
	[VirtualAccountUserId] [uniqueidentifier] NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[VirtualAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[VirtualAccountBalance]    Script Date: 9/02/2018 15:36:23 ******/

CREATE TABLE [dbo].[VirtualAccountBalance](
	[VirtualAccountBalanceId] [uniqueidentifier] NOT NULL,
	[VirtualAccountId] [uniqueidentifier] NULL,
	[Amount] [decimal](18, 0) NULL,
	[Entered] [datetime] NOT NULL,
	[EnteredBy] [int] NULL,
	[Updated] [datetime] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[VirtualAccountBalanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[U_VirtualAccountDeals]    Script Date: 9/02/2018 15:36:58 ******/


CREATE TABLE [dbo].[U_VirtualAccountDeals](
	[UVAD_ID] [int] IDENTITY(1,1) NOT NULL,
	[UVAD_GUID] [uniqueidentifier] NOT NULL,
	[UVA_GUID] [uniqueidentifier] NOT NULL,
	[RD_GUID] [uniqueidentifier] NOT NULL,
	[UVAD_Entered] [datetime] NOT NULL,
	[UVAD_EnteredBy] [int] NOT NULL,
	[UVAD_Updated] [datetime] NOT NULL,
	[UVAD_UpdatedBy] [int] NOT NULL,
	[UVAD_Status] [bit] NOT NULL,
 CONSTRAINT [PK_U_VirtualAccountDeals_UVAD_ID] PRIMARY KEY CLUSTERED 
(
	[UVAD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_U_VirtualAccountDeals_UVAD_GUID] UNIQUE NONCLUSTERED 
(
	[UVAD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
