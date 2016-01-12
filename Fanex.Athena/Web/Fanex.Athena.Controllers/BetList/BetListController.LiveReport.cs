// -----------------------------------------------------------------------
// <copyright file="BetListController.LiveReport.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Web.ResultExtension;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for live report bet list by match.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult LiveReportMatch(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateLiveReportDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";
                    var matchName = metaModel.HomeId > 0 && metaModel.AwayId > 0
                        ? CacheRef.GetTeamName(metaModel.HomeId) + " vs " + CacheRef.GetTeamName(metaModel.AwayId) + " "
                        : string.Empty;
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        matchName + CacheRef.GetBettype(metaModel.BetType),
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));
                    return LiveReportBetListExcel(metaModel, UspKey, title);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/LiveReport/Match.cshtml", metaModel);
        }

        /// <summary>
        /// Action Get for live report mix parlay bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult LiveReportMixParlay(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateLiveReportDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";
                    var matchName = metaModel.HomeId > 0 && metaModel.AwayId > 0
                        ? CacheRef.GetTeamName(metaModel.HomeId) + " vs " + CacheRef.GetTeamName(metaModel.AwayId) + "- "
                        : string.Empty;
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        matchName + CacheRef.GetBettype(metaModel.BetType),
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));
                    return LiveReportBetListExcel(metaModel, UspKey, title);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/LiveReport/MixParlay.cshtml", metaModel);
        }

        /// <summary>
        /// Action Get for live report outright bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult LiveReportOutright(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateLiveReportDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";

                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.Outright,
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));
                    return LiveReportBetListExcel(metaModel, UspKey, title);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/LiveReport/Outright.cshtml", metaModel);
        }

        /// <summary>
        /// Action Get for live report number game bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult LiveReportNumberGame(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateLiveReportDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";

                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.ProductNG,
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));
                    return LiveReportBetListExcel(metaModel, UspKey, title);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/LiveReport/NumberGame.cshtml", metaModel);
        }

        /// <summary>
        /// Action Get for live report micro gaming bet list.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult LiveReportMicroGaming(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateLiveReportDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossMicroGaming";

                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.ProductNG,
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));
                    return LiveReportBetListExcel(metaModel, UspKey, title);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/LiveReport/MicroGaming.cshtml", metaModel);
        }

        /// <summary>
        /// The racing live report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult LiveReportRacing(LiveReportBetListMetaModel metaModel)
        {
            const string UspKeyRacing = "Lrf_BetList_LiveReport_SelWinlossRacing";
            const string UspKeyVHRacing = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

            metaModel = CreateLiveReportDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.ProductRB,
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));

                    var racingBettypes = new List<int>()
                    {
                        (int)SportType.VirtualHorseRacing,
                        (int)SportType.VirtualGreyhound,
                        (int)SportType.VirtualMotoRacing,
                        (int)SportType.VirtualSpeedway,
                        (int)SportType.VirtualCycling
                    };

                    var spkey = racingBettypes.Contains(metaModel.SportType) ? UspKeyVHRacing : UspKeyRacing;

                    return LiveReportBetListExcel(metaModel, spkey, title);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            return View("~/Views/BetList/LiveReport/Racing.cshtml", metaModel);
        }

        /// <summary>
        /// Export excel for live report bet list by match.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="uspKey">The store procedure key.</param>
        /// <param name="title">The report title.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        public ActionResult LiveReportBetListExcel(BetListMetaModel metaModel, string uspKey, string title)
        {
            // Sum value by page
            var ticketResult = this._betListService.GetBetListData(metaModel, uspKey);

            ticketResult.SumWinLoss = ticketResult.MainTicketList.Sum(x => x.Winlost);
            ticketResult.SumLicenseeWL = ticketResult.MainTicketList.Sum(x => x.LicenseeWL);
            ticketResult.SumComm = ticketResult.MainTicketList.Sum(x => x.Comm);
            ticketResult.SumPlayerComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            var dailyBetListExcel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, new List<int> { BetEffectiveColumn }); // 1: externalid, 2: systemid
            var workbook = dailyBetListExcel.Export(ticketResult, title, startIndex);

            return new ExcelResult(workbook, "LiveReportBetList");
        }

        #region Utility

        /// <summary>
        /// Create default live report meta model.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Reviewed.")]
        public LiveReportBetListMetaModel CreateLiveReportDefaultMetaModel(LiveReportBetListMetaModel metaModel = null)
        {
            metaModel = metaModel ?? new LiveReportBetListMetaModel();
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var balanceUpInfo = this._balanceUpService.BalanceUpInfo;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.FromDate = DateTime.SpecifyKind(metaModel.FromDate.Date, DateTimeKind.Utc);
            metaModel.ToDate = DateTime.SpecifyKind(metaModel.ToDate.Date, DateTimeKind.Utc);
            metaModel.Date = DateTime.SpecifyKind(metaModel.Date.Date, DateTimeKind.Utc);

            return metaModel;
        }

        #endregion Utility
    }
}