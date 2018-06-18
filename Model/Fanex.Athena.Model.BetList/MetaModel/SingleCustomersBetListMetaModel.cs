namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class SingleCustomersBetListMetaModel : BaseBettingTicketMetaModel
    {
        public int Status { get; set; }

        public int Platform { get; set; }

        public int LiveStatus { get; set; }

        public override bool IsValid => base.IsValid && CustId > 0;
    }
}