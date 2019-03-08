namespace Fanex.Athena.Api.BetList
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Helpers;
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
            var userCurrencyList = SessionService.UserSession.CurrencyList;

            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = userCurrencyList;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml();

            return await _betListApiClient.PostAsync("/BettingTicketCenter/GetBySingleCustomer", metaModel).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> GetByRefno(RefNoBetListMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = SessionService.UserSession.CurrencyList;

            return await _betListApiClient.PostAsync("/BettingTicketCenter/GetByRefno", metaModel).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<dynamic> GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            var currencyList = metaModel.CurrencyIdList?.Split(',').Select(x => int.Parse(x)).ToArray();
            if (!metaModel.IsValid || !BetListHelper.IsValidCurrencyId(currencyList, userCurrencyList))
            {
                return null;
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(currencyList);
            metaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;
            metaModel.UserCurrencyList = SessionService.UserSession.CurrencyList;

            return await _betListApiClient.PostAsync("/BettingTicketCenter/GetByRunning", metaModel).ConfigureAwait(false);
        }
    }
}