namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;

    public interface IExchangeBetListDataAccess
    {
        ExchangeBetListSummaryViewModel GetBetListData(ExchangeBetListMetaModel metaModel);

        IEnumerable<ExchangeTicketDetail> GetTicketDetailList();
    }
}