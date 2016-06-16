// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using CasinoLibrary.SBAgent;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Service.Casino;
    using Model.BetSettings.DTO.RngCasino;
    using Model.BetSettings.ViewModel;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using Resources;

    public class RngCasinoBetSettingService : BetSettingBaseService, IRngCasinoBetSettingService
    {
        private readonly IRngCasinoBetSettingDataAccess _betSettingDataAccess;

        private readonly CasinoSetting _casinoSetting;

        private readonly Customer _customerApi;

        public RngCasinoBetSettingService()
        {
            _betSettingDataAccess = new RngCasinoBetSettingDataAccess();
            _casinoSetting = new CasinoSetting();
            _customerApi = new Customer();
        }

        public RngCasinoBetSettingViewModel GetViewModel(int custId, CustomerInfo customer, string language, string licenseeName)
        {
            IList<GameKind> gameKinds;
            if (customer.SyncCasino)
            {
                gameKinds = _casinoSetting.GetRngCasinoSettings(custId, licenseeName, language);
            }
            else
            {
                gameKinds = new List<GameKind>();
            }

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
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            if (isSyncCasino && !AppSettings.IsDisableRngCasino)
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
                return ErrorResult(ViewResources.UpdateSpecialLevelMsg);
            }

            var result = false;

            if (isSyncCasino)
            {
                if (!AppSettings.IsDisableRngCasino)
                {
                    result = _casinoSetting.UpdateSetting(metaModel.TableParam, metaModel.LicenseeName);

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
            bool? resultValue = false;
            if (customer.DisableCasino == false && customer.SyncCasino)
            {
                resultValue = true;
            }
            else if (customer.SyncCasino == false)
            {
                resultValue = null;
            }

            return resultValue;
        }
    }
}