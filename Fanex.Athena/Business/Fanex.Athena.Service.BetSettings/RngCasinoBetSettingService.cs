// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using CasinoLibrary.SBAgent;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Service.Casino;
    using Fanex.Athena.Service.Customer;
    using Models.DTO.Common;
    using Models.MetaModel.Casino;
    using Models.ViewModel;

    public class RngCasinoBetSettingService : BetSettingBaseService, IRngCasinoBetSettingService
    {
        private readonly IRngCasinoBetSettingDataAccess _betSettingDataAccess;

        private readonly CasinoSetting _casinoSetting;

        private readonly CustomerService _customerService;

        private readonly Customer _customerApi;

        public RngCasinoBetSettingService()
        {
            _betSettingDataAccess = new RngCasinoBetSettingDataAccess();
            _casinoSetting = new CasinoSetting();
            _customerService = new CustomerService();
            _customerApi = new Customer();
        }

        public IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip, string username)
        {
            if (!HasPermissionOnData(custId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var customerInfo = _customerService.GetCustomerInfo(custId);
            if (customerInfo.SyncCasino && !AppSettings.IsDisableRngCasino)
            {
                if (!_customerApi.UpdateCustEnableCasino(custId, status == 0, false, ip, userId, username, 4))
                {
                    return ErrorResult(ViewResources.UpdateUnSuccess);
                }

                return _betSettingDataAccess.UpdateRngCasinoStatus(userId, custId, status, language, ip);
            }

            return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
        }

        public IResultInfo UpdateRngCasinoSetting(RngCasinoBetSettingMetaModel metaModel)
        {
            var customerInfo = _customerService.GetCustomerInfo(metaModel.CustId);

            if (customerInfo.DisableCasino || !HasPermissionOnData(metaModel.CustId))
            {
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var result = false;

            if (customerInfo.SyncCasino)
            {
                if (!AppSettings.IsDisableRngCasino)
                {
                    result = _casinoSetting.UpdateRngCasinoSetting(metaModel.TableParam, metaModel.LicenseeName);

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
    }
}