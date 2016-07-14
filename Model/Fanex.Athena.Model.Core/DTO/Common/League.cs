// -----------------------------------------------------------------------
// <copyright file="League.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using Fanex.Athena.Model.Core.Helper;

    /// <summary>
    /// League class.
    /// </summary>
    public class League
    {
        private string _leagueName;

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                if (string.IsNullOrEmpty(_leagueName))
                {
                    _leagueName = CacheRef.GetLeagueName(LeagueId);
                }

                return _leagueName;
            }

            set
            {
                _leagueName = value;
            }
        }

        public bool IsOutRight { get; set; }
    }
}