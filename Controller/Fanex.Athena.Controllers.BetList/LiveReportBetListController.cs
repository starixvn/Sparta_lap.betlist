namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
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
            var model = CreateDefaultMetaModel(metaModel);
            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossByMatch";
                    var matchName = model.HomeId > 0 && model.AwayId > 0
                        ? CacheRef.GetTeamName(model.HomeId) + " vs " + CacheRef.GetTeamName(model.AwayId) + " "
                        : string.Empty;
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        matchName + CacheRef.GetBettype(model.BetType),
                        model.FromDate.ToShortDateString(),
                        model.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, model.IsConvert ? model.BaseCurrency.ToUpperInvariant() : model.CurrencyName));
                    return Excel(model, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/Match.cshtml", model);
        }

        [HttpGet]
        public ActionResult MixParlay(LiveReportBetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossParlay";
                    var matchName = model.HomeId > 0 && model.AwayId > 0
                        ? CacheRef.GetTeamName(model.HomeId) + " vs " + CacheRef.GetTeamName(model.AwayId) + "- "
                        : string.Empty;
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        matchName + CacheRef.GetBettype(model.BetType),
                        model.FromDate.ToShortDateString(),
                        model.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, model.IsConvert ? model.BaseCurrency.ToUpperInvariant() : model.CurrencyName));
                    return Excel(model, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/MixParlay.cshtml", model);
        }

        [HttpGet]
        public ActionResult Outright(LiveReportBetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);

            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossOutright";
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.Outright,
                        model.FromDate.ToShortDateString(),
                        model.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, model.IsConvert ? model.BaseCurrency.ToUpperInvariant() : model.CurrencyName));
                    return Excel(model, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/Outright.cshtml", model);
        }

        [HttpGet]
        public ActionResult NumberGame(LiveReportBetListMetaModel metaModel)
        {
            var model = CreateDefaultMetaModel(metaModel);
            if (model.IsExportExcel)
            {
                var currencyList = SessionService.UserSession.CurrencyList;

                if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
                {
                    const string UspKey = "Lrf_BetList_LiveReport_SelWinlossNumberGame";
                    var title = ViewResources.BetList

                        + string.Format(
                        " - {0} - {1} --> {2} - {3}",
                        ViewResources.ProductNG,
                        model.FromDate.ToShortDateString(),
                        model.ToDate.ToShortDateString(),
                        string.Format(ViewResources.Viewin, model.IsConvert ? model.BaseCurrency.ToUpperInvariant() : model.CurrencyName));
                    return Excel(model, UspKey, title);
                }

                return EmptyExcel();
            }

            return View("~/Views/BetList/LiveReport/NumberGame.cshtml", model);
        }

        [HttpGet]
        public ActionResult Racing(LiveReportBetListMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_LiveReport_SelWinlossRacing";

            var model = CreateDefaultMetaModel(metaModel);
            var sportName = CacheRef.GetSportName(model.SportType);
            if (!model.IsExportExcel)
            {
                ViewBag.SportName = sportName;
                return View("~/Views/BetList/LiveReport/Racing.cshtml", model);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
            {
                var currencyName = string.Format(ViewResources.Viewin, model.IsConvert ? model.BaseCurrency.ToUpperInvariant() : model.CurrencyName);
                var title = ViewResources.BetList
                    + $" - {sportName} - {model.FromDate.ToShortDateString()} --> {model.ToDate.ToShortDateString()} - {currencyName}";

                return Excel(model, UspName, title);
            }

            return EmptyExcel();
        }

        [HttpGet]
        public ActionResult VirtualSports(LiveReportBetListMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

            var model = CreateDefaultMetaModel(metaModel);
            var sportName = CacheRef.GetSportName(model.SportType);
            if (!model.IsExportExcel)
            {
                ViewBag.SportName = sportName;
                return View("~/Views/BetList/LiveReport/VirtualSports.cshtml", model);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
            {
                var currencyName = string.Format(ViewResources.Viewin, model.IsConvert ? model.BaseCurrency.ToUpperInvariant() : model.CurrencyName);
                var title = ViewResources.BetList
                    + $" - {sportName} - {model.FromDate.ToShortDateString()} --> {model.ToDate.ToShortDateString()} - {currencyName}";

                return Excel(model, UspName, title);
            }

            return EmptyExcel();
        }

        private ActionResult Excel(BetListMetaModel metaModel, string uspKey, string title)
        {
            // Sum value by page
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            ticketResult.SumWinLoss = ticketResult.MainTicketList.Sum(x => x.Winlost);
            ticketResult.SumLicenseeWL = ticketResult.MainTicketList.Sum(x => x.LicenseeWL);
            ticketResult.SumComm = ticketResult.MainTicketList.Sum(x => x.Comm);
            ticketResult.SumPlayerComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
            };
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
            var dailyBetListExcel = new BetListExcel(showingOption, metaModel.LicenseeId, new List<int> { BetEffectiveColumn }); // 1: externalid, 2: systemid
            var workbook = dailyBetListExcel.Export(ticketResult, title, startIndex);

            return new ExcelResult(workbook, "LiveReportBetList");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Reviewed.")]
        private LiveReportBetListMetaModel CreateDefaultMetaModel(LiveReportBetListMetaModel metaModel = null)
        {
            var defaultMetaModel = metaModel ?? new LiveReportBetListMetaModel();
            defaultMetaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            defaultMetaModel.UserId = SessionService.UserSession.UserId;

            var balanceUp = GetBalanceUp();
            defaultMetaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            defaultMetaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            defaultMetaModel.FromDate = DateTime.SpecifyKind(defaultMetaModel.FromDate.Date, DateTimeKind.Utc);
            defaultMetaModel.ToDate = DateTime.SpecifyKind(defaultMetaModel.ToDate.Date, DateTimeKind.Utc);
            defaultMetaModel.Date = DateTime.SpecifyKind(defaultMetaModel.Date.Date, DateTimeKind.Utc);

            return defaultMetaModel;
        }
    }
}