namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System.Collections.Generic;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;
    using BetList.Model.ViewModel;

    public interface IExchangeBetListService
    {
        ExchangeBetListViewModel GetBetListData(ExchangeBetListMetaModel metaModel);

        IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListDetailMetaModel metaModel);
    }
}