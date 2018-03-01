namespace Fanex.Athena.Service.BetList.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.BetList.DTO;
    using Model.BetList.ViewModel;

    public interface IExchangeWinLossBetListService
    {
        ExchangeBetListSummaryViewModel GetBetListData(BetListMetaModel metaModel);

        IEnumerable<ExchangeTicketDetail> GetTicketDetailList();
    }
}