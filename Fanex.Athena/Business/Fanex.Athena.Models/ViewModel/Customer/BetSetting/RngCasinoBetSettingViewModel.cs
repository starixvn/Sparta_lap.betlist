// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.RngCasino;

    /// <summary>
    /// RNG Casino BetSetting View Model class.
    /// </summary>
    public class RngCasinoBetSettingViewModel : BetSettingViewModel
    {
        public IList<GameKind> GameKinds { get; set; }
    }
}