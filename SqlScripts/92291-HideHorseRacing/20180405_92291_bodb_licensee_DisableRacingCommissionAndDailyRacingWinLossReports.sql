USE bodb_licensee
GO

/*
 Created: 20180405@Joseph
 Feature: #92291: Hide Horse Racing in the backend

 - Disable two reports: Racing Commission and Daily Racing Commission
*/


  update f set IsActive = 0
  FROM [bodb_licensee].[dbo].[Functions] f
  where FunctionId in (34,35)