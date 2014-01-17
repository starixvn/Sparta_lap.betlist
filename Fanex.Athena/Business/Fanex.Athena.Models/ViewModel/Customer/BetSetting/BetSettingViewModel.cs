// -----------------------------------------------------------------------
// <copyright file="BetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.RngCasino;
    using Fanex.Athena.Models.MetaModel.Customer.BetSetting;

    /// <summary>
    /// BetSettingViewModel class.
    /// </summary>
    public class BetSettingViewModel
    {
        public BetSettingMetaModel Param { get; set; }

        public IList<BetSetting> MainData { get; set; }

        public IList<LiveCasinoBetSettingViewModel> LiveCasinoData { get; set; }

        public RngCasinoBetSetting RngCasinoData { get; set; }

        public bool? IsEnabled { get; set; }
    }
}