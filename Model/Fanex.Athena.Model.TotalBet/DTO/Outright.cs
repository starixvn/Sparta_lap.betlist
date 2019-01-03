// -----------------------------------------------------------------------
// <copyright file="Outright.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    public class Outright : TotalBetsBase
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public decimal Balance { get; set; }

        public decimal Forecast { get; set; }
    }
}