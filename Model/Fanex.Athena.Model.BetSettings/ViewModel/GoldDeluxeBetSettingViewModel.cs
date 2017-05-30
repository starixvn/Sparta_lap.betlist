// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSettingViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;
    using DTO.GoldDeluxe;
    using Fanex.Athena.Model.BetSettings.DTO.AgCasino;

    public class GoldDeluxeBetSettingViewModel : BetSettingViewModel
    {
        public GoldDeluxeBetSetting BetSetting { get; set; }

        public IList<GoldDeluxeGame> Games { get; set; }
    }
}