namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Athena.BetList.Model.ViewModel;
    using Base;
    using Base.Attributes;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Service.Core.Utility.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossBetListController : BaseApiController
    {
        private readonly IBridgeClientService _bridgeClientService;

        private readonly IAuthorizationService _authorizationService;

        public ExchangeWinLossBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IAuthorizationService authorizationService)

        : base(sessionService, productService, languageService)
        {
            _bridgeClientService = bridgeClientService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public async Task<dynamic> Index(ExchangeBetListMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                return new ExchangeBetListViewModel
                {
                    MainData = null
                };
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);

            var betlistAPIUrl = new Uri($"http://localhost:55050/ExchangeWinLossBetList/GetData");
            return await _bridgeClientService.PostAsync(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> Detail(ExchangeBetListDetailMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                return new List<ExchangeTicketDetail>();
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);

            var betlistAPIUrl = new Uri($"http://localhost:55050/ExchangeWinLossBetList/Detail");
            return await _bridgeClientService.PostAsync(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        private bool IsValidRequest(BaseExchangeBetListMetaModel metaModel)
        {
            return metaModel.IsValid
                && (metaModel.CurrencyId == BaseCurrencyType.All || KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(
                    SessionService.UserSession.CurrencyList, SessionService.LicenseeSession.LicenseeId, metaModel.CurrencyId))
                && _authorizationService.HasPermissionOnData(SessionService.LicenseeSession.LicenseeId, metaModel.CustId, 0);
        }
    }
}