namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IExchangeBetListDataAccess
    {
        ExchangeBetListResult GetBetListData(ExchangeBetListMetaModel metaModel);

        IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListDetailMetaModel metaModel);
    }
}