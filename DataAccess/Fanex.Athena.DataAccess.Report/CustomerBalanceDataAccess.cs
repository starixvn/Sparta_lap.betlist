// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;

    public class CustomerBalanceDataAccess : BaseDataAccess, ICustBalanceDataAccess
    {
        public IList<CustBalance> GetDataList(CustomerBalanceMetaModel metaModel, ref CustBalance totalData, ref int totalRow)
        {
            IList<CustBalance> result = new List<CustBalance>();
            using (var db = this.GetObjectDb("Lrf_Rpt_CustomerBalance"))
            {
                db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("topcount", DbType.Int32, metaModel.TopCount);
                db.AddInParameter("orderitem", DbType.Byte, metaModel.OrderItem);
                db.AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
                db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

                foreach (var item in metaModel.ProductOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(ReadDataItem(reader));
                    }

                    if (result.Count > 0 && reader.NextResult() && reader.Read())
                    {
                        totalData = ReadTotalData(reader);
                        totalRow = reader.ToInt("TotalRows");
                    }
                }
            }

            return result;
        }

        private static CustBalance ReadDataItem(IDataReader reader)
        {
            return new CustBalance
            {
                ExternalId = reader.ToString("nickname"),
                SystemId = reader.ToString("username"),
                OpeningBalance = reader.ToDecimal("openingbalance"),
                Deposit = reader.ToDecimal("deposit"),
                Withdrawal = reader.ToDecimal("withdrawal"),
                Bonus = reader.ToDecimal("bonus"),
                Adjustment = reader.ToDecimal("adjustment"),
                Turnover = reader.ToDecimal("turnover"),
                DrawGame = reader.ToDecimal("drawgame"),
                NetTurnover = reader.ToDecimal("turnover") - reader.ToDecimal("drawgame"),
                PendingFunds = reader.ToDecimal("outstanding"),
                WinLoss = reader.ToDecimal("winloss"),
                PlayerComm = reader.ToDecimal("playercomm"),
                TotalWL = reader.ToDecimal("winloss") + reader.ToDecimal("playercomm"),
                AffiliateTransfer = reader.ToDecimal("affiliatetransfer"),
                ClosingBalance = reader.ToDecimal("closingbalance"),
                TurnoverBA = reader.ToDecimal("TurnOverBA"),
                CommBA = reader.ToDecimal("CommBA"),
                AmountDueToBA = reader.ToDecimal("AmountDueToBA")
            };
        }

        private static CustBalance ReadTotalData(IDataReader reader)
        {
            return new CustBalance
            {
                OpeningBalance = reader.ToDecimal("sumopeningbalance"),
                Deposit = reader.ToDecimal("sumdeposit"),
                Withdrawal = reader.ToDecimal("sumwithdrawal"),
                Bonus = reader.ToDecimal("sumbonus"),
                Adjustment = reader.ToDecimal("sumadjustment"),
                Turnover = reader.ToDecimal("sumturnover"),
                DrawGame = reader.ToDecimal("sumdrawgame"),
                NetTurnover = reader.ToDecimal("sumturnover") - reader.ToDecimal("sumdrawgame"),
                PendingFunds = reader.ToDecimal("sumoutstanding"),
                WinLoss = reader.ToDecimal("sumwinloss"),
                AffiliateTransfer = reader.ToDecimal("sumaffiliatetransfer"),
                ClosingBalance = reader.ToDecimal("sumclosingbalance"),
                TurnoverBA = reader.ToDecimal("sumturnoverba"),
                CommBA = reader.ToDecimal("sumcommba"),
                AmountDueToBA = reader.ToDecimal("sumamountduetoba"),
                PlayerComm = reader.ToDecimal("sumplayercomm"),
                TotalWL = reader.ToDecimal("sumwinloss") + reader.ToDecimal("sumplayercomm")
            };
        }
    }
}