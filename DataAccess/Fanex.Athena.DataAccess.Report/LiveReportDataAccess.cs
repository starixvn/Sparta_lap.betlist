// -----------------------------------------------------------------------
// <copyright file="LiveReportDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Constant.DataDomain;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.Helper;
    using VendorAdapter.Interfaces;

    public class LiveReportDataAccess : BaseDataAccess, ILiveReportDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public LiveReportDataAccess(
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public async Task<IList<LiveReport>> GetLiveReportSummary(LiveReportMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_LiveReportSummary";
            var moveBetsDate = (await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false)).MoveBetsDay;

            using (var db = this.GetObjectDb(UspName))
            {
                AddParameters(db, metaModel, moveBetsDate);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var list = new List<LiveReport>();
                    var currency = metaModel.BaseCurrency;
                    while (reader.Read())
                    {
                        var productName = reader.ToString("ProductName");
                        var productCode = reader.ToString("ProductCode");

                        var item = new LiveReport
                        {
                            ProductName = ProductHelper.GetProductName(productCode, productName),
                            ProductCode = productCode
                        };
                        ReadCommonColumns(reader, currency, item);
                        list.Add(item);
                    }

                    return list;
                }
            }
        }

        public async Task<IList<LiveReport>> GetDetailBySport(LiveReportMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_LiveReportBySport";
            var moveBetsDate = (await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false)).MoveBetsDay;

            var currency = metaModel.BaseCurrency;
            var list = new List<LiveReport>();
            using (var db = this.GetObjectDb(UspName))
            {
                AddParameters(db, metaModel, moveBetsDate);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        var sportType = reader.ToString("SportType");
                        var item = new LiveReport
                        {
                            ProductName = GetProductNameBySport(sportType),
                            ProductCode = sportType
                        };
                        ReadCommonColumns(reader, currency, item);
                        list.Add(item);
                    }

                    return list;
                }
            }
        }

        public async Task<Tuple<IList<LiveReport>, IList<LiveReportByBetType>>> GetDetailByCurrencyAndBettype(
            LiveReportMetaModel metaModel,
            IDictionary<int, string> currencyList,
             IEnumerable<Product> products)
        {
            var currency = metaModel.BaseCurrency;
            const string UspName = "Lrf_Rpt_LiveReportSummaryDetail";
            var moveBetsDate = (await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false)).MoveBetsDay;

            using (var db = this.GetObjectDb(UspName))
            {
                AddParameters(db, metaModel, moveBetsDate);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var resultByCurrency = ReadDetailByCurrency(reader, currencyList, currency);

                    var resultByBettype = reader.NextResult() ? ReadDetailByBettype(reader, products, currency) : new List<LiveReportByBetType>();

                    return new Tuple<IList<LiveReport>, IList<LiveReportByBetType>>(resultByCurrency, resultByBettype);
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
                    Justification = "Reviewed. Suppression is OK here.")]
        public async Task<IList<LiveReportWinLossByMatch>> GetLiveReportWinlossByMatch(LiveReportMatchListMetalModel metaModel)
        {
            const string UspName = "Lrf_Rpt_LiveReportWinlossByMatch";
            var moveBetsDate = (await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false)).MoveBetsDay;

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("fromdate", metaModel.FromDate);
                db.SetParameterValue("todate", metaModel.ToDate);
                db.SetParameterValue("movebetsday", moveBetsDate);
                db.SetParameterValue("sportid", metaModel.SportId);
                db.SetParameterValue("liveindicator", metaModel.LiveIndicator);
                var productList = metaModel.ProductCodeList.Split(',');
                foreach (var product in productList)
                {
                    db.SetParameterValue("show" + product, true);
                }

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var currency = metaModel.BaseCurrency;
                    var data = new List<LiveReportWinLossByMatch>();

                    while (reader.Read())
                    {
                        data.Add(new LiveReportWinLossByMatch
                        {
                            MatchId = reader.ToLong("matchid"),
                            HomeId = reader.ToInt("homeid"),
                            AwayId = reader.ToInt("awayid"),
                            BetType = reader.ToInt("bettype"),
                            ShowTime = reader.ToString("showtime"),
                            BetCount = reader.ToInt("Betcount"),
                            TurnOver = reader.ToDecimal("Turnover" + currency),
                            CustomerWinLoss = reader.ToDecimal("Winlost" + currency),
                            CustomerComm = reader.ToDecimal("PlayerComm" + currency),
                            LicenseeComm = reader.ToDecimal("Comm" + currency),
                            LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + currency),
                            AmountDue = reader.ToDecimal("AmtDueto3rd" + currency),
                            SportId = metaModel.SportId
                        });
                    }

                    return data;
                }
            }
        }

        private static void AddParameters(IObjectDb db, LiveReportMetaModel metaModel, DateTime moveBetsDate)
        {
            db.SetParameterValue("ProductCodes", metaModel.ProductCodeList);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("fromdate", metaModel.FromDate);
            db.SetParameterValue("todate", metaModel.ToDate);
            db.SetParameterValue("movebetsday", moveBetsDate);
            db.SetParameterValue("liveindicator", metaModel.LiveIndicator);
        }

        private static void ReadCommonColumns(IDataReader reader, string currency, LiveReport item)
        {
            item.TurnOver = reader.ToDecimal("Turnover" + currency);
            item.CustomerWinLoss = reader.ToDecimal("Winlost" + currency);
            item.CustomerComm = reader.ToDecimal("PlayerComm" + currency);
            item.LicenseeComm = reader.ToDecimal("Comm" + currency);
            item.LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + currency);
            item.AmountDue = reader.ToDecimal("AmtDueto3rd" + currency);
            item.BetCount = reader.ToInt("Betcount");
        }

        protected internal virtual string GetProductNameBySport(string sportType)
        {
            var sportId = string.IsNullOrEmpty(sportType) ? 0 : Convert.ToInt32(sportType);

            // 101 -> 104: const from SP to separate names for special bettypes
            switch (sportId)
            {
                case 101:
                    return _cacheRefAdapter.GetBettype((int)BetType.MixParlay);

                case 102:
                    return _cacheRefAdapter.GetBettype((int)BetType.CombinationMixParlay);

                case 103:
                    return _cacheRefAdapter.GetBettype((int)BetType.Outright);

                case 104:
                    return _cacheRefAdapter.GetBettypeWithProductName((int)BetType.VirtualSports2MixParlay);

                default:
                    return _cacheRefAdapter.GetSportName(sportType);
            }
        }

        private IList<LiveReport> ReadDetailByCurrency(IDataReader reader, IDictionary<int, string> currencyList, string currency)
        {
            var list = new List<LiveReport>();
            while (reader.Read())
            {
                var currencyId = reader.ToInt("Currency");
                var currencyName = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString());
                var item = new LiveReport
                {
                    ProductName = currencyName,
                    ProductCode = currencyName
                };

                ReadCommonColumns(reader, currency, item);
                list.Add(item);
            }

            return list.OrderBy(x => x.ProductName).ToList();
        }

        private IList<LiveReportByBetType> ReadDetailByBettype(IDataReader reader, IEnumerable<Product> products, string currency)
        {
            var dataList = new List<LiveReport>();
            while (reader.Read())
            {
                var bettype = reader.ToString("bettype");
                var item = new LiveReport
                {
                    ProductName = GetBetTypeName(Convert.ToInt32(bettype)),
                    ProductCode = reader.ToString("ProductCode")
                };
                ReadCommonColumns(reader, currency, item);

                dataList.Add(item);
            }

            var result = new List<LiveReportByBetType>();
            if (dataList.Any())
            {
                result = products.Join(
                    dataList,
                    p => p.ProductCode,
                    d => d.ProductCode,
                    (p, d) => new { p.ProductName, LiveReports = d })
                    .GroupBy(
                    x => x.ProductName,
                    (key, g) => new LiveReportByBetType
                    {
                        ProductName = key,
                        LiveReports = g.Select(x => x.LiveReports).ToList()
                    }).ToList();
            }

            return result;
        }

        private string GetBetTypeName(int betType)
        {
            string suffix = GetBettypeNameSuffix(betType);
            return _cacheRefAdapter.GetBettype(Convert.ToInt32(betType), false) + suffix;
        }

        private string GetBettypeNameSuffix(int betType)
        {
            if (betType == (int)BetType.NumberGameHappy5OddEven)
            {
                return " (" + _cacheRefAdapter.GetSportName((int)SportType.NumberGameHappy5) + ")";
            }

            if (betType >= (int)BetType.VirtualBasketballOverUnder && betType <= (int)BetType.VirtualBasketballFirstHalfHandicap)
            {
                return " (" + _cacheRefAdapter.GetSportName((int)SportType.VirtualBasketball) + ")";
            }

            return string.Empty;
        }
    }
}