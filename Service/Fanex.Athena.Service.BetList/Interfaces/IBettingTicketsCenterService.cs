namespace Fanex.Athena.Service.BetList.Interfaces
{
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;

    public interface IBettingTicketsCenterService
    {
        TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel);

        TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel);

        TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel);
    }
}