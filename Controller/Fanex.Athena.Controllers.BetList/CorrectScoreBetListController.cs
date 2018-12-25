// -----------------------------------------------------------------------
// <copyright file="CorrectScoreBetListController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
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

    [FunctionAuthorization(PermissionKeys.TotalBetCorrectScore)]
    public class CorrectScoreBetListController : BetListBaseController
    {
        public CorrectScoreBetListController(
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
        public ActionResult Index()
        {
            var metaModel = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(metaModel);

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Desktop/BetList/CorrectScoreBetList/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var currencies = SessionService.UserSession.CurrencyList;
            if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_TotalBets_Sel430";

                var excelModel = CreateDefaultMetaModel(metaModel);
                excelModel.AgentListXml = SessionService.GenerateAgentListXml(excelModel.CurrencyId);
                excelModel.IsShowSumStake = true;

                var ticketResult = BetListService.GetBetListData(excelModel, UserCurrencyList, UspKey);

                var showingOption = new ShowingOption
                {
                    CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                };

                var title = ViewResources.CorrectScore + " - " + GetViewInCurrency(excelModel);

                return ProcessExportExcel(showingOption, excelModel, ticketResult, HiddenColumnIndexes, title, "CorrectScoreBetList");
            }

            return EmptyExcel();
        }
    }
}