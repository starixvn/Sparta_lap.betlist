namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;
    using Service.BetList;
    using Service.BetList.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossBetListController : BaseApiController
    {
        private readonly IExchangeBetListService _service;

        public ExchangeWinLossBetListController()
        {
            _service = new ExchangeBetListService();
        }

        public ExchangeWinLossBetListController(IExchangeBetListService service)
        {
            _service = service;
        }

        [HttpPost]
        public ExchangeBetListViewModel Index(ExchangeBetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
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
        public IEnumerable<ExchangeTicketDetail> Detail(ExchangeBetListMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);

            return _service.GetTicketDetailList(metaModel);
        }
    }
}