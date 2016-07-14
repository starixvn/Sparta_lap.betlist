// -----------------------------------------------------------------------
// <copyright file="Outright.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    using Model.Core.Helper;

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