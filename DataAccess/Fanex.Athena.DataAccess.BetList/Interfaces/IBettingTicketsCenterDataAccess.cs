namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public interface IBettingTicketsCenterDataAccess
    {
        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListForSingleCustomer();

        TicketResult GetBetListByRefno();

        TicketResult GetRunningBetList();
    }
}