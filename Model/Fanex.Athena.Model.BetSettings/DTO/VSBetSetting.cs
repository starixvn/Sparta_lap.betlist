// -----------------------------------------------------------------------
// <copyright file="VSBetSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VSBetSetting
    {
        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public string SportId { get; set; }
    }
}