namespace Fanex.Athena.BetList.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

        public async Task<ExchangeBetListViewModel> GetBetListDataAsync(ExchangeBetListMetaModel metaModel)
        {
            var betList = await _dataAccess.GetBetListDataAsync(metaModel).ConfigureAwait(false);

            if (betList.SumWinLossList.Any() && betList.TicketList.Any())
            {
                var mainData = betList.SumWinLossList.GroupJoin(
                    betList.TicketList,
                    m => new { m.UserId, m.EventId, m.MarketId },
                    d => new { d.UserId, d.EventId, d.MarketId },
                   (main, detailList) => new ExchangeBetList { Ticket = main, Details = detailList.OrderBy(d => d.BetId) })
                        .Where(x => x.Details.Any())
                        .OrderBy(x => x.Ticket.MarketId);

                return new ExchangeBetListViewModel
                {
                    MainData = mainData,
                    GrandTotal = betList.GrandTotal
                };
            }

            return new ExchangeBetListViewModel();
        }

        public async Task<IEnumerable<ExchangeTicketDetail>> GetTicketDetailListAsync(ExchangeBetListDetailMetaModel metaModel)
        {
            return await _dataAccess.GetTicketDetailListAsync(metaModel).ConfigureAwait(false);
        }

        public Task<ExchangeResult<ExchangeRunningTicket>> GetRunningTicketsAsync(ExchangeRunningTicketMetaModel metaModel)
            => _dataAccess.GetRunningTicketsAsync(metaModel);
    }
}