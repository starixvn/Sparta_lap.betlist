namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class RefNoBetListMetaModel : BaseBettingTicketMetaModel
    {
        public long RefNo { get; set; }

        public override bool IsValid => RefNo > 0;
    }
}