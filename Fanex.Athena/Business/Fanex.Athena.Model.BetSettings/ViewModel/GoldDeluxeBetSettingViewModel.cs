// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO.AgCasino;

    /// <summary>
    /// GoldDeluxeBetSettingViewModel class.
    /// </summary>
    public class GoldDeluxeBetSettingViewModel : BetSettingViewModel
    {
        public GoldDeluxeBetSetting BetSetting { get; set; }

        public IList<GoldDeluxeGame> Games { get; set; }
    }
}