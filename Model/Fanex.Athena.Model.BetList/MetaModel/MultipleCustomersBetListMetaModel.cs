namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class MultipleCustomersBetListMetaModel : BaseBettingTicketMetaModel
    {
        public int LiveStatus { get; set; }

        public int Status { get; set; }

        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public int Platform { get; set; }
    }
}