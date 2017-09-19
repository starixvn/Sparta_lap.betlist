namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Match(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
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
                    return Excel(metaModel, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/Match.cshtml", metaModel);
        }

        [HttpGet]
        public ActionResult MixParlay(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
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
                    return Excel(metaModel, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/MixParlay.cshtml", metaModel);
        }

        [HttpGet]
        public ActionResult Outright(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.Outright,
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));
                    return Excel(metaModel, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/Outright.cshtml", metaModel);
        }

        [HttpGet]
        public ActionResult NumberGame(LiveReportBetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);
            if (metaModel.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.ProductNG,
                        metaModel.FromDate.ToShortDateString(),
                        metaModel.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));
                    return Excel(metaModel, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/NumberGame.cshtml", metaModel);
        }

        [HttpGet]
        public ActionResult Racing(LiveReportBetListMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_LiveReport_SelWinlossRacing";

            metaModel = CreateDefaultMetaModel(metaModel);
            var sportName = CacheRef.GetSportName(metaModel.SportType);
            if (!metaModel.IsExportExcel)
            {
                ViewBag.SportName = sportName;
                return View("~/Views/BetList/LiveReport/Racing.cshtml", metaModel);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var currencyName = string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
                var title = ViewResources.BetList
                    + $" - {sportName} - {metaModel.FromDate.ToShortDateString()} --> {metaModel.ToDate.ToShortDateString()} - {currencyName}";

                return Excel(metaModel, UspName, title);
            }

            return EmptyExcel();
        }

        [HttpGet]
        public ActionResult VirtualSports(LiveReportBetListMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

            metaModel = CreateDefaultMetaModel(metaModel);
            var sportName = CacheRef.GetSportName(metaModel.SportType);
            if (!metaModel.IsExportExcel)
            {
                ViewBag.SportName = sportName;
                return View("~/Views/BetList/LiveReport/VirtualSports.cshtml", metaModel);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var currencyName = string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
                var title = ViewResources.BetList
                    + $" - {sportName} - {metaModel.FromDate.ToShortDateString()} --> {metaModel.ToDate.ToShortDateString()} - {currencyName}";

                return Excel(metaModel, UspName, title);
            }

            return EmptyExcel();
        }

        private ActionResult Excel(BetListMetaModel metaModel, string uspKey, string title)
        {
            // Sum value by page
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = this.BetListService.GetBetListData(metaModel, uspKey);

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Reviewed.")]
        private LiveReportBetListMetaModel CreateDefaultMetaModel(LiveReportBetListMetaModel metaModel = null)
        {
            metaModel = metaModel ?? new LiveReportBetListMetaModel();
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var balanceUpInfo = this.BalanceUpService.BalanceUpInfo;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.FromDate = DateTime.SpecifyKind(metaModel.FromDate.Date, DateTimeKind.Utc);
            metaModel.ToDate = DateTime.SpecifyKind(metaModel.ToDate.Date, DateTimeKind.Utc);
            metaModel.Date = DateTime.SpecifyKind(metaModel.Date.Date, DateTimeKind.Utc);

            return metaModel;
        }
    }
}