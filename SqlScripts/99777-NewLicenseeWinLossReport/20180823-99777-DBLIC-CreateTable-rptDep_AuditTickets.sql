USE bodb_LicenseeReport
GO

/*
	 Creator: 20180820@Amanda
	 Task: Create new table dbo.rptDep_AuditTickets - [Redmine: #99777]
	 Database: DBLIC.bodb_LicenseeReport

	 Revisions:
	 -20180820@Amanda: Create new table dbo.rptDep_AuditTickets [redmine:#99777]
*/
GO
CREATE TABLE [dbo].[rptDep_AuditTickets]
(
	[PlayerNo]			INT				NOT NULL
   ,[PlayerName]		VARCHAR(100)
   ,[PlayerType]		VARCHAR(10)	 
   ,[TransactionDate]	DATETIME
   ,[TransactionId]		BIGINT			NOT NULL	
   ,[SettlementDate]	DATETIME		NOT NULL
   ,[Brand]				VARCHAR(50)
   ,[BrandUrl]			VARCHAR(100)
   ,[GameCode]			VARCHAR(10)
   ,[GameName]			VARCHAR(100)
   ,[GameType]			VARCHAR(50)		NOT NULL
   ,[CurrencyCode]		VARCHAR(10)
   ,[BetType]			VARCHAR(100)
   ,[BetAmount]		MONEY
   ,[DebitAmount]		MONEY
   ,[CreditAmount]		MONEY
   ,[NetAmount]		MONEY
	CONSTRAINT [PK_rptDep_AuditTickets] PRIMARY KEY NONCLUSTERED
	(
		[TransactionId] ASC
	) ON [LicenseeData]
)

CREATE CLUSTERED INDEX [CIX_playerNo] ON [dbo].[rptDep_AuditTickets]
(
	[PlayerNo] ASC,
	[SettlementDate] ASC
) ON [LicenseeData]

GO
