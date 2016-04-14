// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings.AgCasino;

    /// <summary>
    /// AgCasinoBetSettingViewModel class.
    /// </summary>
    public class AgCasinoBetSettingViewModel : BetSettingViewModel
    {
        public AgCasinoBetSetting BetSetting { get; set; }

        public IList<AgCasinoGame> Games { get; set; }
    }
}