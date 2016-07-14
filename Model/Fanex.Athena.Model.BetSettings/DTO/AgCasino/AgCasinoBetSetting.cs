// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO.AgCasino
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