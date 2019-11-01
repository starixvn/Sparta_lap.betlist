namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IBettingTicketCenterMultiCustomerDataAccess
    {
        Task<TicketResult> GetBetListForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel);

        Task<GroupTicketResult> GetBetListForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup);

        Task<int> GetTotalRowForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel);
    }
}