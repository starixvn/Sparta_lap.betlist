// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO.RngCasino;

    /// <summary>
    /// RNG Casino BetSetting View Model class.
    /// </summary>
    public class RngCasinoBetSettingViewModel : BetSettingViewModel
    {
        public IList<GameKind> GameKinds { get; set; }
    }
}