// -----------------------------------------------------------------------
// <copyright file="Outright.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
{
    using Cache;

    /// <summary>
    /// Outright class.
    /// </summary>
    public class Outright : TotalBetsBase
    {
        private string _teamName;

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return _teamName ?? (_teamName = CacheRef.GetTeamName(TeamId));
            }
        }

        public decimal Balance { get; set; }

        public decimal Forecast { get; set; }
    }
}