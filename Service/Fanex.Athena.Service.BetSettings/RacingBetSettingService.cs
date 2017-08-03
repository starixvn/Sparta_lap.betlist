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
    using Core.Users.Interfaces;
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

        public RacingBetSettingService(
            IAuthorizationService authorizationService,
            IRacingBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
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

        public IDictionary<string, string> UpdateBetSetting(int licenseeId, IDictionary<string, RacingBetSetting> racingParam)
        {
            if (racingParam == null || racingParam.Count == 0)
            {
                throw new ServiceException("Invalid parameter. racingParam is null", "BetSetting");
            }

            var custId = racingParam.First().Value.CustId;
            if (!HasPermissionOnData(licenseeId, custId))
            {
                throw new ServiceException(ViewResources.AccessDeniedMsg, "BetSetting");
            }

            return _betSettingDataAccess.UpdateBetSetting(racingParam);
        }
    }
}