// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;
    using Session.Interfaces;
    using VendorAdapter.Interfaces;

    public class CustomerTurnoverDataAccess : BaseDataAccess, ICustomerTurnoverDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public CustomerTurnoverDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
            : base(appSetting, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public ReportResultModel<CustTurnover> GetDataList(CustomerTurnoverMetaModel metaModel, string languageCode)
        {
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
                        var currencyid = reader.ToInt("currencyid");
                        result.Add(new CustTurnover
                        {
                            Index = reader.ToInt("RowNo"),
                            ReportBy = ConvertHelper.ToDateTime(reader.ToString("ReportBy"), metaModel.ReportBy == ReportByType.Monthly),
                            UserName = reader.ToString("username"),
                            Currency = GetCurrencyName(currencyid),
                            SportId = sportId,
                            SportType = _cacheRefAdapter.GetSportName(sportId, languageCode),
                            TotalTransactions = reader.ToInt("BetCount"),
                            Bettype = reader.ToString("Bettype"),
                            Turnover = turnover,
                            TotalTurnover = turnover,
                            CustomerWinLoss = reader.ToDecimal("Winlost"),
                            EffectiveBettingValue = reader.ToDecimal("BetEffective"),
                            CustomerComm = reader.ToDecimal("PlayerComm"),
                        });
                    }

                    CustTurnover totalData = null;
                    int totalRows = 0;
                    if (reader.NextResult() && reader.Read())
                    {
                        totalData = ReadTotalData(reader);
                        totalRows = reader.ToInt("TotalRows");
                    }

                    return new ReportResultModel<CustTurnover>
                    {
                        DataList = result,
                        TotalData = totalData ?? new CustTurnover(),
                        TotalRows = totalRows
                    };
                }
            }
        }

        private static CustTurnover ReadTotalData(IDataReader reader)
        {
            return new CustTurnover
            {
                TotalTransactions = reader.ToInt("SumBetCount"),
                Turnover = reader.ToDecimal("SumTurnover"),
                TotalTurnover = reader.ToDecimal("SumTurnover"),
                CustomerWinLoss = reader.ToDecimal("SumWinlost"),
                CustomerComm = reader.ToDecimal("SumComm"),
                EffectiveBettingValue = reader.ToDecimal("SumBetEffective")
            };
        }

        private void AddParameters(CustomerTurnoverMetaModel metaModel, string languageCode, IObjectDb db)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("odds", DbType.Decimal, metaModel.Odds);
            db.AddInParameter("reportby", DbType.Int32, metaModel.ReportBy);
            db.AddInParameter("username", DbType.String, string.IsNullOrWhiteSpace(metaModel.Username) ? null : metaModel.Username);
            db.AddInParameter("movebetsday", DbType.DateTime, balanceUp.MoveBetsDay);
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