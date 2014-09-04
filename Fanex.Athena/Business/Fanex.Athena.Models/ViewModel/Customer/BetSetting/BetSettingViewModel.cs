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
        private List<BetSetting> _mainData = new List<BetSetting>();
        private List<LiveCasinoBetSettingViewModel> _liveCasinoData = new List<LiveCasinoBetSettingViewModel>();
        private AgCasinoBetSettingViewModel _agcasinoViewModel = new AgCasinoBetSettingViewModel();

        public BetSettingMetaModel Param { get; set; }

        public List<BetSetting> MainData
        {
            get
            {
                return _mainData;
            }
        }

        public List<LiveCasinoBetSettingViewModel> LiveCasinoData
        {
            get
            {
                return _liveCasinoData;
            }
        }

        public AgCasinoBetSettingViewModel AgCasinoViewModel
        {
            get
            {
                return _agcasinoViewModel;
            }
        }

        public RngCasinoBetSetting RngCasinoData { get; set; }

        public bool? IsEnabled { get; set; }
    }
}