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
    public class LiveCasinoBetSettingViewModel : BetSettingViewModel
    {
        public IList<LiveCasinoBetSettingGame> LiveCasinoData { get; set; }
    }
}