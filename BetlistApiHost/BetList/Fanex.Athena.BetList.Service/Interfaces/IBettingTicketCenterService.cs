using System.Collections.Generic;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;

namespace Fanex.Athena.BetList.Service.Interfaces
{
    public interface IBettingTicketCenterService
    {
        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        GroupTicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, int rowPerGroup, IDictionary<int, string> currencyList);

        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);

        int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm);

        IList<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel, bool isAllowedCustomerComm);

        string GetTitle(BaseBettingTicketMetaModel metaModel);
    }
}