namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportLiveReport)]
    public class LiveReportBetListController : BetListBaseController
    {
        public LiveReportBetListController()
        {
        }

        public LiveReportBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(
            languageService,
            sessionService,
            appSetting,
            clientViewHelper,
            authorizationService,
            productService,
            betListService,
            balanceUpDataAccess,
            cacheRefAdapter)
        {
        }

        [HttpGet]
        public ActionResult Match(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossByMatch",
                "~/Views/BetList/LiveReport/Match.cshtml");
        }

        [HttpGet]
        public ActionResult MixParlay(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossParlay",
                "~/Views/BetList/LiveReport/MixParlay.cshtml");
        }

        [HttpGet]
        public ActionResult Outright(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossOutright",
                "~/Views/BetList/LiveReport/Outright.cshtml",
                ViewResources.Outright);
        }

        [HttpGet]
        public ActionResult NumberGame(LiveReportBetListMetaModel metaModel)
        {
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossNumberGame",
                "~/Views/BetList/LiveReport/NumberGame.cshtml",
                ViewResources.ProductNG);
        }

        [HttpGet]
        public ActionResult Racing(LiveReportBetListMetaModel metaModel)
        {
            var sportName = CacheRefAdapter.GetSportName(metaModel.SportType);
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossRacing",
                "~/Views/BetList/LiveReport/Racing.cshtml",
                sportName,
                sportName);
        }

        [HttpGet]
        public ActionResult VirtualSports(LiveReportBetListMetaModel metaModel)
        {
            var sportName = CacheRefAdapter.GetSportName(metaModel.SportType);
            return ProcessRequest(
                metaModel,
                "Lrf_BetList_LiveReport_SelWinlossVHRacing",
                "~/Views/BetList/LiveReport/VirtualSports.cshtml",
                sportName,
                sportName);
        }

        private string GetMatchName(LiveReportBetListMetaModel model)
        {
            return model.HomeId > 0 && model.AwayId > 0
                                ? CacheRefAdapter.GetTeamName(model.HomeId) + " vs " + CacheRefAdapter.GetTeamName(model.AwayId) + " "
                                : string.Empty;
        }

        private ActionResult ProcessRequest(
            LiveReportBetListMetaModel metaModel,
            string uspKey,
            string viewPath,
            string caption = null,
            string sportName = null)
        {
            var model = CreateDefaultMetaModel(metaModel);
            if (!model.IsExportExcel)
            {
                ViewBag.SportName = sportName;
                return View(viewPath, model);
            }

            var currencyList = SessionService.UserSession.CurrencyList;

            if (model.CurrencyId == BaseCurrencyType.All || currencyList.ContainsKey(model.CurrencyId))
            {
                var betlistCaption = string.IsNullOrEmpty(caption) ?
                    GetMatchName(model) + CacheRefAdapter.GetBettype(model.BetType) :
                    caption;
                var viewInCaption = string.Format(ViewResources.Viewin, model.IsConvert ? model.BaseCurrency.ToUpperInvariant() : model.CurrencyName);
                var title = ViewResources.BetList
                    + string.Format(
                    " - {0} - {1} --> {2} - {3}",
                    betlistCaption,
                    model.FromDate.ToShortDateString(),
                    model.ToDate.ToShortDateString(),
                    viewInCaption);
                return Excel(model, uspKey, title);
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