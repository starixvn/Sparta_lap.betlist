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
        public IList<LiveReport> GetLiveReportSummary(LiveReportMetalModel metaModel, DateTime moveBetsDate)
        {
            const string UspName = "Lrf_Rpt_LiveReportSummary";
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

        public void GetLiveReportSummaryDetail(
            LiveReportMetalModel metaModel,
            DateTime moveBetsDate,
            string productCode,
            IList<Product> products,
            out IList<LiveReport> resultBySport,
            out IList<LiveReport> resultByByCurrency,
            out IList<LiveReportByBetType> resultByBettype)
        {
            var currency = metaModel.BaseCurrency;
            var taskDetail = Task<Tuple<IList<LiveReport>, IList<LiveReport>>>.Factory.StartNew(() =>
            {
                return GetSummaryDetail(metaModel, moveBetsDate, productCode, currency);
            });

            var taskForResultBySport = Task<IList<LiveReport>>.Factory.StartNew(() =>
            {
                return GetBySport(metaModel, moveBetsDate, productCode, currency);
            });

            // Task detail (by currency + by bettype)
            var detailResult = taskDetail.Result;
            resultByByCurrency = detailResult.Item1.Count > 0 ? detailResult.Item1 : null;
            if (detailResult.Item2.Count > 0)
            {
                resultByBettype = new List<LiveReportByBetType>();
                foreach (var product in products)
                {
                    if (IsIgnoredProduct(product, resultByBettype))
                    {
                        continue;
                    }

                    var temp = detailResult.Item2.Where(i => product.ValidateBettype(Convert.ToInt32(i.ProductCode))).ToList();
                    if (temp.Count > 0)
                    {
                        resultByBettype.Add(new LiveReportByBetType
                        {
                            ProductName = product.ProductName,
                            LiveReports = temp
                        });
                    }
                }
            }
            else
            {
                resultByBettype = null;
            }

            // Task by sport
            resultBySport = taskForResultBySport.Result;
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

        private static IList<LiveReport> GetBySport(LiveReportMetalModel metaModel, DateTime moveBetsDate, string productCode, string currency)
        {
            const string UspName = "Lrf_Rpt_LiveReportBySport";
            try
            {
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

        private static Tuple<IList<LiveReport>, IList<LiveReport>> GetSummaryDetail(
            LiveReportMetalModel metaModel,
            DateTime moveBetsDate,
            string productCode,
            string currency)
        {
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
                        var dataByCurrency = ReadDetailByCurrency(reader, currency);

                        var dataByBettype = reader.NextResult() ? ReadDetailByBettype(reader, currency) : new List<LiveReport>();

                        return new Tuple<IList<LiveReport>, IList<LiveReport>>(dataByCurrency, dataByBettype);
                    }
                }
            }
            catch (ObjectDbException ex)
            {
                Logging.LogService.Error(ex.Message);
                throw new DataAccessException(ex.Message, UspName, new { Model = metaModel, MoveBetDate = moveBetsDate, ProductCode = productCode });
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

        private static IList<LiveReport> ReadDetailByBettype(IDataReader reader, string currency)
        {
            var list = new List<LiveReport>();
            while (reader.Read())
            {
                list.Add(new LiveReport
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

            return list;
        }

        private static bool IsIgnoredProduct(Product product, IList<LiveReportByBetType> resultByBettype) =>
            product.ProductCode == Product.MGCLC || product.ProductCode == Product.MGCRNG ||
            product.ProductCode == Product.BA ||
            resultByBettype.Any(p => p.ProductName == product.ProductName);
    }
}