// -----------------------------------------------------------------------
// <copyright file="RacingBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Exception;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.ViewModel;
    using Resources;

    public class RacingBetSettingService : BetSettingBaseService, IRacingBetSettingService
    {
        private readonly IRacingBetSettingDataAccess _betSettingDataAccess;

        public RacingBetSettingService()
        {
            _betSettingDataAccess = new RacingBetSettingDataAccess();
        }

        public BetSettingViewModel GetViewModel(int licenseeId, int custId, bool isAllowSaToreCurrency)
        {
            var betSettings = HasPermissionOnData(licenseeId, custId)
                        ? _betSettingDataAccess.GetBetSetting(custId, isAllowSaToreCurrency)
                        : new List<BetSetting>();
            return new BetSettingViewModel
            {
                MainData = betSettings
            };
        }

        public IDictionary<string, string> UpdateBetSetting(int licenseeId, Dictionary<string, RacingBetSetting> racingParam)
        {
            if (racingParam == null || racingParam.Count == 0)
            {
                throw new ServiceException("Invalid parameter. racingParam is null", "BetSetting");
            }

            var custId = racingParam.First().Value.CustId;
            if (!HasPermissionOnData(licenseeId, custId))
            {
                throw new ServiceException(ViewResources.UpdateSpecialLevelMsg, "BetSetting");
            }

            return _betSettingDataAccess.UpdateBetSetting(racingParam);
        }
    }
}