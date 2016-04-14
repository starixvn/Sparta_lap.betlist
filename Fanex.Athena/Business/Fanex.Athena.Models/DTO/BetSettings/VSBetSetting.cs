// -----------------------------------------------------------------------
// <copyright file="VSBetSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetSettings
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