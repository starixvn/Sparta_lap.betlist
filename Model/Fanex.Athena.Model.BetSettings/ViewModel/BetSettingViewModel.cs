// -----------------------------------------------------------------------
// <copyright file="BetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DataDomain;

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