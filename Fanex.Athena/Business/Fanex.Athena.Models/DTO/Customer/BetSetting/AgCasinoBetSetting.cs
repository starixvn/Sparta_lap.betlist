// -----------------------------------------------------------------------
// <copyright file="AgCasinoGroupBettingLimit.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.BetSetting
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgCasinoGroupBettingLimit
    {
        public int Currency { get; set; }

        public string AgGroup { get; set; }

        public bool IsEnabled { get; set; }
    }
}