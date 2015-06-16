// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingGame.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;

    /// <summary>
    /// LiveCasinoBetSettingGame class.
    /// </summary>
    public class LiveCasinoBetSettingGame
    {
        public string GameTypeName { get; set; }

        public int GameTypeId { get; set; }

        public List<LiveCasinoBetSetting> BettingLimitList { get; set; }

        public string StakeLevelNameChecked { get; set; }

        public int BetTypeId { get; set; }
    }
}