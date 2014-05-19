// -----------------------------------------------------------------------
// <copyright file="ReportController.MatchWinLoss.cs" company="Nexcel">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Fanex.Common;

    /// <summary>
    /// The match win-loss controller.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// The match win-loss report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult MatchWinLoss()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new MatchWinLossMetaModel
            {
                BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                LicenseeId = licenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, IgnoreCasinoAndBingoAndKeno);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.Sb
                                             }).ToList();
            metaModel.ProductOptions = productOption;

            var model = new MatchWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Report/MatchWinLoss/Main.cshtml", model);
        }

        /// <summary>
        /// The match win-loss.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [SuppressMessage(
          "StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult MatchWinLoss(MatchWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId; 
            if (metaModel.IsExportExcel)
            {
                var currencyList = userSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return MatchWinLossExcel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            var reportData = _matchWinLossService.GetDataList(metaModel);
            var reportDataOutrightParlay = new List<MatchWinLoss>();

            if (metaModel.ProductOptions.Count(i => (i.Code == Product.Ba || i.Code == Product.Sb) && i.Status) > 0)
            {
                reportDataOutrightParlay = _matchWinLossService.GetOutrightParlayList(metaModel).ToList();
            }

            // Calculate the league list
            var leagueList = new Dictionary<int, string>();
            for (int i = 0, count = reportData.Count; i < count; i++)
            {
                if (!leagueList.ContainsKey(reportData[i].LeagueId))
                {
                    leagueList.Add(reportData[i].LeagueId, reportData[i].LeagueName);
                }
            }

            var total = GetMatchWinLossTotal(reportData);
            var grandTotal = GetMatchWinLossGrandTotal(reportDataOutrightParlay, total);

            var model = new MatchWinLossViewModel
            {
                Param = metaModel,
                MainResult = reportData,
                LeagueList = leagueList,
                SpecialBetTypeResult = reportDataOutrightParlay,

                CurrencyList = userSession.CurrencyList,
                LicenseeTypeList = licenseeSession.LicenseeType,

                ProductAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, IgnoreCasinoAndBingoAndKeno),
                ColumnCount = GetMatchWinlossColumnCount(licenseeSession),
                Total = total,
                GrandTotal = grandTotal,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };
            return View("~/Views/Report/MatchWinLoss/Main.cshtml", model);
        }

        /// <summary>
        /// The match win-loss detail.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult MatchWinLossDetail(MatchWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            var reportData = _matchWinLossService.GetDetailDataList(metaModel);

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, IgnoreCasinoAndBingoAndKeno);

            var model = new MatchWinLossViewModel
            {
                MainResult = reportData,
                ProductAccess = productAccess,
                LicenseeTypeList = licenseeSession.LicenseeType,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Report/MatchWinLoss/Content.data.Detail.cshtml", model);
        }

        /// <summary>
        /// The match win-loss excel report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public ActionResult MatchWinLossExcel(MatchWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "MatchWinLoss";
            var templatePath = Server.MapPath(@"~/App_Data/Template/Reports/MatchWinLoss.xls");
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, IgnoreCasinoAndBingoAndKeno);
            var showingOption = new ShowingOption
            {
                BaVisible = productAccess.IsAllowBa,
                RbVisible = productAccess.IsAllowRb,
                NgVisible = productAccess.IsAllowNg,
                LcVisible = productAccess.IsAllowLc,
                SbVisible = productAccess.IsAllowSB,
                VsVisible = productAccess.IsAllowVs,
                KnVisible = productAccess.IsAllowKN,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            metaModel.PageSize = 63000;
            var reportData = _matchWinLossService.GetDetailDataSet(metaModel);
            var mainTable = reportData.Tables[0];
            mainTable = SortMatchWinlossTable(mainTable);
            mainTable.DefaultView.Sort = "newproductid asc, leaguename asc, matchname ASC";
            mainTable = mainTable.DefaultView.ToTable();
            var matchWinLossExcel = new MatchWinLossExcel(
                templatePath,
                FileName,
                convertingOption,
                showingOption,
                mainTable,
                reportData.Tables[1],
                metaModel);
            matchWinLossExcel.ProductOptions = metaModel.ProductOptions;

            matchWinLossExcel.Export();

            return new ExcelResult(matchWinLossExcel.Workbook, FileName);
        }

        #region Utility

        /// <summary>
        /// Get total data.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The total data.</returns>
        private static MatchWinLoss GetMatchWinLossTotal(IList<MatchWinLoss> dataList)
        {
            var totalMatchWinLoss = new MatchWinLoss
            {
                Turnover = dataList.Sum(item => item.Turnover),
                CustomerWl = dataList.Sum(item => item.CustomerWl),
                CustomerComm = dataList.Sum(item => item.CustomerComm),
                LicenseeWl = dataList.Sum(item => item.LicenseeWl),
                BaCommission = dataList.Sum(item => item.BaCommission),
                AmtDueToBa = dataList.Sum(item => item.AmtDueToBa),
                RacingRevenue = dataList.Sum(item => item.RacingRevenue),
                AmtDueToRacing = dataList.Sum(item => item.AmtDueToRacing)
            };

            return totalMatchWinLoss;
        }

        /// <summary>
        /// Get grand total data.
        /// </summary>
        /// <param name="dataListOutright">The out right data list.</param>
        /// <param name="total">The total data.</param>
        /// <returns>The grand total. </returns>
        private static MatchWinLoss GetMatchWinLossGrandTotal(IList<MatchWinLoss> dataListOutright, MatchWinLoss total)
        {
            var grandTotalMatchWinLoss = new MatchWinLoss
            {
                Turnover =
                    dataListOutright.Sum(item => item.Turnover) + total.Turnover,
                CustomerWl =
                    dataListOutright.Sum(item => item.CustomerWl) +
                    total.CustomerWl,
                CustomerComm =
                    dataListOutright.Sum(item => item.CustomerComm) +
                    total.CustomerComm,
                LicenseeWl =
                    dataListOutright.Sum(item => item.LicenseeWl) +
                    total.LicenseeWl,
                BaCommission = dataListOutright.Sum(item => item.BaCommission) +
                               total.BaCommission,
                AmtDueToBa = dataListOutright.Sum(item => item.AmtDueToBa) +
                             total.AmtDueToBa,
                RacingRevenue = dataListOutright.Sum(item => item.RacingRevenue) +
                                total.RacingRevenue,
                AmtDueToRacing = dataListOutright.Sum(item => item.AmtDueToRacing) +
                                 total.AmtDueToRacing
            };

            return grandTotalMatchWinLoss;
        }

        #region Obsolete

        /// <summary>
        ///     This function is obsolete, and will be replaced by newer function soon.
        ///     Recalculate extra column such as product name, match name, league name, for easy to sort.
        ///     This function is obsolete, and will be replaced by newer function soon.
        /// </summary>
        /// <param name="mainTable">The main table data.</param>
        /// <returns>The data table.</returns>
        [SuppressMessage(
              "StyleCop.CSharp.Nexcel.NexcelCustomRules",
              "SP2101:MethodMustNotContainMoreLinesThan",
              Justification = "Reviewed. Suppression is OK here.")]
        private static DataTable SortMatchWinlossTable(DataTable mainTable)
        {
            mainTable.Columns.Add("productname", typeof(string));
            mainTable.Columns.Add("newproductid", typeof(int));
            mainTable.Columns.Add("matchname", typeof(string));
            mainTable.Columns.Add("leaguename", typeof(string));

            /*Sort data*/
            foreach (DataRow row in mainTable.Rows)
            {
                /* by product name*/
                var betType = Converter.ToInt(row["bettype"]);
                if (betType >= 81 && betType < 120)
                {
                    row["productname"] = Product.Ng;
                    row["newproductid"] = 3;
                }
                else if (betType >= 31 && betType <= 33)
                {
                    row["productname"] = Product.Rb;
                    row["newproductid"] = 2;
                }
                else if (betType >= 1101 && betType <= 1199)
                {
                    row["productname"] = Product.Lc;
                    row["newproductid"] = 4;
                }
                else if (betType >= 1021 && betType <= 1299)
                {
                    row["productname"] = Product.Vs;
                    row["newproductid"] = 5;
                }
                else
                {
                    row["productname"] = Product.Sb;
                    row["newproductid"] = 1;
                }

                /* by Match name*/
                switch (row["productname"].ToString())
                {
                    case Product.Rb:
                        row["matchname"] = ViewResources.Race + Converter.ToString2(row["race"]);
                        break;

                    case Product.Ng:
                        row["matchname"] = ViewResources.NumberGameNo + " " + row["matchcode"];
                        break;

                    case Product.Lc:

                        row["matchname"] = string.Format("{0} {1} {2} ", CacheRef.GetSportName(162), CacheRef.GetBettype(row["bettype"]), row["showtime"]);
                        break;

                    default:
                        // Virtual Horse Racing
                        var virtualHrBettypes = new List<int> { 1231, 1232, 1233, 1237, 1238 };
                        if (virtualHrBettypes.Contains(betType))
                        {
                            var culture = new CultureInfo("en-US");
                            var timeFormatter = "{0:MM/dd/yyyy hh:mm:ss tt}";
                            var kickOffTime = Converter.ToDateTime(row["KickoffTime"]);
                            row["matchname"] = string.Format(culture, timeFormatter, kickOffTime);
                            break;
                        }

                        var homeName = CacheRef.GetTeamName(row["homeid"]) +
                                       (Converter.ToBool(row["isneutral"])
                                            ? " " + ViewResources.IsNeutral
                                            : string.Empty);
                        var awayName = CacheRef.GetTeamName(row["awayid"]);
                        row["matchname"] = homeName + "-vs-" + awayName;

                        break;
                }

                switch (row["productname"].ToString())
                {
                    case Product.Rb:
                        row["leaguename"] = ViewResources.Racing + " - " + CacheRef.GetLeagueName(row["leagueid"]);
                        break;

                    case Product.Ng:
                        row["leaguename"] = ViewResources.NumberGame;
                        break;

                    default:
                        row["leaguename"] = CacheRef.GetLeagueName(row["leagueid"]);
                        break;
                }
            }

            return mainTable;
        }

        #endregion Obsolete

        /// <summary>
        /// Count number of column.
        /// </summary>
        /// <param name="licenseeSession">The licensee session.</param>
        /// <returns>The number of column.</returns>
        private int GetMatchWinlossColumnCount(LicenseeSession licenseeSession)
        {
            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, IgnoreCasinoAndBingoAndKeno);
            var columnCount = 5;
            if (productAccess.IsAllowBa)
            {
                columnCount += 2;
            }

            if (productAccess.IsAllowRb)
            {
                columnCount += 2;
            }

            if (SessionService.LicenseeSession.IsAllowedCustomerComm)
            {
                columnCount += 2;
            }

            return columnCount;
        }

        #endregion Utility     
    }
}