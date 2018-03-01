namespace Fanex.Athena.Api.BetList
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;

    [ApiFunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossBetListController : BaseApiController
    {
        private readonly IExchangeBetListDataAccess _dataAccess;

        public ExchangeWinLossBetListController()
        {
            _dataAccess = new ExchangeBetListDataAccess();
        }

        public ExchangeWinLossBetListController(IExchangeBetListDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        [HttpPost]
        public ExchangeBetListSummaryViewModel Index(ExchangeBetListMetaModel metaModel)
        {
            if (metaModel.CurrencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId))
            {
                return new ExchangeBetListSummaryViewModel
                {
                    MainData = null
                };
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);

            return _dataAccess.GetBetListData(metaModel);
        }

        [HttpPost]
        public IEnumerable<ExchangeTicketDetail> Detail()
        {
            return _dataAccess.GetTicketDetailList();
        }
    }
}