namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public interface IExchangeBetListDataAccess
    {
        ExchangeBetListResult GetBetListData(ExchangeBetListMetaModel metaModel);

        IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListMetaModel metaModel);
    }
}