// -----------------------------------------------------------------------
// <copyright file="TotalBetsBase.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
{
    using Cache;

    /// <summary>
    /// TotalBetsBase class.
    /// </summary>
    public class TotalBetsBase
    {
        private int _leagueId;
        private string _leagueName;
        private string _awayTeamName;

        private string _homeTeamName;

        public string ShowTime { get; set; }

        public int LeagueId
        {
            get
            {
                return _leagueId;
            }

            set
            {
                _leagueId = value;
            }
        }

        public string LeagueName
        {
            get { return _leagueName ?? (_leagueName = CacheRef.GetLeagueName(_leagueId)); }
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
                       (_homeTeamName = CacheRef.GetTeamName(HomeId) + (IsNeutral == 1 ? " (N)" : string.Empty));
            }
        }

        public string AwayTeamName
        {
            get { return _awayTeamName ?? (_awayTeamName = CacheRef.GetTeamName(AwayId)); }
        }

        public int LiveIndicator { get; set; }

        public string LiveHomeScore { get; set; }

        public string LiveAwayScore { get; set; }
    }
}