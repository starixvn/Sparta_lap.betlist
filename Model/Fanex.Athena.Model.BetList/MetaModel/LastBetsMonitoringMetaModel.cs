// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class LastBetsMonitoringMetaModel
    {
        public LastBetsMonitoringMetaModel()
        {
            PageSize = 10;
        }

        public int SportId { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public string BetType { get; set; }

        public string Level { get; set; }

        public int LiveStatus { get; set; }

        public string OddsType { get; set; }

        public int TimeZone { get; set; }

        public string Currency { get; set; }

        public string BasedCurrency { get; set; }

        public int StakeFrom { get; set; }

        public int StakeTo { get; set; }

        public int PageSize { get; set; }

        public long TransId { get; set; }

        public int BetFrom { get; set; }

        public string AgentListXml { get; set; }
    }
}