namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class BettingTicketCenterController : BaseApiController
    {
        private readonly IBridgeClientService _bridgeClientService;

        public BettingTicketCenterController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IBridgeClientService bridgeClientService)
            : base(sessionService, productService, langService)
        {
            _bridgeClientService = bridgeClientService;
        }

        [HttpPost]
        public async Task<dynamic> GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId, userCurrencyList))
            {
                return null;
            }

            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = userCurrencyList;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var betlistAPIUrl = new Uri($"http://localhost:55050/BettingTicketCenter/GetBySingleCustomer");
            return await _bridgeClientService.PostAsync(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> GetByRefno(RefNoBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = SessionService.UserSession.CurrencyList;

            var betlistAPIUrl = new Uri($"http://localhost:55050/BettingTicketCenter/GetByRefno");
            return await _bridgeClientService.PostAsync(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            var userCurrencyList = SessionService.UserSession.CurrencyList;

            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId, userCurrencyList))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = SessionService.UserSession.CurrencyList;

            var betlistAPIUrl = new Uri($"http://localhost:55050/BettingTicketCenter/GetByRunning");
            return await _bridgeClientService.PostAsync(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        private static bool IsValidCurrencyId(int currencyId, IDictionary<int, string> currencyList)
        {
            return currencyId == BaseCurrencyType.All || currencyList.ContainsKey(currencyId);
        }
    }
}