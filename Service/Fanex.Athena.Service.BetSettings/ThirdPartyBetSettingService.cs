// -----------------------------------------------------------------------
// <copyright file="ThirdPartyBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Threading.Tasks;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class ThirdPartyBetSettingService : BetSettingBaseService, IThirdPartyBetSettingService
    {
        private readonly IThirdPartyBetSettingDataAccess _betSettingDataAccess;

        public ThirdPartyBetSettingService(
            IAuthorizationService authorizationService,
            IThirdPartyBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
        }

        public async Task<ThirdPartyBetSettingViewModel> GetViewModelAsync(int licenseeId, int custId, int productid)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return null;
            }

            var setting = await _betSettingDataAccess.GetBetSettingAsync(custId, productid).ConfigureAwait(false);

            return new ThirdPartyBetSettingViewModel
            {
                IsLineEnabled = setting.IsLineEnabled,
                IsEnabled = setting.IsEnabled
            };
        }

        public async Task<IResultInfo> UpdateStatusAsync(int licenseeId, int productId, int userId, int custId, string language, string ip, bool isEnabled)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            var setting = await _betSettingDataAccess.GetBetSettingAsync(custId, productId).ConfigureAwait(false);
            if (!setting.IsLineEnabled)
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            return await _betSettingDataAccess.UpdateStatusAsync(productId, userId, custId, language, ip, isEnabled);
        }
    }
}