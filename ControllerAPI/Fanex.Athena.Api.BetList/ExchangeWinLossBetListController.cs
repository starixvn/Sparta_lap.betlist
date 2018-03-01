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
        private readonly IExchangeWinLossBetListService _service;

        public ExchangeWinLossBetListController()
        {
            _service = new ExchangeWinLossBetListService();
        }

        public ExchangeWinLossBetListController(IExchangeWinLossBetListService service)
        {
            _service = service;
        }

        [HttpPost]
        public ExchangeBetListSummaryViewModel Index(BetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                return null;
            }

            ////metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var viewModel = _service.GetBetListData(metaModel);

            return viewModel;
        }

        [HttpPost]
        public IEnumerable<ExchangeTicketDetail> Detail()
        {
            return _service.GetTicketDetailList();
        }
    }
}