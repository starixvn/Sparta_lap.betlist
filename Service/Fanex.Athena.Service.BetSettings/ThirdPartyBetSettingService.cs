// -----------------------------------------------------------------------
// <copyright file="ThirdPartyBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using BetSettings.Interfaces;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Resources;

    public class ThirdPartyBetSettingService : BetSettingBaseService, IThirdPartyBetSettingService
    {
        private readonly IThirdPartyBetSettingDataAccess _betSettingDataAccess;

        public ThirdPartyBetSettingService()
        {
            _betSettingDataAccess = new ThirdPartyBetSettingDataAccess();
        }

        public ThirdPartyBetSettingViewModel GetViewModel(int custId, int productid)
        {
            if (!HasPermissionOnData(custId))
            {
                return null;
            }

            var setting = _betSettingDataAccess.GetBetSetting(custId, productid);

            return new ThirdPartyBetSettingViewModel
            {
                IsLineEnabled = setting.IsLineEnabled,
                IsEnabled = setting.IsEnabled
            };
        }

        public IResultInfo UpdateStatus(int productId, int userId, int custId, string language, string ip, bool isEnabled)
        {
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var setting = _betSettingDataAccess.GetBetSetting(custId, productId);
            if (!setting.IsLineEnabled)
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            return _betSettingDataAccess.UpdateStatus(productId, userId, custId, language, ip, isEnabled);
        }
    }
}