namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;

    [ApiFunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossBetListController : BaseApiController
    {
        private readonly IExchangeBetListService _service;

        private readonly IAuthorizationService _authorizationService;
        
        public ExchangeWinLossBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IExchangeBetListService service,
            IAuthorizationService authorizationService)
        : base(sessionService, productService, languageService)
        {
            _service = service;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public ExchangeBetListViewModel Index(ExchangeBetListMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                return new ExchangeBetListViewModel
                {
                    MainData = null
                };
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);

            return _service.GetBetListData(metaModel);
        }

        [HttpPost]
        public IEnumerable<ExchangeTicketDetail> Detail(ExchangeBetListDetailMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                return new List<ExchangeTicketDetail>();
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);

            return _service.GetTicketDetailList(metaModel);
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