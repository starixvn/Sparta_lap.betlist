// -----------------------------------------------------------------------
// <copyright file="TurnoverDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Session.Interfaces;

    public class TurnoverDataAccess : BaseDataAccess, ITurnoverDataAccess
    {
        public TurnoverDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel)
        {
            var result = new List<TurnoverDTO>();

            using (var db = this.GetObjectDb("Lrf_Rpt_TurnOver"))
            {
                AddParams(db, metaModel);

                using (var reader = db.ExecuteReader())
                {
                    var index = 1;
                    while (reader.Read())
                    {
                        var item = ReadDataItem(reader, metaModel.BaseCurrency, index++);
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private static TurnoverDTO ReadDataItem(IDataReader reader, string baseCurrency, int index)
        {
            return new TurnoverDTO
            {
                Index = index,
                Date = reader.ToDateTime("winlostdate").ToString("MM/dd/yyyy"),
                OpeningBalance = reader.ToDecimal("OpeningBalance" + baseCurrency),
                Deposit = reader.ToDecimal("Deposit" + baseCurrency),
                Withdrawal = reader.ToDecimal("Withdrawal" + baseCurrency),
                AffiliateTransfer = reader.ToDecimal("AffiliateTransfer" + baseCurrency),
                Bonus = reader.ToDecimal("Bonus" + baseCurrency),
                Adjustment = reader.ToDecimal("Adjustment" + baseCurrency),
                Turnover = reader.ToDecimal("Turnover" + baseCurrency),
                PendingFunds = reader.ToDecimal("PendingFund" + baseCurrency),
                WinLoss = reader.ToDecimal("WinLoss" + baseCurrency),
                ClosingBalance = reader.ToDecimal("ClosingBalance" + baseCurrency),
                Transfer3rd = reader.ToDecimal("Transfer3rd" + baseCurrency),
                DrawGame = reader.ToDecimal("DrawGame" + baseCurrency),
                TurnoverBA = reader.ToDecimal("TurnoverBA" + baseCurrency),
                CommBA = reader.ToDecimal("CommBA" + baseCurrency),
                AmountDueToBA = reader.ToDecimal("AmountDueToBA" + baseCurrency),
                NetTurnover = reader.ToDecimal("Turnover" + baseCurrency) - reader.ToDecimal("DrawGame" + baseCurrency),
                PlayerComm = reader.ToDecimal("PlayerComm" + baseCurrency),
            };
        }

        private static void AddParams(IObjectDb db, TurnoverMetaModel metaModel)
        {
            foreach (var item in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
            }

            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("username", DbType.String, metaModel.Username);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}