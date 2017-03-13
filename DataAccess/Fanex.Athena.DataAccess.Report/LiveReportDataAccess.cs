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

    public class LiveReportDataAccess : BaseDataAccess, ILiveReportDataAccess
    {
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
                            items.Add(new LiveReport
                            {
                                ReportType = 1,
                                ProductName = reader.ToString("ProductName"),
                                ProductCode = reader.ToString("ProductCode"),
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
                using (var db = new ObjectDb(UspName))
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
                            items.Add(new LiveReport
                            {
                                ReportType = 2,
                                ProductName = reader.ToString("SportType"),
                                ProductCode = reader.ToString("SportType"),
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
                using (var db = new ObjectDb(UspName))
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
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
                    db.AddInParameter("sportid", DbType.Int32, metaModel.SportId);
                    db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);
                    foreach (var product in metaModel.ProductOptions.Where(p => p.Status))
                    {
                        db.AddInParameter("show" + product.Code, DbType.Boolean, true);
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

        private static IList<LiveReport> ReadDetailByCurrency(IDataReader reader, string currency)
        {
            var list = new List<LiveReport>();
            while (reader.Read())
            {
                var productName = reader.ToString("Currency");
                list.Add(new LiveReport
                {
                    ReportType = 0,
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
                dataList.Add(new LiveReport
                {
                    ReportType = 3,
                    ProductName = reader.ToString("bettype"),
                    ProductCode = reader.ToString("bettype"),
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
                foreach (var product in products)
                {
                    if (!IsIgnoredProduct(product, result))
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
            }

            return result;
        }

        private static bool IsIgnoredProduct(Product product, IList<LiveReportByBetType> resultByBettype) =>
            product.ProductCode == Product.MGCLC || product.ProductCode == Product.MGCRNG ||
            product.ProductCode == Product.BA ||
            resultByBettype.Any(p => p.ProductName == product.ProductName);
    }
}