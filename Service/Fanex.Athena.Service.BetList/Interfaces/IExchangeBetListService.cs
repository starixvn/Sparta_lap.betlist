namespace Fanex.Athena.Service.BetList.Interfaces
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;

    public interface IExchangeBetListService
    {
        ExchangeBetListViewModel GetBetListData(ExchangeBetListMetaModel metaModel);

        IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListDetailMetaModel metaModel);
    }
}