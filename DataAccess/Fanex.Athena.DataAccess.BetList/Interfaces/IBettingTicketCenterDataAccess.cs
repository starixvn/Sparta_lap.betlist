namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public interface IBettingTicketCenterDataAccess
    {
        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel);
    }
}