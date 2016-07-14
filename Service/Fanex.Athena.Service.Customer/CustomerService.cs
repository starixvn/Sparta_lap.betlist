// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    #region Using

    using System.Collections.Generic;
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
    using Resources;

    #endregion Using

    /// <summary>
    /// Customer service class.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        #region Varialbes

        private readonly CustomerDataAccess _customerDataAccess;

        private readonly ICasinoSetting _casinoSetting;

        private readonly IAuthorizationService _authorizationService;

        private readonly IProductService _productService;

        private readonly ISessionService _sessionService;

        private readonly IRNGCasinoService _rngCasinoService;

        #endregion Varialbes

        #region Constructors

        public CustomerService()
        {
            _customerDataAccess = new CustomerDataAccess();
            _casinoSetting = new CasinoSetting();
            _authorizationService = new AuthorizationService();
            _productService = new ProductService();
            _sessionService = new SessionService();
            _rngCasinoService = new RNGCasinoService();
        }

        #endregion Constructors

        #region Read

        public IEnumerable<string> SearchCustomers(int userId, string userName, int licenseeId, string licenseeType, int currencyId)
        {
            return _customerDataAccess.SearchCustomers(userId, userName, licenseeId, licenseeType, currencyId);
        }

        /// <summary>
        /// Advance search customer.
        /// </summary>
        /// <param name="metaModel">Meta model contains the filter such as: BA type, special level type, ...</param>
        /// <param name="totalRows">The total rows.</param>
        /// <returns>The customer list.</returns>
        public IList<CustomerDTO> Search(CustomerListMetaModel metaModel, ref int totalRows)
        {
            return _customerDataAccess.Search(metaModel, ref totalRows);
        }

        /// <summary>
        /// Get customer Information such as: SyncCasino, DisableBingo, ...
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <returns>The customer information.</returns>
        public CustomerInfo GetCustomerInfo(int licenseeId, int custId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, custId, 0)
                ? _customerDataAccess.GetCustomerInfo(custId)
                : new CustomerInfo();
        }

        #endregion Read

        #region LogInfo

        /// <summary>
        /// Get customer login information, last transfer time, ...
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <returns>The login information.</returns>
        public LogInfo GetCustomerLoginInfo(int custId, int licenseeId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, custId, 0)
                ? _customerDataAccess.GetCustomerLoginInfo(custId, licenseeId)
                : new LogInfo();
        }

        #endregion LogInfo

        #region Group Commission

        /// <summary>
        ///  Update customer commission setting.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="isSyncCSComm">Sync RNG commission or not.</param>
        /// <returns>The information of result.</returns>
        public IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel, bool isSyncCSComm = false)
        {
            var result = _customerDataAccess.UpdateCustomersCommission(metaModel);
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
                        ModifyUserName = _sessionService.LicenseeSession.UserName
                    });
                }
            }

            if (callApiSuccess)
            {
                return result;
            }
            else
            {
                return new ResultInfo()
                {
                    Code = 1,
                    Message = "Having errors when calling CasinoAPI to update customer commission."
                };
            }
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
            var groupService = new GroupComService();
            IResultInfo result = null;
            var groupInfo = groupService.GetById(metaModel.GroupComId);
            var callApiSuccess = true;
            if (groupInfo != null)
            {
                result = _customerDataAccess.MoveCustomersToGroup(metaModel);

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

            if (callApiSuccess)
            {
                return result;
            }
            else
            {
                return new ResultInfo()
                {
                    Code = 1,
                    Message = "Having errors when calling CasinoAPI to update customer commission."
                };
            }
        }

        /// <summary>
        /// Remove customer from commission group.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="isAllowedRNGCasino">Allow RNG Casino or not.</param>
        /// <returns>The result information.</returns>
        public IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel, bool isAllowedRNGCasino)
        {
            var result = _customerDataAccess.RemoveCustomersFromGroup(metaModel);
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

            if (callApiSuccess)
            {
                return result;
            }
            else
            {
                return new ResultInfo()
                {
                    Code = 1,
                    Message = "Having errors when calling CasinoAPI to update customer commission."
                };
            }
        }

        #endregion Group Commission

        #region Update

        /// <summary>
        /// Update customer odds group.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The result information.</returns>
        public IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel)
        {
            if (!_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0))
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }

            return _customerDataAccess.UpdateOddsGroup(metaModel);
        }

        /// <summary>
        /// Update customer closed status.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The result information.</returns>
        public IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel, string languageCode)
        {
            if (!_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0))
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }

            var customerInfo = _customerDataAccess.GetCustomerInfo(metaModel.CustId);
            var closeCasinoResult = true;
            var productAccess = this._productService.GetProductAccess(metaModel.LicenseeId, languageCode);
            var casinoSupported = productAccess.IsAllowCS;

            if (customerInfo.SyncCasino && casinoSupported)
            {
                if (!AppSettings.IsDisableRngCasino)
                {
                    var casinoMetaModel = new CasinoStatusMetaModel
                    {
                        CustId = metaModel.CustId,
                        LicenseeName = metaModel.LicenseeName.ToUpper(),
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
                ? _customerDataAccess.UpdateCustClosed(metaModel)
                : new ResultInfo { Code = 1, Message = "Error when update closed status on casino db" };
        }

        /// <summary>
        /// Update customer suspend status.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The result information.</returns>
        public IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel, string languageCode)
        {
            if (!_authorizationService.HasPermissionOnData(metaModel.LicenseeId, metaModel.CustId, 0))
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }

            var customerInfo = _customerDataAccess.GetCustomerInfo(metaModel.CustId);
            var suspendCasinoResult = true;
            var productAccess = this._productService.GetProductAccess(metaModel.LicenseeId, languageCode);
            var casinoSupported = productAccess.IsAllowCS;

            if (customerInfo.SyncCasino && casinoSupported)
            {
                if (!AppSettings.IsDisableRngCasino)
                {
                    var casinoMetaModel = new CasinoStatusMetaModel
                    {
                        CustId = metaModel.CustId,
                        LicenseeName = metaModel.LicenseeName.ToUpper(),
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
                ? _customerDataAccess.UpdateCustSuspend(metaModel)
                : new ResultInfo { Code = 1, Message = "Error when update suspend status on casino db" };
        }

        /// <summary>
        /// Update customer ba status.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="modifiedBy">The modifier.</param>
        /// <param name="allowBA">Is allowed BA.</param>
        /// <param name="request">The request information.</param>
        /// <returns>The result information.</returns>
        public IResultInfo UpdateCustomerBAStatus(
            int licenseeId,
            int custId,
            int modifiedBy,
            bool allowBA,
            Request request)
        {
            if (!_authorizationService.HasPermissionOnData(licenseeId, custId, 0))
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }

            return this._customerDataAccess.UpdateCustomerBAStatus(
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
                return new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.UpdateSpecialLevelMsg
                };
            }

            return _customerDataAccess.UpdateSpecialLevel(userId, custId, level, ip, language);
        }
        
        #endregion Update
    }
}