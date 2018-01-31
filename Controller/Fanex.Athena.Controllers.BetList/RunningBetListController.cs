// -----------------------------------------------------------------------
// <copyright file="RunningBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;
    using Web.ActionFilter;

    #endregion Using

    /// <summary>
    /// RunningBetList contains bet list:.
    /// Mix Parlay              - bet type: 9.
    /// Clean Sheet             - bet type: 13.
    /// Double Chance           - bet type: 24.
    /// B/O/N team to score     - bet type: 26.
    /// To win to nil           - bet type: 27.
    /// 3-Way Handicap          - bet type: 28.
    /// 1H 3-Way Handicap       - bet type: 453.
    /// </summary>
    public class RunningBetListController : BetListBaseController
    {
        public RunningBetListController()
        {
        }

        public RunningBetListController(
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
        [FunctionAuthorization(PermissionKeys.TotalBetCleanSheet)]
        public ActionResult CleanSheet(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.CleanSheet;
            ViewBag.ReportTitle = ViewResources.CleanSheet;
            ViewBag.UserGuideName = "TotalBetsCleanSheet";
            return ProcessRequest(metaModel);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetToWinToNil)]
        public ActionResult ToWinToNil(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.ToWinToNil;
            ViewBag.ReportTitle = ViewResources.ToWinToNil;
            ViewBag.UserGuideName = "TotalBetsToWinToNil";
            return ProcessRequest(metaModel);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBet3WayHandicap)]
        public ActionResult ThreeWayHandicap(BetListMetaModel metaModel)
        {
            metaModel.BetType = (int)BetType.ThreeWayHandicap;
            metaModel.MultipleBettype = $"{(int)BetType.ThreeWayHandicap},{(int)BetType.FirstHalfThreeWayHandicap}";
            ViewBag.ReportTitle = ViewResources.lbl3WayHandicap;
            ViewBag.UserGuideName = "TotalBets3WayHandicap";
            return ProcessRequest(metaModel, "Lrf_BetList_SelRunningByBettypeList");
        }

        private ActionResult ProcessRequest(BetListMetaModel metaModel, string uspKey = "Lrf_BetList_SelRunningByBettype")
        {
            var userSession = SessionService.UserSession;
            var viewParam = CreateDefaultMetaModel(metaModel);
            var currencies = userSession.CurrencyList;

            if (viewParam.IsExportExcel)
            {
                if (viewParam.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(viewParam.CurrencyId))
                {
                    viewParam.AgentListXml = SessionService.GenerateAgentListXml(viewParam.CurrencyId);
                    return Excel(viewParam, uspKey);
                }

                return EmptyExcel();
            }

            SetConvetedUSDCurrencyName(viewParam);

            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = viewParam,
                CurrencyList = currencies,
                ProductAccess = ProductService.GetProductAccess(viewParam.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/RunningBetList/Main.cshtml", model);
        }

        private ActionResult Excel(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = BetListService.GetBetListData(metaModel, uspKey);

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var baseCurrency = GetViewInCurrency(metaModel);
            var title = $"{CacheRefAdapter.GetBettype(metaModel.BetType)} - {baseCurrency}";

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId, HiddenColumnIndexes, true);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RunningBetList");
        }
    }
}