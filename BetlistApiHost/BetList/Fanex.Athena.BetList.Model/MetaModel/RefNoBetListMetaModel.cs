namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class RefNoBetListMetaModel : BaseBettingTicketMetaModel
    {
        public long RefNo { get; set; }
        public string TxId { get; set; }
        public override bool IsValid => RefNo > 0 || !string.IsNullOrEmpty(TxId);
    }
}