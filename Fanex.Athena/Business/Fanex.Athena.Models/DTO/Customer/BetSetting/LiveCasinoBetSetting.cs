// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.BetSetting
{
    /// <summary>
    /// LiveCasinoBetSetting class.
    /// </summary>
    public class LiveCasinoBetSetting
    {
        public string GameType { get; set; }

        public int GameTypeId { get; set; }

        public int StakeLevelId { get; set; }

        public string StakeLevelName { get; set; }

        public int BetTypeId { get; set; }

        public bool IsCheck { get; set; }
    }
}