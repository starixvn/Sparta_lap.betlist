namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class SingleCustomersBetListMetaModel : BaseBettingTicketMetaModel
    {
        public string Status { get; set; }

        public int Platform { get; set; }

        public int LiveStatus { get; set; }

        public override bool IsValid => base.IsValid && !string.IsNullOrWhiteSpace(UserName);
    }
}