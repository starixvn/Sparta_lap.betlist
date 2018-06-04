namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class RefNoBetListMetaModel : BaseBettingTicketMetaModel
    {
        public string RefNo { get; set; }

        public string UserName { get; set; }

        public override bool IsValid => !string.IsNullOrWhiteSpace(RefNo);
    }
}