// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System;
    using Casino.Wrappers;
    using DataAccess.Customer.Interfaces;
    using Fanex.Athena.DataAccess.Customer;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Logging;

    public class CustomerCommService : ICustomerCommService
    {
        private readonly ILogger _logger;

        private readonly ICustomerApiWrapper _customerApiWrapper;

        private readonly ICustomerCommDataAccess _customerCommDataAccess;

        private readonly ISessionService _sessionService;

        private readonly IGroupComDataAccess _groupComDataAccess;

        public CustomerCommService() : this(null, null, null, null, null)
        {
        }

        public CustomerCommService(
            ILogger logger,
            ICustomerApiWrapper customerApiWrapper,
            ICustomerCommDataAccess customerCommDataAccess,
            ISessionService sessionService,
            IGroupComDataAccess groupComDataAccess)
        {
            _logger = logger ?? Logger.Log;
            _customerApiWrapper = customerApiWrapper ?? new CustomerApiWrapper();
            _customerCommDataAccess = customerCommDataAccess ?? new CustomerCommDataAccess();
            _sessionService = sessionService ?? new SessionService();
            _groupComDataAccess = groupComDataAccess ?? new GroupComDataAccess();
        }

        public IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel, bool isSyncCSComm = false)
        {
            var result = _customerCommDataAccess.UpdateCustomersCommission(metaModel);
            var callApiSuccess = true;
            if (result.IsSuccessful && isSyncCSComm && !string.IsNullOrWhiteSpace(result.ExtendedData))
            {
                callApiSuccess = CallApiToUpdateComm(result.ExtendedData, metaModel.CSComm, metaModel.Ip);
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
            var groupInfo = _groupComDataAccess.GetById(metaModel.GroupComId);
            if (groupInfo == null)
            {
                return new ResultInfo
                {
                    Code = 1,
                    Message = "Group does not exists"
                };
            }

            var callApiSuccess = true;
            result = _customerCommDataAccess.MoveCustomersToGroup(metaModel);
            if (result.IsSuccessful && metaModel.IsAllowedRNG && !string.IsNullOrWhiteSpace(result.ExtendedData))
            {
                callApiSuccess = CallApiToUpdateComm(result.ExtendedData, groupInfo.CSComm, metaModel.Ip);
            }

            return callApiSuccess ? result : new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };
        }

        public IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel, bool isAllowedRNGCasino)
        {
            var result = _customerCommDataAccess.RemoveCustomersFromGroup(metaModel);
            var callApiSuccess = true;
            if (result.IsSuccessful && metaModel.IsRemovePlayercomm && isAllowedRNGCasino && !string.IsNullOrWhiteSpace(result.ExtendedData))
            {
                callApiSuccess = CallApiToUpdateComm(result.ExtendedData, 0, metaModel.Ip);
            }

            return callApiSuccess ? result : new ResultInfo
            {
                Code = 1,
                Message = "Having errors when calling CasinoAPI to update customer commission."
            };
        }

        public bool UpdateMemberCommissionApi(RNGCasinoUpdateMemberComMetaModel metaModel)
        {
            const int System = 4;
            const string API_ERROR_MSG = "Found errors when calling casino API to update customer commission.\n customers=";
            const string AdditionErrorMsgPattern = "\n IP={0},CustomerId={1},ModifyBy={2},ModifyBy={3},System={4},Site={5}";
            var additionalErrorMsg = string.Format(
                AdditionErrorMsgPattern,
                metaModel.FromIp,
                string.Join(",", metaModel.CustomerId),
                metaModel.ModifyBy,
                metaModel.ModifyUserName,
                System,
                metaModel.SiteName);
            var success = false;
            try
            {
                success = _customerApiWrapper.UpdateComm(
                     metaModel.CustomerId,
                     metaModel.Comm,
                     metaModel.FromIp,
                     metaModel.ModifyBy,
                     metaModel.ModifyUserName,
                     System);
                if (!success)
                {
                    _logger.Error(API_ERROR_MSG + string.Join(",", metaModel.CustomerId) + additionalErrorMsg);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message + additionalErrorMsg, exception);
            }

            return success;
        }

        private bool CallApiToUpdateComm(string customerIdList, decimal commission, string ip)
        {
            var customerIds = customerIdList.Split(',');
            if (customerIds.Length > 0)
            {
                var userSession = _sessionService.UserSession;
                return UpdateMemberCommissionApi(new RNGCasinoUpdateMemberComMetaModel
                {
                    Comm = commission,
                    CustomerId = customerIds,
                    FromIp = ip,
                    ModifyBy = userSession.UserId,
                    ModifyUserName = userSession.UserName
                });
            }

            return true;
        }
    }
}