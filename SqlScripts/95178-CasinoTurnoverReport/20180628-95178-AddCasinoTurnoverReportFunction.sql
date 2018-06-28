USE bodb_licensee
GO

/*
	 Creator: 20180628@Amanda
	 Task: Add function Casino Turnover report - [Redmine: #95178]
	 Database: DBLIC.bodb_licensee

	 Revisions:
	 -20180628@Amanda : LRF - Add function Casino Turnover report [redmine:#95178]
*/

SET IDENTITY_INSERT Functions ON

INSERT INTO  Functions 
(
	FunctionId,
	Name,
	Description,
	ParentId,
	Url,
	IsMenu,
	IsActive,
	DisplayOrder,
	IsNew,
	FunctionKey,
	Environment,
	IsFunction
)
VALUES (80, 'Casino Turnover', '', 3, 'CasinoTurnover', 1, 1, 55, 1, 'CasinoTurnover80', 'LOCAL', 1)
  
SET IDENTITY_INSERT Functions OFF