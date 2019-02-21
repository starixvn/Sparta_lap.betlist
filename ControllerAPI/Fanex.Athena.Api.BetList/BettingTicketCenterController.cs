namespace Fanex.Athena.Api.BetList
{
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
        private readonly IBetListApiClient _betListApiClient;

        public BettingTicketCenterController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IBetListApiClient betListApiClient)
            : base(sessionService, productService, langService)
        {
            _betListApiClient = betListApiClient;
        }

        [HttpPost]
        public async Task<dynamic> GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId, userSession.CurrencyList))
            {
                return null;
            }

            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = userSession.CurrencyList;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            return await _betListApiClient.PostAsync("/BettingTicketCenter/GetBySingleCustomer", metaModel, userSession.Language).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> GetByRefno(RefNoBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return null;
            }

            var userSession = SessionService.UserSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = userSession.CurrencyList;

            return await _betListApiClient.PostAsync("/BettingTicketCenter/GetByRefno", metaModel, userSession.Language).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;

            if (!metaModel.IsValid || !IsValidCurrencyId(metaModel.CurrencyId, userSession.CurrencyList))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = userSession.CurrencyList;

            return await _betListApiClient.PostAsync("/BettingTicketCenter/GetByRunning", metaModel, userSession.Language).ConfigureAwait(false);
        }

        private static bool IsValidCurrencyId(int currencyId, IDictionary<int, string> currencyList)
        {
            return currencyId == BaseCurrencyType.All || currencyList.ContainsKey(currencyId);
        }
    }
}