// -----------------------------------------------------------------------
// <copyright file="BetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.MetaModel.BetSettings;

    /// <summary>
    /// BetSettingViewModel class.
    /// </summary>
    public class BetSettingViewModel
    {
        public BetSettingViewModel()
        {
            MainData = new List<BetSetting>();
        }

        public BetSettingMetaModel Param { get; set; }

        public IList<BetSetting> MainData { get; set; }

        public bool? IsEnabled { get; set; }

        public EsportSettingType EsportSetting { get; set; }
    }
}