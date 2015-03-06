// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.AgCasino;

    /// <summary>
    /// GoldDeluxeBetSettingViewModel class.
    /// </summary>
    public class GoldDeluxeBetSettingViewModel
    {
        public GoldDeluxeBetSetting BetSetting { get; set; }

        public IList<GoldDeluxeGame> Games { get; set; }
    }
}