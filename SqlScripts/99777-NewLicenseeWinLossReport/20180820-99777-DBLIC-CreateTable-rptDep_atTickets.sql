USE bodb_LicenseeReport
/*
	 Creator: 20180820@Amanda
	 Task: Create new table dbo.rptDep_atTickets - [Redmine: #99777]
	 Database: DBLIC.bodb_LicenseeReport

	 Revisions:
	 -20180820@Amanda: Create new table dbo.rptDep_atTickets [redmine:#99777]
*/
GO
CREATE TABLE [dbo].[rptDep_atTickets] 
(
	[player_no]			VARCHAR(50)  NOT NULL
   ,[player_name]		VARCHAR(100)
   ,[player_type]		VARCHAR(10)	 
   ,[transaction_date]	DATETIME	 NOT NULL
   ,[transaction_id]	VARCHAR(100) NOT NULL	
   ,[settlement_date]	DATETIME	 NOT NULL
   ,[brand]				VARCHAR(50)
   ,[brand_url]			VARCHAR(100)
   ,[game_code]			VARCHAR(10)
   ,[game_name]			VARCHAR(100)
   ,[game_type]			VARCHAR(50)
   ,[currency_code]		VARCHAR(10)  NOT NULL
   ,[bet_type]			VARCHAR(100) NOT NULL
   ,[bet_amount]		MONEY
   ,[debit_amount]		MONEY
   ,[credit_amount]		MONEY
   ,[net_amount]		MONEY
	CONSTRAINT [PK_rptDep_atTickets] PRIMARY KEY CLUSTERED 
	(
		[transaction_id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [rptDep_atTickets_table]
) ON [rptDep_atTickets_table]

CREATE NONCLUSTERED INDEX [CIX_date_bettype] ON [dbo].[rptDep_atTickets] 
(
	[settlement_date] ASC,
	[currency_code] ASC,
	[bet_type] ASC,
	[player_no] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [rptDep_atTickets_table]
GO