using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;

namespace Fanex.Athena.BetList.Service.Interfaces
{
    public interface IBettingTicketCenterService
    {
        Task<TicketResult> GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<GroupTicketResult> GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup, IDictionary<int, string> currencyList);

        Task<TicketResult> GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<TicketResult> GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<TicketResult> GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<int> GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm);

        IList<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel, bool isAllowedCustomerComm);

        string GetTitle(BaseBettingTicketMetaModel metaModel);
    }
}