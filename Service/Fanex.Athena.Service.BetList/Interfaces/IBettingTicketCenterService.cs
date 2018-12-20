using System.Collections.Generic;
using Fanex.Athena.Model.BetList.DTO;
using Fanex.Athena.Model.BetList.MetaModel;

namespace Fanex.Athena.Service.BetList.Interfaces
{
    public interface IBettingTicketCenterService
    {
        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel);

        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel);

        int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        bool IsValidCurrencyId(int currencyId);

        string BuildSumWinLoss(TicketResult ticketResult);

        IList<TicketText> BuildTicketText(TicketResult ticketResult, BaseBettingTicketMetaModel metaModel);

        string GetTitle(BaseBettingTicketMetaModel metaModel);
    }
}