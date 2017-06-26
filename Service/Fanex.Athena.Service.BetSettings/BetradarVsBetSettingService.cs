namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.BetSettings;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.ViewModel;

    public class BetradarVsBetSettingService : BetSettingBaseService, IBetradarVsBetSettingService
    {
        private readonly IBetradarVsBetSettingDataAccess _betSettingDataAccess;

        public BetradarVsBetSettingService()
        {
            _betSettingDataAccess = new BetradarVsBetSettingDataAccess();
        }

        public BetSettingViewModel GetViewModel(int licenseeId, int custId)
        {
            var betSettings = HasPermissionOnData(licenseeId, custId)
                        ? _betSettingDataAccess.GetBetSetting(custId)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }
    }
}