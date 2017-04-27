// -----------------------------------------------------------------------
// <copyright file="ReportController.MatchWinLoss.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Fanex.Common;
    using Resources;
    using Service.Report.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportMatchWinLoss)]
    public class MatchWinLossReportController : ReportBaseController
    {
        private readonly IMatchWinLossService _matchWinLossService;

        public MatchWinLossReportController()
        {
            _matchWinLossService = new MatchWinLossService();
        }

        public MatchWinLossReportController(IMatchWinLossService matchWinLossService)
        {
            _matchWinLossService = matchWinLossService ?? new MatchWinLossService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new MatchWinLossMetaModel
            {
                BalanceUpDay = BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                LicenseeId = licenseeSession.LicenseeId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var productAccess = ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.MatchWinLoss);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();
            metaModel.ProductOptions = productOption;

            var model = new MatchWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Report/MatchWinLoss/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(MatchWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var currencies = userSession.CurrencyList;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            if (metaModel.IsExportExcel)
            {
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
                {
                    return Excel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            var reportData = _matchWinLossService.GetDataList(metaModel);
            var reportDataOutrightParlay = new List<MatchWinLoss>();

            if (metaModel.ProductOptions.Count(i => (i.Code == Product.BA || i.Code == Product.SB) && i.Status) > 0)
            {
                reportDataOutrightParlay = _matchWinLossService.GetOutrightParlayList(metaModel).ToList();
            }

            var total = GetTotal(reportData);
            var grandTotal = GetGrandTotal(reportDataOutrightParlay, total);
            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.MatchWinLoss);
            var model = new MatchWinLossViewModel
            {
                Param = metaModel,
                MainResult = reportData,
                LeagueList = CreateLeagueList(reportData),
                SpecialBetTypeResult = reportDataOutrightParlay,

                CurrencyList = currencies,
                ProductAccess = productAccess,
                ColumnCount = GetColumnCount(productAccess, licenseeSession.IsAllowedCustomerComm),
                Total = total,
                GrandTotal = grandTotal,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };
            return View("~/Views/Report/MatchWinLoss/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Detail(MatchWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var reportData = _matchWinLossService.GetDetailDataList(metaModel);

            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.MatchWinLoss);

            var model = new MatchWinLossViewModel
            {
                MainResult = reportData,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Report/MatchWinLoss/Content.data.Detail.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(MatchWinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            const string FileName = nameof(MatchWinLoss);
            var templatePath = Server.MapPath(@"~/App_Data/Template/Reports/MatchWinLoss.xls");
            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.MatchWinLoss);
            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                LCVisible = productAccess.IsAllowLC,
                SBVisible = productAccess.IsAllowSB,
                VSVisible = productAccess.IsAllowVS,
                KNVisible = productAccess.IsAllowKN,
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
            mainTable = AddColumnsForSorting(mainTable);
            mainTable.DefaultView.Sort = "newproductid asc, leaguename asc, matchname ASC";
            mainTable = mainTable.DefaultView.ToTable();
            var matchWinLossExcel = new MatchWinLossExcel(
                templatePath,
                convertingOption,
                showingOption,
                mainTable,
                reportData.Tables[1],
                metaModel)
            {
                ProductOptions = metaModel.ProductOptions
            };

            matchWinLossExcel.Export();

            return new ExcelResult(matchWinLossExcel.Workbook, FileName);
        }

        private static IDictionary<int, string> CreateLeagueList(IList<MatchWinLoss> reportData)
        {
            var leagueList = new Dictionary<int, string>();
            for (int i = 0, count = reportData.Count; i < count; i++)
            {
                var item = reportData[i];
                if (!leagueList.ContainsKey(item.LeagueId))
                {
                    leagueList.Add(item.LeagueId, item.LeagueName);
                }
            }

            return leagueList;
        }

        private static MatchWinLoss GetTotal(IList<MatchWinLoss> dataList)
        {
            var totalMatchWinLoss = new MatchWinLoss
            {
                Turnover = dataList.Sum(item => item.Turnover),
                TotalTransaction = dataList.Sum(item => item.TotalTransaction),
                CustomerWL = dataList.Sum(item => item.CustomerWL),
                CustomerComm = dataList.Sum(item => item.CustomerComm),
                LicenseeWL = dataList.Sum(item => item.LicenseeWL),
                BACommission = dataList.Sum(item => item.BACommission),
                AmtDueToBA = dataList.Sum(item => item.AmtDueToBA),
                RacingRevenue = dataList.Sum(item => item.RacingRevenue),
                AmtDueToRacing = dataList.Sum(item => item.AmtDueToRacing)
            };

            return totalMatchWinLoss;
        }

        private static MatchWinLoss GetGrandTotal(IList<MatchWinLoss> dataListOutright, MatchWinLoss total)
        {
            return new MatchWinLoss
            {
                Turnover = dataListOutright.Sum(item => item.Turnover) + total.Turnover,
                TotalTransaction = dataListOutright.Sum(item => item.TotalTransaction) + total.TotalTransaction,
                CustomerWL = dataListOutright.Sum(item => item.CustomerWL) + total.CustomerWL,
                CustomerComm = dataListOutright.Sum(item => item.CustomerComm) + total.CustomerComm,
                LicenseeWL = dataListOutright.Sum(item => item.LicenseeWL) + total.LicenseeWL,
                BACommission = dataListOutright.Sum(item => item.BACommission) + total.BACommission,
                AmtDueToBA = dataListOutright.Sum(item => item.AmtDueToBA) + total.AmtDueToBA,
                RacingRevenue = dataListOutright.Sum(item => item.RacingRevenue) + total.RacingRevenue,
                AmtDueToRacing = dataListOutright.Sum(item => item.AmtDueToRacing) + total.AmtDueToRacing
            };
        }

        #region Obsolete

        private static DataTable AddColumnsForSorting(DataTable mainTable)
        {
            mainTable.Columns.Add("productname", typeof(string));
            mainTable.Columns.Add("newproductid", typeof(int));
            mainTable.Columns.Add("matchname", typeof(string));
            mainTable.Columns.Add("leaguename", typeof(string));

            foreach (DataRow row in mainTable.Rows)
            {
                var betType = Converter.ToInt(row["bettype"]);
                AssignProduct(row, betType);

                AssignMatchName(row, betType);

                AssignLeagueName(row);
            }

            return mainTable;
        }

        private static void AssignProduct(DataRow row, int betType)
        {
            if (betType >= 81 && betType < 120)
            {
                row["productname"] = Product.NG;
                row["newproductid"] = 3;
            }
            else if (betType >= 31 && betType <= 33)
            {
                row["productname"] = Product.RB;
                row["newproductid"] = 2;
            }
            else if (betType >= 1101 && betType <= 1199)
            {
                row["productname"] = Product.LC;
                row["newproductid"] = 4;
            }
            else if (betType >= 1021 && betType <= 1299)
            {
                row["productname"] = Product.VS;
                row["newproductid"] = 5;
            }
            else
            {
                row["productname"] = Product.SB;
                row["newproductid"] = 1;
            }
        }

        private static void AssignMatchName(DataRow row, int betType)
        {
            switch (row["productname"].ToString())
            {
                case Product.RB:
                    {
                        row["matchname"] = ViewResources.Race + Converter.ToString2(row["race"]);
                        break;
                    }

                case Product.NG:
                    {
                        row["matchname"] = ViewResources.NumberGameNo + " " + row["matchcode"];
                        break;
                    }

                case Product.LC:
                    {
                        row["matchname"] = string.Format("{0} {1} {2} ", CacheRef.GetSportName(162), CacheRef.GetBettype(row["bettype"]), row["showtime"]);
                        break;
                    }

                default:
                    {
                        // Virtual Horse Racing
                        var virtualHrBettypes = new List<int> { 1231, 1232, 1233, 1237, 1238 };
                        if (virtualHrBettypes.Contains(betType))
                        {
                            var culture = new CultureInfo("en-US");
                            const string TimeFormatter = "{0:MM/dd/yyyy hh:mm:ss tt}";
                            var kickOffTime = Converter.ToDateTime(row["KickoffTime"]);
                            row["matchname"] = string.Format(culture, TimeFormatter, kickOffTime);
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
            }
        }

        private static void AssignLeagueName(DataRow row)
        {
            switch (row["productname"].ToString())
            {
                case Product.RB:
                    row["leaguename"] = ViewResources.ProductRB + " - " + CacheRef.GetLeagueName(row["leagueid"]);
                    break;

                case Product.NG:
                    row["leaguename"] = ViewResources.ProductNG;
                    break;

                default:
                    row["leaguename"] = CacheRef.GetLeagueName(row["leagueid"]);
                    break;
            }
        }

        #endregion Obsolete

        private static int GetColumnCount(ProductAccess productAccess, bool isAllowedCustomerComm)
        {
            var columnCount = 6;
            if (productAccess.IsAllowBA)
            {
                columnCount += 2;
            }

            if (productAccess.IsAllowRB)
            {
                columnCount += 2;
            }

            if (isAllowedCustomerComm)
            {
                columnCount += 2;
            }

            return columnCount;
        }
    }
}