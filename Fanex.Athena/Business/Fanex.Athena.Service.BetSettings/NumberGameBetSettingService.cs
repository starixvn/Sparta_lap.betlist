// -----------------------------------------------------------------------
// <copyright file="NumberGameBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections;
    using System.Collections.Generic;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Models.DTO.BetSettings;
    using Models.DTO.Common;
    using Models.ViewModel;
    using Models.ViewModel.BetSettings;

    public class NumberGameBetSettingService : BetSettingBaseService, INumberGameBetSettingService
    {
        private readonly INumberGameBetSettingDataAccess _numberBetSettingDataAccess;

        public NumberGameBetSettingService()
        {
            _numberBetSettingDataAccess = new NumberGameBetSettingDataAccess();
        }

        public BetSettingViewModel GetViewModel(int custId)
        {
            var betSettings = HasPermissionOnData(custId)
                        ? _numberBetSettingDataAccess.GetBetSetting(custId)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        /// <summary>
        /// Update NumberGame bet setting.
        /// </summary>
        /// <param name="setting">The metal model.</param>
        /// <param name="data">The parameter.</param>
        /// <returns>The information of result.</returns>
        public IResultInfo UpdateBetSetting(CommonBetSetting setting, Hashtable data)
        {
            if (!HasPermissionOnData(setting.CustId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var numberGameBetSetting = new NumberGameBetSetting
            {
                CustId = setting.CustId,
                UserId = setting.UserId,
                Language = setting.Language,
                Ip = setting.Ip,
                MinBet = (decimal)data["minbet161"],
                MaxBet = (decimal)data["maxbet161"],
                MaxPerMatch = (decimal)data["maxpermatch161"],
                IsDataChange =
                                                !string.IsNullOrEmpty(data["minbet161"].ToString()) &&
                                                !string.IsNullOrEmpty(data["maxbet161"].ToString()) &&
                                                !string.IsNullOrEmpty(data["maxpermatch161"].ToString()) &&
                                                !string.IsNullOrEmpty(data["maxperball"].ToString()),
                MaxPerBall = (decimal)data["maxperball"]
            };

            return _numberBetSettingDataAccess.UpdateBetSetting(numberGameBetSetting);
        }
    }
}