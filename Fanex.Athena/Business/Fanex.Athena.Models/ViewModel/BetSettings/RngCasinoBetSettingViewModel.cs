// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings.RngCasino;

    /// <summary>
    /// RNG Casino BetSetting View Model class.
    /// </summary>
    public class RngCasinoBetSettingViewModel : BetSettingViewModel
    {
        public IList<GameKind> GameKinds { get; set; }
    }
}