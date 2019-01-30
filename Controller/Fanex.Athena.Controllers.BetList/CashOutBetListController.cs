namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    public class CashOutBetListController : BetListBaseController
    {
        private readonly IBridgeClientService _bridgeClientService;

#pragma warning disable S107 // Methods should not have too many parameters

        public CashOutBetListController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IBridgeClientService bridgeClientService)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting, productService, balanceUpDataAccess, cacheRefAdapter)
        {
            _bridgeClientService = bridgeClientService;
        }

        [HttpGet]
        public async Task<ActionResult> Detail(CashOutBetListDetailMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            metaModel.HasPlayerComm = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = SessionService.UserSession.CurrencyList;

            var betlistAPIUrl = new Uri($"http://localhost:55050/CashOutBetList/Detail");
            var viewModel = await _bridgeClientService.PostAsync<CashOutBetListDetailViewModel>(betlistAPIUrl, metaModel).ConfigureAwait(false);

            return this.View("~/Views/Desktop/BetList/CashOutDetail/Main.cshtml", viewModel);
        }
    }
}