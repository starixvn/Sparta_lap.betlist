using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;

namespace Fanex.Athena.BetList.Service.Interfaces
{
    public interface IBettingTicketCenterService
    {
        Task<TicketResult> GetBetListByRefnoAsync(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<GroupTicketResult> GetBetListForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup, IDictionary<int, string> currencyList);

        Task<TicketResult> GetBetListForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<TicketResult> GetBetListForSingleCustomerAsync(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<TicketResult> GetRunningBetListAsync(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<int> GetTotalRowForMultipleCustomersAsync(MultipleCustomersBetListMetaModel metaModel);

        string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm);

        string GetTitle(BaseBettingTicketMetaModel metaModel);
    }
}