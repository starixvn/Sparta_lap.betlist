// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System.Threading.Tasks;
    using DataAccess.Customer.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Model.Core.ResultModel;
    using Resources;

    public class CustomerListService : ICustomerListService
    {
        private readonly ICustomerDataAccess _customerDataAccess;

        private readonly ICustomerListDataAccess _customerListDataAccess;

        private readonly IDormantCustomerListDataAccess _dormantCustomerListDataAccess;

        private readonly ICasinoCustomerService _casinoCustomerService;

        private readonly IAuthorizationService _authorizationService;

        private readonly IProductService _productService;

        private readonly IAppSettings _appSettings;

        public CustomerListService(
            ICustomerDataAccess customerDataAccess,
            ICustomerListDataAccess customerListDataAccess,
            IDormantCustomerListDataAccess dormantCustomerListDataAccess,
            ICasinoCustomerService casinoCustomerService,
            IAuthorizationService authorizationService,
            IProductService productService,
            IAppSettings appSettings)
        {
            _customerDataAccess = customerDataAccess;
            _customerListDataAccess = customerListDataAccess;
            _dormantCustomerListDataAccess = dormantCustomerListDataAccess;
            _casinoCustomerService = casinoCustomerService;
            _authorizationService = authorizationService;
            _productService = productService;
            _appSettings = appSettings;
        }

        public Task<ReportResultModel<CustomerDTO>> GetActiveCustomers(CustomerListMetaModel metaModel)
        {
            return _customerListDataAccess.GetActiveCustomers(metaModel);
        }

        public Task<ReportResultModel<DormantCustomer>> GetDormantCustomers(CustomerListMetaModel metaModel)
        {
            return _dormantCustomerListDataAccess.GetDormantCustomers(metaModel);
        }

        public async Task<LogInfo> GetCustomerLoginInfo(int custId, int licenseeId, string siteName)
        {
            return await _authorizationService.HasPermissionOnData(licenseeId, custId, 0).ConfigureAwait(false)
                ? _customerListDataAccess.GetCustomerLoginInfo(custId, licenseeId, siteName)
                : new LogInfo();
        }

        #region Update

        public async Task<IResultInfo> UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel)
        {
            if (!(await _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).ConfigureAwait(false)))
            {
                return CreateErrorResult();
            }

            return _customerListDataAccess.UpdateOddsGroup(metaModel);
        }

        public async Task<IResultInfo> UpdateCustClosed(CustomerStatusMetaModel metaModel, string languageCode)
        {
            if (!(await _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).ConfigureAwait(false)))
            {
                return CreateErrorResult();
            }

            var customerInfo = _customerDataAccess.GetCustomerInfo(metaModel.CustId);
            var closeCasinoResult = true;
            var productAccess = await this._productService.GetProductAccess(metaModel.LicenseeId, languageCode).ConfigureAwait(false);
            var casinoSupported = productAccess.IsAllowCS;

            if (customerInfo.SyncCasino && casinoSupported)
            {
                if (!_appSettings.IsDisableRngCasino)
                {
                    var casinoMetaModel = new CustomerStatusMetaModel
                    {
                        CustId = metaModel.CustId,
                        LicenseeName = metaModel.LicenseeName.ToUpperInvariant(),
                        IsOn = metaModel.IsOn,
                        ModifyById = metaModel.ModifyById,
                        ModifyByUsername = metaModel.ModifyByUsername,
                        Ip = metaModel.Ip
                    };
                    closeCasinoResult = _casinoCustomerService.UpdateClosed(casinoMetaModel);
                }
                else
                {
                    return new ResultInfo { Code = 1, Message = "RNG Casino Database is not currently available. Please try again later." };
                }
            }

            return closeCasinoResult
                ? _customerListDataAccess.UpdateCustClosed(metaModel)
                : new ResultInfo { Code = 1, Message = "Error when update closed status on casino db" };
        }

        public async Task<IResultInfo> UpdateCustSuspend(CustomerStatusMetaModel metaModel, string languageCode)
        {
            if (!(await _authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0).ConfigureAwait(false)))
            {
                return CreateErrorResult();
            }

            var customerInfo = _customerDataAccess.GetCustomerInfo(metaModel.CustId);
            var suspendCasinoResult = true;
            var productAccess = await this._productService.GetProductAccess(metaModel.LicenseeId, languageCode).ConfigureAwait(false);
            var casinoSupported = productAccess.IsAllowCS;

            if (customerInfo.SyncCasino && casinoSupported)
            {
                if (!_appSettings.IsDisableRngCasino)
                {
                    var casinoMetaModel = new CustomerStatusMetaModel
                    {
                        CustId = metaModel.CustId,
                        LicenseeName = metaModel.LicenseeName.ToUpperInvariant(),
                        IsOn = metaModel.IsOn,
                        ModifyById = metaModel.ModifyById,
                        ModifyByUsername = metaModel.ModifyByUsername,
                        Ip = metaModel.Ip
                    };

                    suspendCasinoResult = _casinoCustomerService.UpdateSuspend(casinoMetaModel);
                }
                else
                {
                    return new ResultInfo { Code = 1, Message = "RNG Casino Database is not currently available. Please try again later." };
                }
            }

            return suspendCasinoResult
                ? _customerListDataAccess.UpdateCustSuspend(metaModel)
                : new ResultInfo { Code = 1, Message = "Error when update suspend status on casino db" };
        }

        public async Task<IResultInfo> UpdateCustomerBAStatus(
            int licenseeId,
            int custId,
            int modifiedBy,
            bool allowBA,
            Request request)
        {
            if (!(await _authorizationService.HasPermissionOnData(licenseeId, custId, 0).ConfigureAwait(false)))
            {
                return CreateErrorResult();
            }

            return this._customerListDataAccess.UpdateCustomerBAStatus(
                licenseeId,
                   custId,
                   modifiedBy,
                   allowBA,
                   request);
        }

        public async Task<IResultInfo> UpdateSpecialLevel(int licenseeId, int userId, int custId, int level, string ip, string language)
        {
            if (!(await _authorizationService.HasPermissionOnData(licenseeId, custId, 0).ConfigureAwait(false)))
            {
                return CreateErrorResult();
            }

            return _customerListDataAccess.UpdateSpecialLevel(userId, custId, level, ip, language);
        }

        private static IResultInfo CreateErrorResult()
        {
            return new ResultInfo
            {
                Code = 1,
                Message = ViewResources.UpdateSpecialLevelMsg
            };
        }

        #endregion Update
    }
}