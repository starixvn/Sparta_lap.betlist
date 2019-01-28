namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public interface IBettingTicketCenterDataAccess
    {
        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        GroupTicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup, IDictionary<int, string> currencyList);

        int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListForSingleCustomer(
            SingleCustomersBetListMetaModel metaModel,
            IDictionary<int, string> currencyList);

        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}