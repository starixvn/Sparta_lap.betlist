// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO.GoldDeluxe
{
    public class GoldDeluxeBetSetting
    {
        public int CurrencyId { get; set; }

        public string Group { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLineEnabled { get; set; }

        public string[] CheckedGroups { get; set; }
    }
}