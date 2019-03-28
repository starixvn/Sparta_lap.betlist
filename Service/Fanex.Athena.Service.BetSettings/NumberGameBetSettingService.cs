// -----------------------------------------------------------------------
// <copyright file="NumberGameBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class NumberGameBetSettingService : BetSettingBaseService, INumberGameBetSettingService
    {
        private readonly INumberGameBetSettingDataAccess _betSettingDataAccess;

        public NumberGameBetSettingService(
            IAuthorizationService authorizationService,
            INumberGameBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
        }

        public async Task<BetSettingViewModel> GetViewModelAsync(int licenseeId, int custId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return new BetSettingViewModel
                {
                    MainData = new List<BetSetting>()
                };
            }

            var betSettings = await _betSettingDataAccess.GetBetSettingAsync(custId).ConfigureAwait(false);

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
        public async Task<IResultInfo> UpdateBetSettingAsync(CommonBetSetting setting, Hashtable data)
        {
            if (!HasPermissionOnData(setting.LicenseeId, setting.CustId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
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

            return await _betSettingDataAccess.UpdateBetSettingAsync(numberGameBetSetting);
        }
    }
}