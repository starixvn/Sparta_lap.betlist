// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.AgCasino;

    /// <summary>
    /// AgCasinoBetSettingViewModel class.
    /// </summary>
    public class AgCasinoBetSettingViewModel
    {
        public AgCasinoBetSetting BetSetting { get; set; }

        public IList<AgCasinoGame> Games { get; set; }
    }
}