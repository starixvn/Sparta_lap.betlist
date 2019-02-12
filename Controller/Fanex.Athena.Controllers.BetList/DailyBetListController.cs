// -----------------------------------------------------------------------
// <copyright file="DailyBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Model.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ActionFilter;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.ReportDailyBetList)]
    public class DailyBetListController : BetListBaseController
    {
        private const int MaxPageSize = int.MaxValue - 1000;

        private const string BetListDailyBetListName = "BetList_DailyBetList";

        public DailyBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IBetListApiClient betListApiClient)
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter, betListApiClient)
        {
        }

        [HttpGet]
        public ActionResult Index(DailyBetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var balanceUp = GetBalanceUp();
            metaModel.BalanceUpDay = balanceUp.BalanceUpDay;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            var currencies = userSession.CurrencyList;

            if (metaModel.IsExportExcel)
            {
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
                {
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
                ProductAccess = this.ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, userSession.Language, BetListDailyBetListName),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Desktop/BetList/DailyBetList/Main.cshtml", model);
        }

        private ActionResult Excel(BetListExcelMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinlossDaily";

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.IsServerPaging = true;
            metaModel.PageSize = MaxPageSize;
            metaModel.PageIndex = 1;
            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.CurrencyList = UserCurrencyList;
            metaModel.UspKey = UspKey;

            // Sum value by page
            //ticketResult.SumWinLoss = ticketResult.MainTicketList.Sum(x => x.Winlost);
            //ticketResult.SumLicenseeWL = ticketResult.MainTicketList.Sum(x => x.LicenseeWL);
            //ticketResult.SumComm = ticketResult.MainTicketList.Sum(x => x.Comm);
            //ticketResult.SumPlayerComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);

            metaModel.ShowingOption = new BetListShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            var currencyTitle = GetViewInCurrency(metaModel);
            metaModel.Title = $"{ViewResources.DailyBetList} {ViewResources.On} {metaModel.Date.ToString("MM/dd/yyyy")} - {currencyTitle}";
            metaModel.HiddenColumns = new[] { BetEffectiveColumn };
            return ProcessExportExcel(metaModel, "DailyBetList", "/BetList/ExportExcelDailyBetList");
        }
    }
}