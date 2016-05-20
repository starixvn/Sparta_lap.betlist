// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO.AgCasino;

    /// <summary>
    /// AgCasinoBetSettingViewModel class.
    /// </summary>
    public class AgCasinoBetSettingViewModel : BetSettingViewModel
    {
        public AgCasinoBetSetting BetSetting { get; set; }

        public IList<AgCasinoGame> Games { get; set; }
    }
}