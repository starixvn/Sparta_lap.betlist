namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IBettingTicketCenterMultiCustomerDataAccess
    {
        Task<TicketResult> GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<GroupTicketResult> GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup, IDictionary<int, string> currencyList);

        Task<int> GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);
    }
}