// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using Casino.Interfaces;
    using CasinoLibrary.SBAgent;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Service.Casino;
    using Interfaces;
    using Model.BetSettings.DTO.RngCasino;
    using Model.BetSettings.ViewModel;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using Resources;

    public class RngCasinoBetSettingService : BetSettingBaseService, IRngCasinoBetSettingService
    {
        private readonly IRngCasinoBetSettingDataAccess _betSettingDataAccess;

        private readonly ICasinoSetting _casinoSetting;

        private readonly Customer _customerApi;

        private readonly IAppSettings _appSettings;

        public RngCasinoBetSettingService()
        {
            _betSettingDataAccess = new RngCasinoBetSettingDataAccess();
            _casinoSetting = new CasinoSetting();
            _customerApi = new Customer();
            _appSettings = new AppSettings();
        }

        public RngCasinoBetSettingService(IAppSettings appSettings, ICasinoSetting casinoSetting, IRngCasinoBetSettingDataAccess betSettingDataAccess)
        {
            _betSettingDataAccess = betSettingDataAccess;
            _casinoSetting = casinoSetting;
            _appSettings = appSettings;
        }

        public RngCasinoBetSettingViewModel GetViewModel(int custId, CustomerInfo customer, string language)
        {
            IList<GameKind> gameKinds;

            gameKinds = customer.SyncCasino ?
            _casinoSetting.GetRngCasinoSettings(custId, language) : new List<GameKind>();

            return new RngCasinoBetSettingViewModel
            {
                GameKinds = gameKinds,
                IsEnabled = IsRngCasinoEnabled(customer)
            };
        }

        public IResultInfo UpdateStatus(int licenseeId, int userId, int custId, int status, string language, string ip, string username, bool isSyncCasino)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            if (isSyncCasino && !_appSettings.IsDisableRngCasino)
            {
                if (!_customerApi.UpdateCustEnableCasino(custId, status == 0, false, ip, userId, username, 4))
                {
                    return ErrorResult(ViewResources.UpdateUnSuccess);
                }

                return _betSettingDataAccess.UpdateStatus(userId, custId, status, language, ip);
            }

            return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
        }

        public IResultInfo UpdateSetting(RngCasinoBetSettingMetaModel metaModel, bool isSyncCasino, bool isDisableCasino)
        {
            if (isDisableCasino || !HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            var result = false;

            if (isSyncCasino)
            {
                if (!_appSettings.IsDisableRngCasino)
                {
                    result = _casinoSetting.UpdateSetting(metaModel.TableParam);

                    return result ?
                        new ResultInfo { Code = 0, Message = ViewResources.MessageSuccess }
                        : new ResultInfo { Code = 1, Message = ViewResources.Fail };
                }
                else
                {
                    return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
                }
            }

            return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
        }

        private static bool? IsRngCasinoEnabled(CustomerInfo customer)
        {
            if (!customer.DisableCasino && customer.SyncCasino)
            {
                return true;
            }

            if (!customer.SyncCasino)
            {
                return null;
            }

            return false;
        }
    }
}