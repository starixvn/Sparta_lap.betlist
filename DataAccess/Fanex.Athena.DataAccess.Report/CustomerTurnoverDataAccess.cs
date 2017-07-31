// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.Helper;

    public class CustomerTurnoverDataAccess : BaseDataAccess, ICustTurnoverDataAccess
    {
        private readonly BalanceUp _balanceUp;

        public CustomerTurnoverDataAccess(BalanceUp balanceUp)
        {
            _balanceUp = balanceUp;
        }

        public IList<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode, out CustTurnover total, out int totalRow)
        {
            total = new CustTurnover();
            totalRow = 0;

            using (var db = this.GetObjectDb("Lrf_Rpt_CustomerTurnOver"))
            {
                AddParameters(metaModel, languageCode, db);

                using (var reader = db.ExecuteReader())
                {
                    var result = new List<CustTurnover>();
                    while (reader.Read())
                    {
                        var turnover = reader.ToDecimal("Turnover");
                        var sportId = reader.ToInt("sporttype");
                        result.Add(new CustTurnover
                        {
                            ReportBy = ConvertHelper.ToDateTime(reader.ToString("ReportBy"), metaModel.ReportBy == ReportByType.Monthly),
                            UserName = reader.ToString("username"),
                            Currency = reader.ToString("currency"),
                            SportId = sportId,
                            SportType = CacheRef.GetSportName(sportId, languageCode),
                            TotalTransactions = reader.ToInt("BetCount"),
                            Bettype = reader.ToString("Bettype"),
                            Turnover = turnover,
                            TotalTurnover = turnover,
                            CustomerWinLoss = reader.ToDecimal("Winlost"),
                            EffectiveBettingValue = reader.ToDecimal("BetEffective"),
                            CustomerComm = reader.ToDecimal("PlayerComm"),
                        });
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        total.TotalTransactions = reader.ToInt("SumBetCount");
                        total.Turnover = reader.ToDecimal("SumTurnover");
                        total.TotalTurnover = reader.ToDecimal("SumTurnover");
                        total.CustomerWinLoss = reader.ToDecimal("SumWinlost");
                        total.CustomerComm = reader.ToDecimal("SumComm");
                        total.EffectiveBettingValue = reader.ToDecimal("SumBetEffective");
                        totalRow = reader.ToInt("TotalRows");
                    }

                    return result;
                }
            }
        }

        private void AddParameters(CustomerTurnoverMetaModel metaModel, string languageCode, IObjectDb db)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("odds", DbType.Decimal, metaModel.Odds);
            db.AddInParameter("reportby", DbType.Int32, metaModel.ReportBy);
            db.AddInParameter("username", DbType.String, string.IsNullOrWhiteSpace(metaModel.Username) ? null : metaModel.Username);
            db.AddInParameter("movebetsday", DbType.DateTime, _balanceUp.MoveBetsDay);
            db.AddInParameter("balanceupday", DbType.DateTime, _balanceUp.BalanceUpDay);
            db.AddInParameter("Langkey", DbType.String, languageCode);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("IsExportExcel", DbType.Boolean, metaModel.IsExportExcel);
            db.AddInParameter("showwin", DbType.Boolean, metaModel.ShowWin);
            db.AddInParameter("showloss", DbType.Boolean, metaModel.ShowLoss);
            db.AddInParameter("showdraw", DbType.Boolean, metaModel.ShowDraw);
            db.AddInParameter("Product", DbType.String, metaModel.ProductParameters);
            db.AddInParameter("Bettype", DbType.String, metaModel.BettypeParameters);
        }
    }
}