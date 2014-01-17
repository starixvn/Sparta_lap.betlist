// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;

    /// <summary>
    /// LiveCasinoBetSettingViewModel class.
    /// </summary>
    public class LiveCasinoBetSettingViewModel
    {
        public string GameTypeName { get; set; }

        public int GameTypeId { get; set; }

        public List<LiveCasinoBetSetting> BettingLimitList { get; set; }

        public string StakeLevelNameChecked { get; set; }

        public int BetTypeId { get; set; }
    }
}