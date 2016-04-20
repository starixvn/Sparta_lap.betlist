// -----------------------------------------------------------------------
// <copyright file="Match.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using Fanex.Athena.Models.Cache;

    /// <summary>
    /// Match class.
    /// </summary>
    public class Match
    {
        private string _matchName;

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string MatchName
        {
            get
            {
                if (_matchName == null)
                {
                    _matchName = CacheRef.GetTeamName(HomeId) + (IsNeutral ? " (N)" : string.Empty)
                                   + " -vs- "
                               + CacheRef.GetTeamName(AwayId);
                }

                return _matchName;
            }

            set
            {
                _matchName = value;
            }
        }

        public bool IsOutRight { get; set; }

        public bool IsNeutral { get; set; }
    }
}