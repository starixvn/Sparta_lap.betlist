// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;

    /// <summary>
    /// LiveCasinoBetSettingViewModel class.
    /// </summary>
    public class LiveCasinoBetSettingViewModel : BetSettingViewModel
    {
        public IList<LiveCasinoBetSettingGame> LiveCasinoData { get; set; }
    }
}