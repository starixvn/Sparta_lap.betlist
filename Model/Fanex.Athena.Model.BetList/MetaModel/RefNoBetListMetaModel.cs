namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;

    public class RefNoBetListMetaModel : BaseBettingTicketMetaModel
    {
        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override bool IsValid => RefNo > 0;
    }
}