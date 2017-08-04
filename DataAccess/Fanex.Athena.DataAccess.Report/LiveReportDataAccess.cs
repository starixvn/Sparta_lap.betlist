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
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.DataDomain;
    using Model.Core.Helper;

    public class LiveReportDataAccess : BaseDataAccess, ILiveReportDataAccess
    {
        private static readonly IEnumerable<string> IgnoredProductInBettypeSection = new string[]
        {
            Product.MGCLC, Product.MGCRNG, Product.BA
        };

        public IList<LiveReport> GetLiveReportSummary(LiveReportMetaModel metaModel, DateTime moveBetsDate)
        {
            const string UspName = "Lrf_Rpt_LiveReportSummary";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
                    db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);
                    using (var reader = db.ExecuteReader())
                    {
                        var items = new List<LiveReport>();
                        var currency = metaModel.BaseCurrency;
                        while (reader.Read())
                        {
                            var productName = reader.ToString("ProductName");
                            var productCode = reader.ToString("ProductCode");

                            items.Add(new LiveReport
                            {
                                ProductName = ProductHelper.GetProductName(productCode, productName),
                                ProductCode = productCode,
                                TurnOver = reader.ToDecimal("Turnover" + currency),
                                CustomerWinLoss = reader.ToDecimal("Winlost" + currency),
                                CustomerComm = reader.ToDecimal("PlayerComm" + currency),
                                LicenseeComm = reader.ToDecimal("Comm" + currency),
                                LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + currency),
                                AmountDue = reader.ToDecimal("AmtDueto3rd" + currency),
                                BetCount = reader.ToInt("Betcount")
                            });
                        }

                        return items;
                    }
                }
            }
            catch (ObjectDbException ex)
            {
                Logging.LogService.Error(ex.Message);
                throw new DataAccessException(ex.Message, UspName, new { Model = metaModel, MoveBetDate = moveBetsDate });
            }
        }

        public IList<LiveReport> GetDetailBySport(LiveReportMetaModel metaModel, DateTime moveBetsDate, string productCode)
        {
            const string UspName = "Lrf_Rpt_LiveReportBySport";
            try
            {
                var currency = metaModel.BaseCurrency;
                var items = new List<LiveReport>();
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
                    db.AddInParameter("productcode", DbType.String, productCode.Equals("ALL") ? string.Empty : productCode);
                    db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sportType = reader.ToString("SportType");
                            items.Add(new LiveReport
                            {
                                ProductName = GetProductNameBySport(sportType),
                                ProductCode = sportType,
                                TurnOver = reader.ToDecimal("Turnover" + currency),
                                CustomerWinLoss = reader.ToDecimal("Winlost" + currency),
                                CustomerComm = reader.ToDecimal("PlayerComm" + currency),
                                LicenseeComm = reader.ToDecimal("Comm" + currency),
                                LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + currency),
                                AmountDue = reader.ToDecimal("AmtDueto3rd" + currency),
                                BetCount = reader.ToInt("Betcount")
                            });
                        }

                        return items;
                    }
                }
            }
            catch (ObjectDbException ex)
            {
                Logging.LogService.Error(ex.Message);
                throw new DataAccessException(ex.Message, UspName, new { Model = metaModel, MoveBetDate = moveBetsDate, ProductCode = productCode });
            }
        }

        public Tuple<IList<LiveReport>, IList<LiveReportByBetType>> GetDetailByCurrencyAndBettype(
            LiveReportMetaModel metaModel,
            DateTime moveBetsDate,
            string productCode,
            IList<Product> products)
        {
            var currency = metaModel.BaseCurrency;
            const string UspName = "Lrf_Rpt_LiveReportSummaryDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
                    db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);
                    db.AddInParameter("productcode", DbType.String, productCode.Equals("ALL") ? string.Empty : productCode);
                    using (var reader = db.ExecuteReader())
                    {
                        var resultByCurrency = ReadDetailByCurrency(reader, currency);

                        var resultByBettype = reader.NextResult() ? ReadDetailByBettype(reader, products, currency) : new List<LiveReportByBetType>();

                        return new Tuple<IList<LiveReport>, IList<LiveReportByBetType>>(resultByCurrency, resultByBettype);
                    }
                }
            }
            catch (ObjectDbException ex)
            {
                Logging.LogService.Error(ex.Message);
                throw new DataAccessException(ex.Message, UspName, new { Model = metaModel, MoveBetDate = moveBetsDate, ProductCode = productCode });
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
                    Justification = "Reviewed. Suppression is OK here.")]
        public IList<LiveReportWinLossByMatch> GetLiveReportWinlossByMatch(LiveReportMatchListMetalModel metaModel, DateTime moveBetsDate)
        {
            const string UspName = "Lrf_Rpt_LiveReportWinlossByMatch";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
                    db.AddInParameter("sportid", DbType.Int32, metaModel.SportId);
                    db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);
                    var productList = metaModel.ProductCodeList.Split(',');
                    foreach (var product in productList)
                    {
                        db.AddInParameter("show" + product, DbType.Boolean, true);
                    }

                    using (var reader = db.ExecuteReader())
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
            catch (ObjectDbException ex)
            {
                Logging.LogService.Error(ex.Message);
                throw new DataAccessException(ex.Message, UspName, new { Model = metaModel, MoveBetDate = moveBetsDate });
            }
        }

        private static string GetProductNameBySport(string sportType)
        {
            var sportId = string.IsNullOrEmpty(sportType) ? 0 : Convert.ToInt32(sportType);

            // 101 -> 104: const from SP to separate names for special bettypes
            switch (sportId)
            {
                case 101:
                    return CacheRef.GetBettype((int)BetType.MixParlay);

                case 102:
                    return CacheRef.GetBettype((int)BetType.CombinationMixParlay);

                case 103:
                    return CacheRef.GetBettype((int)BetType.Outright);

                case 104:
                    return CacheRef.GetBettypeWithProductName((int)BetType.BetradarVsMixParlay, false);

                default:
                    return CacheRef.GetSportName(sportType);
            }
        }

        private static IList<LiveReport> ReadDetailByCurrency(IDataReader reader, string currency)
        {
            var list = new List<LiveReport>();
            while (reader.Read())
            {
                var productName = reader.ToString("Currency");
                list.Add(new LiveReport
                {
                    ProductName = productName,
                    ProductCode = productName,
                    TurnOver = reader.ToDecimal("Turnover" + currency),
                    CustomerWinLoss = reader.ToDecimal("Winlost" + currency),
                    CustomerComm = reader.ToDecimal("PlayerComm" + currency),
                    LicenseeComm = reader.ToDecimal("Comm" + currency),
                    LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + currency),
                    AmountDue = reader.ToDecimal("AmtDueto3rd" + currency),
                    BetCount = reader.ToInt("Betcount")
                });
            }

            return list;
        }

        private static IList<LiveReportByBetType> ReadDetailByBettype(IDataReader reader, IList<Product> products, string currency)
        {
            var dataList = new List<LiveReport>();
            while (reader.Read())
            {
                var bettype = reader.ToString("bettype");
                dataList.Add(new LiveReport
                {
                    ProductName = CacheRef.GetBettype(Convert.ToInt32(bettype), false),
                    ProductCode = bettype,
                    TurnOver = reader.ToDecimal("Turnover" + currency),
                    CustomerWinLoss = reader.ToDecimal("Winlost" + currency),
                    CustomerComm = reader.ToDecimal("PlayerComm" + currency),
                    LicenseeComm = reader.ToDecimal("Comm" + currency),
                    LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + currency),
                    AmountDue = reader.ToDecimal("AmtDueto3rd" + currency),
                    BetCount = reader.ToInt("Betcount")
                });
            }

            var result = new List<LiveReportByBetType>();
            if (dataList.Any())
            {
                foreach (var product in products.Where(p => !IgnoredProductInBettypeSection.Contains(p.ProductCode)))
                {
                    var bettypeList = dataList.Where(i => product.ValidateBettype(Convert.ToInt32(i.ProductCode))).ToList();
                    if (bettypeList.Any())
                    {
                        result.Add(new LiveReportByBetType
                        {
                            ProductName = product.ProductName,
                            LiveReports = bettypeList
                        });
                    }
                }
            }

            return result;
        }
    }
}