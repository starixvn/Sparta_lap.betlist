// -----------------------------------------------------------------------
// <copyright file="CustomerTurnoverDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
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
    using VendorAdapter.Interfaces;

    public class CustomerTurnoverDataAccess : BaseDataAccess, ICustomerTurnoverDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public CustomerTurnoverDataAccess(
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public async Task<ReportResultModel<CustTurnover>> GetDataList(CustomerTurnoverMetaModel metaModel, IDictionary<int, string> currencyList, string languageCode)
        {
            using (var db = this.GetObjectDb("Lrf_Rpt_CustomerTurnOver"))
            {
                AddParameters(metaModel, languageCode, db);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
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
                            Currency = currencyList?.GetValueOrDefault(currencyid, currencyid.ToString()),
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
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("fromdate", metaModel.FromDate);
            db.SetParameterValue("todate", metaModel.ToDate);
            db.SetParameterValue("odds", metaModel.Odds);
            db.SetParameterValue("reportby", metaModel.ReportBy);
            db.SetParameterValue("username", string.IsNullOrWhiteSpace(metaModel.Username) ? null : metaModel.Username);
            db.SetParameterValue("movebetsday", balanceUp.MoveBetsDay);
            db.SetParameterValue("Langkey", languageCode);
            db.SetParameterValue("PageSize", metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.PageIndex);
            db.SetParameterValue("currencyid", metaModel.CurrencyId);
            db.SetParameterValue("IsExportExcel", metaModel.IsExportExcel);
            db.SetParameterValue("showwin", metaModel.ShowWin);
            db.SetParameterValue("showloss", metaModel.ShowLoss);
            db.SetParameterValue("showdraw", metaModel.ShowDraw);
            db.SetParameterValue("Product", metaModel.ProductParameters);
            db.SetParameterValue("Bettype", metaModel.BettypeParameters);
        }
    }
}