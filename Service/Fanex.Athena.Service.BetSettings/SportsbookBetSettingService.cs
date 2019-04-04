// -----------------------------------------------------------------------
// <copyright file="SportsbookBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Constant.DataDomain;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.BetSettings.Interfaces;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Resources;

    public class SportsbookBetSettingService : BetSettingBaseService, ISportsbookBetSettingService
    {
        private readonly ISportsbookBetSettingDataAccess _betSettingDataAccess;

        private readonly IDcsBetSettingService _dcsBetSettingService;

        public SportsbookBetSettingService(
            IAuthorizationService authorizationService,
            ISportsbookBetSettingDataAccess betSettingDataAccess,
            IDcsBetSettingService dcsBetSettingService)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
            _dcsBetSettingService = dcsBetSettingService;
        }

        public async Task<BetSettingViewModel> GetViewModelAsync(int licenseeId, int custId)
        {
            const int EsportIndex = 2;
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return new BetSettingViewModel
                {
                    MainData = new List<BetSetting>()
                };
            }

            var viewModel = await _betSettingDataAccess.GetBetSettingAsync(custId).ConfigureAwait(false);

            // Esport must be inserted in this "Feng Shui" position.
            var betSetting = await GetESportsSettingAsync(licenseeId, custId).ConfigureAwait(false);
            viewModel.MainData.Insert(EsportIndex, betSetting);
            return viewModel;
        }

        public async Task<IResultInfo> UpdateBetSettingAsync(
            CommonBetSetting setting,
            IDictionary<string, object> hashParam,
            DcsBetSetting esportSetting)
        {
            if (setting == null || esportSetting == null)
            {
                return ErrorResult("Invalid parameters. bet setting is null");
            }

            if (!HasPermissionOnData(setting.LicenseeId, setting.CustId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            var result = await _betSettingDataAccess.UpdateBetSettingAsync(setting, hashParam);

            if (!result.IsSuccessful)
            {
                return result;
            }

            return await _dcsBetSettingService.UpdateBetSettingAsync(setting.LicenseeId, setting.CustId, setting.UserId, setting.Ip, new[] { esportSetting });
        }

        private async Task<BetSetting> GetESportsSettingAsync(int licenseeId, int custId)
        {
            var dataList = await _dcsBetSettingService.GetBetSettingsAsync(licenseeId, custId, ProductTypes.Sportsbooks2).ConfigureAwait(false);
            if (!dataList.Any())
            {
                throw new InvalidOperationException("There is no E-sport setting data.");
            }

            return dataList.First();
        }
    }
}