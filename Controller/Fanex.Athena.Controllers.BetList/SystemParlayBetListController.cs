// -----------------------------------------------------------------------
// <copyright file="SystemParlayBetListController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Model.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class SystemParlayBetListController : BetListBaseController
    {
        private readonly IBridgeClientService _bridgeClientService;

#pragma warning disable S107 // Methods should not have too many parameters

        public SystemParlayBetListController(
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
        public async Task<ActionResult> Detail(SystemParlayDetailMetaModel metaModel)
        {
            var balanceUp = GetBalanceUp();
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var betlistAPIUrl = new Uri($"http://localhost:55050/SystemParlayBetList/Detail");
            var data = await _bridgeClientService.PostAsyncString(betlistAPIUrl, metaModel).ConfigureAwait(false);

            var viewModel = new SystemParlayDetailViewModel
            {
                DetailText = data
            };

            return View("~/Views/Desktop/BetList/SystemParlayDetail/Main.cshtml", viewModel);
        }
    }
}