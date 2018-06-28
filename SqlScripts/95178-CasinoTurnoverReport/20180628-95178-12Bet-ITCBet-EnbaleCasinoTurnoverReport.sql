USE bodb_licensee
GO

/*
	 Creator: 20180628@Amanda
	 Task: 12Bet & ICTBet - Enable Betting ticket center[Redmine: #95178]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180628@Amanda : LRF - Enable Betting ticket center for 12Bet & ICTBet[Redmine: #95178]
*/

DECLARE @LicenseeID INT, @FunctionID INT

SELECT @FunctionID = FunctionId
FROM dbo.Functions
WHERE FunctionKey = 'CasinoTurnover80'

-- 12bet
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = '12Bet'

SELECT @FunctionID, @LicenseeID

INSERT INTO LicenseeFunctions (LicenseeId, FunctionId)
VALUES (@LicenseeID, @FunctionID)

-- ITCBet
SELECT @LicenseeID = LicenseeID
FROM dbo.Licensees WITH (NOLOCK)
WHERE LicenseeName = 'ITCBet'

SELECT @FunctionID, @LicenseeID

INSERT INTO LicenseeFunctions (LicenseeId, FunctionId)
VALUES (@LicenseeID, @FunctionID)