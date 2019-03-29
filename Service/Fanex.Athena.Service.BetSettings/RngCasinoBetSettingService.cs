// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Casino.Interfaces;
    using Casino.Wrappers;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Interfaces;
    using Model.BetSettings.DTO.RngCasino;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using Resources;

    public class RngCasinoBetSettingService : BetSettingBaseService, IRngCasinoBetSettingService
    {
        private readonly IRngCasinoBetSettingDataAccess _betSettingDataAccess;

        private readonly ICasinoSettingService _casinoSetting;

        private readonly ICustomerApiWrapper _customerApiWrapper;

        private readonly IAppSettings _appSettings;

        public RngCasinoBetSettingService(
            IAuthorizationService authorizationService,
            IAppSettings appSettings,
            ICasinoSettingService casinoSetting,
            ICustomerApiWrapper customerApiWrapper,
            IRngCasinoBetSettingDataAccess betSettingDataAccess)
            : base(authorizationService)
        {
            _betSettingDataAccess = betSettingDataAccess;
            _casinoSetting = casinoSetting;
            _appSettings = appSettings;
            _customerApiWrapper = customerApiWrapper;
        }

        public RngCasinoBetSettingViewModel GetViewModel(int custId, CustomerInfo customer, string language)
        {
            IList<GameKind> gameKinds;
            gameKinds = customer.SyncCasino ?
            _casinoSetting.GetRngCasinoSettings(custId, customer.CurrencyID, language) : new List<GameKind>();

            return new RngCasinoBetSettingViewModel
            {
                GameKinds = gameKinds,
                IsEnabled = IsRngCasinoEnabled(customer)
            };
        }

        public async Task<IResultInfo> UpdateStatusAsync(BetSettingCasinoStatusMetaModel metaModel)
        {
            if (!(await HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId).ConfigureAwait(false)))
            {
                return ErrorResult(ViewResources.AccessDeniedMsg);
            }

            if (metaModel.IsSyncCasino && !_appSettings.IsDisableRngCasino)
            {
                if (!_customerApiWrapper.UpdateCustEnableCasino(metaModel))
                {
                    return ErrorResult(ViewResources.UpdateUnSuccess);
                }

                return await _betSettingDataAccess.UpdateStatusAsync(metaModel);
            }

            return new ResultInfo { Code = 1, Message = ViewResources.RNGCasinoUpdateSettingUnAvailableMsg };
        }

        public async Task<IResultInfo> UpdateSetting(RngCasinoBetSettingMetaModel metaModel, bool isSyncCasino, bool isDisableCasino)
        {
            if (isDisableCasino || !(await HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId).ConfigureAwait(false)))
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