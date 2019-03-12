namespace Fanex.Athena.BetList.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Interfaces;
    using Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using Model.ViewModel;

    public class ExchangeBetListService : IExchangeBetListService
    {
        private readonly IExchangeBetListDataAccess _dataAccess;

        public ExchangeBetListService(IExchangeBetListDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public ExchangeBetListViewModel GetBetListData(ExchangeBetListMetaModel metaModel)
        {
            var betList = _dataAccess.GetBetListData(metaModel);

            if (betList.SumWinLossList.Any() && betList.TicketList.Any())
            {
                var mainData = betList.SumWinLossList.GroupJoin(
                    betList.TicketList,
                    m => new { m.UserId, m.EventId, m.MarketId },
                    d => new { d.UserId, d.EventId, d.MarketId },
                   (main, detailList) => new ExchangeBetList { Ticket = main, Details = detailList }).Where(x => x.Details.Any());

                return new ExchangeBetListViewModel
                {
                    MainData = mainData,
                    GrandTotal = betList.GrandTotal
                };
            }

            return new ExchangeBetListViewModel();
        }

        public IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListDetailMetaModel metaModel)
        {
            return _dataAccess.GetTicketDetailList(metaModel);
        }
    }
}