namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IBettingTicketCenterMultiCustomerDataAccess
    {
        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        GroupTicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup, IDictionary<int, string> currencyList);

        int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);
    }
}