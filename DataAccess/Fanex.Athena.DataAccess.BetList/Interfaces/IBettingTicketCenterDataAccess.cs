namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public interface IBettingTicketCenterDataAccess
    {
        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);


        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}