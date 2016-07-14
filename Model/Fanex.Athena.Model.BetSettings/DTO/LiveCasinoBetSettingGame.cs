// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingGame.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;

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