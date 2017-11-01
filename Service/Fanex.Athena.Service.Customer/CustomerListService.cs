// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System;
    using System.Collections.Generic;
    using DataAccess.Customer.Interfaces;
    using Fanex.Athena.DataAccess.Customer;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Casino;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Resources;

    public class CustomerListService : ICustomerListService
    {
        private readonly ICustomerDataAccess _customerDataAccess;

        private readonly ICustomerListDataAccess _customerListDataAccess;

        private readonly ICasinoSettingService _casinoSetting;

        private readonly IAuthorizationService _authorizationService;

        private readonly IProductService _productService;

        private readonly IAppSettings _appSettings;

        public CustomerListService()
        {
            _customerDataAccess = new CustomerDataAccess();
            _customerListDataAccess = new CustomerListDataAccess();
            _casinoSetting = new CasinoSettingService();
            _authorizationService = new AuthorizationService();
            _productService = new ProductService();
            _appSettings = new AppSettings();
        }

        public CustomerListService(
            ICustomerDataAccess customerDataAccess,
            ICustomerListDataAccess customerListDataAccess,
            ICasinoSettingService casinoSetting,
            IAuthorizationService authorizationService,
            IProductService productService,
            IAppSettings appSettings)
        {
            _customerDataAccess = customerDataAccess;
            _customerListDataAccess = customerListDataAccess;
            _casinoSetting = casinoSetting;
            _authorizationService = authorizationService;
            _productService = productService;
            _appSettings = appSettings;
        }

        public IEnumerable<string> SearchCustomers(int licenseeId, int userId, string userName, string agentListXml)
        {
            if (!IsValidUsername(licenseeId, userName))
            {
                return new string[] { };
            }

            return _customerListDataAccess.SearchCustomers(userId, userName, agentListXml);
        }

        public ReportResultModel<CustomerDTO> Search(CustomerListMetaModel metaModel)
        {
            if (!IsValidUsername(metaModel.LicenseeId, metaModel.Username))
            {
                return new ReportResultModel<CustomerDTO>
                {
                    DataList = new List<CustomerDTO>()
                };
            }

            return _customerListDataAccess.Search(metaModel);
        }

        public LogInfo GetCustomerLoginInfo(int custId, int licenseeId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, custId, 0)
                ? _customerListDataAccess.GetCustomerLoginInfo(custId, licenseeId)
                : new LogInfo();
        }

        #region Update

        public IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel)
        {
            if (!_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0))
            {
                return CreateErrorResult();
            }

            return _customerListDataAccess.UpdateOddsGroup(metaModel);
        }

        public IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel, string languageCode)
        {
            if (!_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0))
            {
                return CreateErrorResult();
            }

            var customerInfo = _customerDataAccess.GetCustomerInfo(metaModel.CustId);
            var closeCasinoResult = true;
            var productAccess = this._productService.GetProductAccess(metaModel.LicenseeId, languageCode);
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
                    closeCasinoResult = _casinoSetting.UpdateCustClosed(casinoMetaModel);
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

        public IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel, string languageCode)
        {
            if (!_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0))
            {
                return CreateErrorResult();
            }

            var customerInfo = _customerDataAccess.GetCustomerInfo(metaModel.CustId);
            var suspendCasinoResult = true;
            var productAccess = this._productService.GetProductAccess(metaModel.LicenseeId, languageCode);
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

                    suspendCasinoResult = _casinoSetting.UpdateCustSuspend(casinoMetaModel);
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

        public IResultInfo UpdateCustomerBAStatus(
            int licenseeId,
            int custId,
            int modifiedBy,
            bool allowBA,
            Request request)
        {
            if (!_authorizationService.HasPermissionOnData(licenseeId, custId, 0))
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

        public IResultInfo UpdateSpecialLevel(int licenseeId, int userId, int custId, int level, string ip, string language)
        {
            if (!_authorizationService.HasPermissionOnData(licenseeId, custId, 0))
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

        private static bool IsValidUsername(int licenseeId, string userName)
        {
            if (userName != null && licenseeId == LicenseeIdentity.IvoryBet)
            {
                const string IgnoredLicenseePrefix = "webet88";
                if (userName.StartsWith(IgnoredLicenseePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Update
    }
}