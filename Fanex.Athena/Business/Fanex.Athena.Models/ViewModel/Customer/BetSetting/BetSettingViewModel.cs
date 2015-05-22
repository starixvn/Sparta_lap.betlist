// -----------------------------------------------------------------------
// <copyright file="BetSettingViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Customer.BetSetting
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.RngCasino;
    using Fanex.Athena.Models.DTO.Customer.BetSetting.VegasGames;
    using Fanex.Athena.Models.MetaModel.Customer.BetSetting;

    /// <summary>
    /// BetSettingViewModel class.
    /// </summary>
    public class BetSettingViewModel
    {
        private List<BetSetting> _mainData;
        private List<LiveCasinoBetSettingViewModel> _liveCasinoData;
        private AgCasinoBetSettingViewModel _agcasinoViewModel;
        private GoldDeluxeBetSettingViewModel _goldDeluxeViewModel;

        public BetSettingViewModel()
        {
            EsportSetting = EsportSettingType.NoneOffering;
            _mainData = new List<BetSetting>();
            _liveCasinoData = new List<LiveCasinoBetSettingViewModel>();
            _agcasinoViewModel = new AgCasinoBetSettingViewModel();
            _goldDeluxeViewModel = new GoldDeluxeBetSettingViewModel();
        }

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

        public GoldDeluxeBetSettingViewModel GoldDeluxeViewModel
        {
            get
            {
                return _goldDeluxeViewModel;
            }
        }

        public VegasGamesBetSetting VegasGamesViewModel { get; set; }

        public RngCasinoBetSetting RngCasinoData { get; set; }

        public bool? IsEnabled { get; set; }

        public EsportSettingType EsportSetting { get; set; }
    }
}