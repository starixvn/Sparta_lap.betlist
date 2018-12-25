using System.Collections.Generic;
using Fanex.Athena.Model.BetList.DTO;
using Fanex.Athena.Model.BetList.MetaModel;

namespace Fanex.Athena.Service.BetList.Interfaces
{
    public interface IBettingTicketCenterService
    {
        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList);

        int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        bool IsValidCurrencyId(int currencyId, IDictionary<int, string> currencyList);

        string BuildSumWinLoss(TicketResult ticketResult, bool isAllowedCustomerComm);

        IList<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel, bool isAllowedCustomerComm);

        string GetTitle(BaseBettingTicketMetaModel metaModel);
    }
}