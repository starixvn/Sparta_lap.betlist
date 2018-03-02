namespace Fanex.Athena.Service.BetList
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.BetList;
    using DataAccess.BetList.Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Service.BetList.Interfaces;

    public class ExchangeBetListService : IExchangeBetListService
    {
        private readonly IExchangeBetListDataAccess _dataAccess;

        public ExchangeBetListService()
        {
            _dataAccess = new ExchangeBetListDataAccess();
        }

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
                    m => new { m.UserId, m.EvenId, m.MarketId },
                    d => new { d.UserId, d.EvenId, d.MarketId },
                   (main, detailList) => new ExchangeBetList { Ticket = main, Details = detailList });

                return new ExchangeBetListViewModel
                {
                    MainData = mainData,
                    GrandTotal = betList.GrandTotal
                };
            }

            return new ExchangeBetListViewModel();
        }

        public IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListMetaModel metaModel)
        {
            return _dataAccess.GetTicketDetailList(metaModel);
        }
    }
}