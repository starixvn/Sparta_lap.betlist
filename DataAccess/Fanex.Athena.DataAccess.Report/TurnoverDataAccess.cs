// -----------------------------------------------------------------------
// <copyright file="TurnoverDataAccess.cs" company="">
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

    public class TurnoverDataAccess : BaseDataAccess, ITurnoverDataAccess
    {
        public IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel)
        {
            IList<TurnoverDTO> result = new List<TurnoverDTO>();
            using (var db = this.GetObjectDb("Lrf_Rpt_TurnOver"))
            {
                foreach (var item in metaModel.ProductOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("username", DbType.String, metaModel.Username);
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

                using (var reader = db.ExecuteReader())
                {
                    var index = 1;
                    while (reader.Read())
                    {
                        result.Add(new TurnoverDTO
                        {
                            Index = index++,
                            Date = reader.ToDateTime("winlostdate").ToString("MM/dd/yyyy"),
                            OpeningBalance = reader.ToDecimal("OpeningBalance" + metaModel.BaseCurrency),
                            Deposit = reader.ToDecimal("Deposit" + metaModel.BaseCurrency),
                            Withdrawal = reader.ToDecimal("Withdrawal" + metaModel.BaseCurrency),
                            AffiliateTransfer = reader.ToDecimal("AffiliateTransfer" + metaModel.BaseCurrency),
                            Bonus = reader.ToDecimal("Bonus" + metaModel.BaseCurrency),
                            Adjustment = reader.ToDecimal("Adjustment" + metaModel.BaseCurrency),
                            Turnover = reader.ToDecimal("Turnover" + metaModel.BaseCurrency),
                            PendingFunds = reader.ToDecimal("PendingFund" + metaModel.BaseCurrency),
                            WinLoss = reader.ToDecimal("WinLoss" + metaModel.BaseCurrency),
                            ClosingBalance = reader.ToDecimal("ClosingBalance" + metaModel.BaseCurrency),
                            Transfer3rd = reader.ToDecimal("Transfer3rd" + metaModel.BaseCurrency),
                            DrawGame = reader.ToDecimal("DrawGame" + metaModel.BaseCurrency),
                            TurnoverBA = reader.ToDecimal("TurnoverBA" + metaModel.BaseCurrency),
                            CommBA = reader.ToDecimal("CommBA" + metaModel.BaseCurrency),
                            AmountDueToBA = reader.ToDecimal("AmountDueToBA" + metaModel.BaseCurrency),
                            NetTurnover = reader.ToDecimal("Turnover" + metaModel.BaseCurrency) - reader.ToDecimal("DrawGame" + metaModel.BaseCurrency),
                            PlayerComm = reader.ToDecimal("PlayerComm" + metaModel.BaseCurrency),
                        });
                    }
                }
            }

            return result;
        }
    }
}