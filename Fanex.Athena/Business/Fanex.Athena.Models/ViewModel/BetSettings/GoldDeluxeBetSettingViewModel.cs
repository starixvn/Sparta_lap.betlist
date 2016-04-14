// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings.AgCasino;

    /// <summary>
    /// GoldDeluxeBetSettingViewModel class.
    /// </summary>
    public class GoldDeluxeBetSettingViewModel : BetSettingViewModel
    {
        public GoldDeluxeBetSetting BetSetting { get; set; }

        public IList<GoldDeluxeGame> Games { get; set; }
    }
}