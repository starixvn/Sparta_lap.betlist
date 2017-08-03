// -----------------------------------------------------------------------
// <copyright file="SportsbookBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    #region

    using System.Collections.Generic;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.BetSettings;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.BetSettings.Interfaces;
    using Resources;

    #endregion

    public class SportsbookBetSettingService : BetSettingBaseService, ISportsbookBetSettingService
    {
        private readonly ISportsbookBetSettingDataAccess _betSettingDataAccess;

        public SportsbookBetSettingService()
        {
            _betSettingDataAccess = new SportsbookBetSettingDataAccess();
        }

        public SportsbookBetSettingService(
            IAuthorizationService authorizationService,
            ISportsbookBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
        }

        public BetSettingViewModel GetViewModel(int licenseeId, int custId)
        {
            var esportSetting = EsportSettingType.NoneOffering;
            var betSettings = HasPermissionOnData(licenseeId, custId)
                        ? _betSettingDataAccess.GetBetSetting(custId, ref esportSetting)
                        : new List<BetSetting>();

            return new BetSettingViewModel
            {
                MainData = betSettings,
                EsportSetting = esportSetting
            };
        }

        public IResultInfo UpdateBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. sportsBook is null");
            }

            if (!HasPermissionOnData(setting.LicenseeId, setting.CustId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            return _betSettingDataAccess.UpdateBetSetting(setting, hashParam);
        }

        public IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. sportsBook is null");
            }

            return _betSettingDataAccess.UpdateEsportStatus(setting, isDisabled);
        }
    }
}