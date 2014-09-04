// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.BetSetting.AgCasino
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgCasinoBetSetting
    {
        public int CurrencyId { get; set; }

        public string AgGroup { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLineEnabled { get; set; }

        public string[] CheckedGroups { get; set; }
    }
}