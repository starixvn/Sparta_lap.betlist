namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using System.Collections.Generic;

    public interface IBettingTicketCenterDataAccess
    {
        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}