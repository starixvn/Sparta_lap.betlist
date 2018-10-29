// -----------------------------------------------------------------------
// <copyright file="SportsbookBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Linq;
    using Constant.DataDomain;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.BetSettings.Interfaces;
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

        public BetSettingViewModel GetViewModel(int licenseeId, int custId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return new BetSettingViewModel
                {
                    MainData = new List<BetSetting>(),
                    EsportSetting = EsportSettingType.NoneOffering
                };
            }


            var viewModel = _betSettingDataAccess.GetBetSetting(custId);
            viewModel.MainData.Add(GetESportsSetting(licenseeId, custId));
            return viewModel;
        }

        public IResultInfo UpdateBetSetting(
            CommonBetSetting setting, 
            IDictionary<string, object> hashParam,
            DcsBetSetting esportSetting)
        {
            if (setting == null)
            {
                return ErrorResult("Invalid parameter. sportsBook is null");
            }

            if (!HasPermissionOnData(setting.LicenseeId, setting.CustId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            var result =_betSettingDataAccess.UpdateBetSetting(setting, hashParam);
            if (result.IsSuccessful)
            {
                return _dcsBetSettingService.UpdateBetSetting(setting.LicenseeId, setting.CustId, setting.UserId, setting.Ip, new[] { esportSetting });
            }

            return result;
        }


        private BetSetting GetESportsSetting(int licenseeId, int custId)
        {
            var dataList = _dcsBetSettingService.GetBetSettings(licenseeId, custId, ProductTypes.Sportsbooks2);
            return dataList.FirstOrDefault();
        }
    }
}