// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System;

    #region Using

    using System.Collections.Generic;
    using DataAccess.Customer.Interfaces;
    using Fanex.Athena.DataAccess.Customer;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Casino.MetaModel;
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

    #endregion Using

    public class CustomerListService : ICustomerListService
    {
        private readonly ICustomerDataAccess _customerDataAccess;

        private readonly ICustomerListDataAccess _customerListDataAccess;

        private readonly ICasinoSetting _casinoSetting;

        private readonly IAuthorizationService _authorizationService;

        private readonly IProductService _productService;

        private readonly ISessionService _sessionService;

        private readonly IRNGCasinoService _rngCasinoService;

        private readonly IAppSettings _appSettings;

        private readonly IGroupComService _groupComService;

        public CustomerListService()
        {
            _customerDataAccess = new CustomerDataAccess();
            _customerListDataAccess = new CustomerListDataAccess();
            _casinoSetting = new CasinoSetting();
            _authorizationService = new AuthorizationService();
            _productService = new ProductService();
            _sessionService = new SessionService();
            _rngCasinoService = new RNGCasinoService();
            _appSettings = new AppSettings();
            _groupComService = new GroupComService();
        }

        public CustomerListService(
            ICustomerDataAccess customerDataAccess,
            ICustomerListDataAccess customerListDataAccess,
            ICasinoSetting casinoSetting,
            IAuthorizationService authorizationService,
            IProductService productService,
            ISessionService sessionService,
            IRNGCasinoService rngCasinoService,
            IAppSettings appSettings)
        {
            _customerDataAccess = customerDataAccess;
            _customerListDataAccess = customerListDataAccess;
            _casinoSetting = casinoSetting;
            _authorizationService = authorizationService;
            _productService = productService;
            _sessionService = sessionService;
            _rngCasinoService = rngCasinoService;
            _appSettings = appSettings;
        }

        public IEnumerable<string> SearchCustomers(int userId, string userName, string agentListXml)
        {
            if (!IsValidUsername(userName))
            {
                return new string[] { };
            }

            return _customerListDataAccess.SearchCustomers(userId, userName, agentListXml);
        }

        public ReportResultModel<CustomerDTO> Search(CustomerListMetaModel metaModel)
        {
            if (!IsValidUsername(metaModel.Username))
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

        #region Group Commission

        public IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel, bool isSyncCSComm = false)
        {
            var result = _customerListDataAccess.UpdateCustomersCommission(metaModel);
            var callApiSuccess = true;
            if (result.IsSuccessful && isSyncCSComm && !string.IsNullOrWhiteSpace(result.ExtendedData))
            {
                var customerIds = result.ExtendedData.Split(',');
                if (customerIds.Length > 0)
                {
                    callApiSuccess = _rngCasinoService.UpdateMemberCommission(new RNGCasinoUpdateMemberComMetaModel
                    {
                        Comm = metaModel.CSComm,
                        CustomerId = customerIds,
                        FromIp = metaModel.Ip,
                        ModifyBy = metaModel.UserId,
                        ModifyUserName = _sessionService.UserSession.UserName
                    });
                }
            }

            return callApiSuccess ? result : new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };
        }

        /// <summary>
        ///     Move customers to new group including 2 following steps:
        ///     1: Remove all customers commission in group.
        ///     2: Update new customers commission in this group.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The result information.</returns>
        public IResultInfo MoveCustomersToGroup(MoveToGroupComMetaModel metaModel)
        {
            IResultInfo result = null;
            var groupInfo = _groupComService.GetById(metaModel.GroupComId);
            var callApiSuccess = true;
            if (groupInfo != null)
            {
                result = _customerListDataAccess.MoveCustomersToGroup(metaModel);

                if (result.IsSuccessful && metaModel.IsAllowedRNG && !string.IsNullOrWhiteSpace(result.ExtendedData))
                {
                    var customerIds = result.ExtendedData.Split(',');
                    if (customerIds.Length > 0)
                    {
                        var user = _sessionService.UserSession;
                        callApiSuccess = _rngCasinoService.UpdateMemberCommission(new RNGCasinoUpdateMemberComMetaModel
                        {
                            Comm = groupInfo.CSComm,
                            CustomerId = customerIds,
                            FromIp = metaModel.Ip,
                            ModifyBy = user.UserId,
                            ModifyUserName = user.UserName
                        });
                    }
                }
            }

            return callApiSuccess ? result : new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };
        }

        public IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel, bool isAllowedRNGCasino)
        {
            var result = _customerListDataAccess.RemoveCustomersFromGroup(metaModel);
            var callApiSuccess = true;
            if (result.IsSuccessful && metaModel.IsRemovePlayercomm && isAllowedRNGCasino && !string.IsNullOrWhiteSpace(result.ExtendedData))
            {
                var customerIds = result.ExtendedData.Split(',');
                if (customerIds.Length > 0)
                {
                    var user = _sessionService.UserSession;
                    callApiSuccess = _rngCasinoService.UpdateMemberCommission(new RNGCasinoUpdateMemberComMetaModel
                    {
                        Comm = 0,
                        CustomerId = customerIds,
                        FromIp = metaModel.Ip,
                        ModifyBy = user.UserId,
                        ModifyUserName = user.UserName
                    });
                }
            }

            return callApiSuccess ? result : new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };
        }

        #endregion Group Commission

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

        private bool IsValidUsername(string userName)
        {
            if (userName != null && _sessionService.LicenseeSession.LicenseeId == LicenseeIdentity.IvoryBet)
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