// -----------------------------------------------------------------------
// <copyright file="BetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;

    public class BetSettingViewModel : BetSettingBaseViewModel
    {
        public BetSettingViewModel()
        {
            MainData = new List<BetSetting>();
        }

        public IList<BetSetting> MainData { get; set; }

        public EsportSettingType EsportSetting { get; set; }
    }
}