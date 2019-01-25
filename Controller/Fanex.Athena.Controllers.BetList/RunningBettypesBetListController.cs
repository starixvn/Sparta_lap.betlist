// -----------------------------------------------------------------------
// <copyright file="RunningBettypesBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Linq;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ActionFilter;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    /// <summary>
    /// RunningBetList contains bet list:.
    /// Mix Parlay              - bet type: 9.
    /// Clean Sheet             - bet type: 13.
    /// Double Chance           - bet type: 24.
    /// B/O/N team to score     - bet type: 26.
    /// To win to nil           - bet type: 27.
    /// 3-Way Handicap          - bet type: 28.
    /// </summary>
    public class RunningBettypesBetListController : BetListBaseController
    {
        private const string DoubleChangeBettypes = "24,151,186,410,431";

        private const string BONTeamToScoreBettypes = "26,145,146,170,188,427,433";

        public RunningBettypesBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
        : base(languageService, sessionService, appSetting, productService, betListService, balanceUpDataAccess, cacheRefAdapter)
        {
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetMixParlay)]
        public ActionResult MixParlay(BetListExcelMetaModel metaModel)
        {
            metaModel.MultipleBettype = ((int)BetType.CombinationMixParlay).ToString();
            ViewBag.ReportTitle = Menu.Menu_12;
            ViewBag.UserGuideName = "TotalBetsMixParlay";
            return ProcessRequest(metaModel);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetDoubleChance)]
        public ActionResult DoubleChange(BetListExcelMetaModel metaModel)
        {
            metaModel.MultipleBettype = DoubleChangeBettypes;
            ViewBag.ReportTitle = ViewResources.DoubleChance;
            ViewBag.UserGuideName = "TotalBetsDoubleChance";
            return ProcessRequest(metaModel);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetBONTeamToScore)]
        public ActionResult BONTeamToScore(BetListExcelMetaModel metaModel)
        {
            metaModel.MultipleBettype = BONTeamToScoreBettypes;
            ViewBag.ReportTitle = ViewResources.BONTeamToScore;
            ViewBag.UserGuideName = "TotalBetsBON";
            return ProcessRequest(metaModel);
        }

        private ActionResult ProcessRequest(BetListExcelMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            CreateDefaultMetaModel(metaModel);
            var currencies = userSession.CurrencyList;

            if (metaModel.IsExportExcel)
            {
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
                {
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                    return Excel(metaModel);
                }

                return EmptyExcel();
            }

            SetConvetedUSDCurrencyName(metaModel);

            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = currencies,
                ProductAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Desktop/BetList/RunningBetListMultipleBettypes/Main.cshtml", model);
        }

        private ActionResult Excel(BetListExcelMetaModel metaModel)
        {
            metaModel.UspKey = "Lrf_BetList_SelRunningByBettypeList";

            metaModel.ShowingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var firstBettype = metaModel.MultipleBettype.Split(',').FirstOrDefault();
            metaModel.Title = $"{CacheRefAdapter.GetBettype(firstBettype)} - {GetViewInCurrency(metaModel)}";

            metaModel.IsShowSumStake = true;
            metaModel.HiddenColumns = HiddenColumnIndexes;

            return ProcessExportExcel(metaModel, "RunningBetList");
        }
    }
}