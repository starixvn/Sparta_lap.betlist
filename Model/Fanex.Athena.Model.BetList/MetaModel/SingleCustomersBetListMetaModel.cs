namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class SingleCustomersBetListMetaModel : BaseBettingTicketMetaModel
    {
        public int Platform { get; set; }

        public string LiveStatus { get; set; }

        public override bool IsValid => base.IsValid && !string.IsNullOrWhiteSpace(UserName);
    }
}