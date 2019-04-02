namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;

    public interface IBettingTicketCenterDataAccess
    {
        Task<TicketResult> GetBetListForSingleCustomer(
            SingleCustomersBetListMetaModel metaModel,
            IDictionary<int, string> currencyList);

        Task<TicketResult> GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<TicketResult> GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}