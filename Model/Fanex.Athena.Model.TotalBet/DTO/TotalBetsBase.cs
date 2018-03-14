// -----------------------------------------------------------------------
// <copyright file="TotalBetsBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    using Model.Core.Helper;
    using VendorAdapter;

    /// <summary>
    /// TotalBetsBase class.
    /// </summary>
    public class TotalBetsBase
    {
        private string _leagueName;
        private string _awayTeamName;

        private string _homeTeamName;

        public string ShowTime { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get { return _leagueName ?? (_leagueName = CacheRefAdapter.Instance.GetLeagueName(LeagueId)); }
        }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int IsNeutral { get; set; }

        public int AwayId { get; set; }

        public string HomeTeamName
        {
            get
            {
                return _homeTeamName ??
                       (_homeTeamName = CacheRefAdapter.Instance.GetTeamName(HomeId) + (IsNeutral == 1 ? " (N)" : string.Empty));
            }
        }

        public string AwayTeamName
        {
            get { return _awayTeamName ?? (_awayTeamName = CacheRefAdapter.Instance.GetTeamName(AwayId)); }
        }

        public int LiveIndicator { get; set; }

        public string LiveHomeScore { get; set; }

        public string LiveAwayScore { get; set; }
    }
}