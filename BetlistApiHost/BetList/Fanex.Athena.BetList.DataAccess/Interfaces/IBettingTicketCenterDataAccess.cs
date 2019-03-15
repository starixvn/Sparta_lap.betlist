namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IBettingTicketCenterDataAccess
    {
        TicketResult GetBetListForSingleCustomer(
            SingleCustomersBetListMetaModel metaModel,
            IDictionary<int, string> currencyList);

        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}