// -----------------------------------------------------------------------
// <copyright file="LiveReportDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
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

    /// <summary>
    /// Live report data access.
    /// </summary>
    public class LiveReportDataAccess : BaseDataAccess, ILiveReportDataAccess
    {
        /// <summary>
        /// Get live report summary.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="moveBetsDate">Move bets date value.</param>
        /// <returns>The result value.</returns>
        public IList<LiveReport> GetLiveReportSummary(LiveReportMetalModel metaModel, DateTime moveBetsDate)
        {
            var uspName = "Lrf_Rpt_LiveReportSummary";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
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
                throw new DataAccessException(ex.Message, uspName, new { Model = metaModel, MoveBetDate = moveBetsDate });
            }
        }

        /// <summary>
        /// Get live report summary detail.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="moveBetsDate">Move bet date value.</param>
        /// <param name="productCode">Product code value.</param>
        /// <param name="products">Products value.</param>
        /// <param name="resultBySport">Result by sport value.</param>
        /// <param name="resultByByCurrency">Result by currency value.</param>
        /// <param name="resultByBettype">Result by bet type value.</param>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void GetLiveReportSummaryDetail(
            LiveReportMetalModel metaModel,
            DateTime moveBetsDate,
            string productCode,
            IList<Product> products,
            out IList<LiveReport> resultBySport,
            out IList<LiveReport> resultByByCurrency,
            out IList<LiveReportByBetType> resultByBettype)
        {
            var uspName = "Lrf_Rpt_LiveReportSummaryDetail";
            var uspNameBySport = "Lrf_Rpt_LiveReportBySport";

            try
            {
                var currency = metaModel.BaseCurrency;
                var task1 = Task<IList<LiveReport>[]>.Factory.StartNew(() =>
                 {
                     using (var db = new ObjectDb(uspName))
                     {
                         db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                         db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                         db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
                         db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                         db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                         db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
                         db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);
                         db.AddInParameter("productcode", DbType.String, productCode.Equals("ALL") ? string.Empty : productCode);
                         var result = new IList<LiveReport>[] { new List<LiveReport>(), new List<LiveReport>() };
                         using (var reader = db.ExecuteReader())
                         {
                             while (reader.Read())
                             {
                                 var productName = reader.ToString("currency");
                                 result[0].Add(new LiveReport
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

                             if (reader.NextResult())
                             {
                                 while (reader.Read())
                                 {
                                     result[1].Add(new LiveReport
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
                             }

                             return result;
                         }
                     }
                 });

                var taskForResultBySport = Task<IList<LiveReport>>.Factory.StartNew(() =>
                    {
                        // Task2
                        var items = new List<LiveReport>();
                        using (var db = new ObjectDb(uspNameBySport))
                        {
                            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                            db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
                            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                            db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                            db.AddInParameter("movebetsday", DbType.DateTime, moveBetsDate);
                            db.AddInParameter("productcode", DbType.String, productCode.Equals("ALL") ? string.Empty : productCode);
                            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator);

                            using (var reader = db.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var item =
                                        new LiveReport
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
                                        };
                                    items.Add(item);
                                }

                                return items;
                            }
                        }
                    });

                // Task1 Result
                var temps = task1.Result;
                resultByByCurrency = temps[0].Count > 0 ? temps[0] : null;
                if (temps[1].Count > 0)
                {
                    resultByBettype = new List<LiveReportByBetType>();
                    foreach (var product in products)
                    {
                        if (IsIgnoredProduct(product, resultByBettype))
                        {
                            continue;
                        }

                        var temp = temps[1].Where(i => product.ValidateBettype(Convert.ToInt32(i.ProductCode))).ToList();
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

                // Task2 Result
                resultBySport = taskForResultBySport.Result;
            }
            catch (ObjectDbException ex)
            {
                Logging.LogService.Error(ex.Message);
                throw new DataAccessException(
                    ex.Message,
                    uspName + "," + uspNameBySport,
                    new
                    {
                        Model = metaModel,
                        MoveBetDate = moveBetsDate,
                        ProductCode = productCode
                    });
            }
        }

        private bool IsIgnoredProduct(Product product, IList<LiveReportByBetType> resultByBettype)
        {
            return product.ProductCode == Product.MGCLC || product.ProductCode == Product.MGCRNG ||
                    product.ProductCode == Product.BA ||
                    resultByBettype.Any(p => p.ProductName == product.ProductName);
        }

        /// <summary>
        /// Get live report win loss match.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="moveBetsDate">Move bet date value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
                    Justification = "Reviewed. Suppression is OK here.")]
        public IList<LiveReportWinLossByMatch> GetLiveReportWinlossByMatch(LiveReportMatchListMetalModel metaModel, DateTime moveBetsDate)
        {
            var uspName = "Lrf_Rpt_LiveReportWinlossByMatch";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
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
                throw new DataAccessException(ex.Message, uspName, new { Model = metaModel, MoveBetDate = moveBetsDate });
            }
        }
    }
}