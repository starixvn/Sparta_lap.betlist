// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;

    /// <summary>
    /// LiveCasinoBetSettingViewModel class.
    /// </summary>
    public class LiveCasinoBetSettingViewModel : BetSettingViewModel
    {
        public IList<LiveCasinoBetSettingGame> LiveCasinoData { get; set; }
    }
}