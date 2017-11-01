// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System;
    using DataAccess.Customer.Interfaces;
    using Fanex.Athena.DataAccess.Customer;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Casino;
    using Fanex.Athena.Service.Casino.Interfaces;
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

        private readonly IGroupComService _groupComService;

        public CustomerCommService()
        {
        }

        public CustomerCommService(
            ILogger logger,
            ICustomerApiWrapper customerApiWrapper,
            ICustomerCommDataAccess customerCommDataAccess,
            ISessionService sessionService,
            IGroupComService groupComService)
        {
            _logger = logger ?? Logger.Log;
            _customerApiWrapper = customerApiWrapper ?? new CustomerApiWrapper();
            _customerCommDataAccess = customerCommDataAccess ?? new CustomerCommDataAccess();
            _sessionService = sessionService ?? new SessionService();
            _groupComService = groupComService ?? new GroupComService();
        }

        public IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel, bool isSyncCSComm = false)
        {
            var result = _customerCommDataAccess.UpdateCustomersCommission(metaModel);
            var callApiSuccess = true;
            if (result.IsSuccessful && isSyncCSComm && !string.IsNullOrWhiteSpace(result.ExtendedData))
            {
                var customerIds = result.ExtendedData.Split(',');
                if (customerIds.Length > 0)
                {
                    callApiSuccess = UpdateMemberCommissionApi(new RNGCasinoUpdateMemberComMetaModel
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
                result = _customerCommDataAccess.MoveCustomersToGroup(metaModel);

                if (result.IsSuccessful && metaModel.IsAllowedRNG && !string.IsNullOrWhiteSpace(result.ExtendedData))
                {
                    var customerIds = result.ExtendedData.Split(',');
                    if (customerIds.Length > 0)
                    {
                        var user = _sessionService.UserSession;
                        callApiSuccess = UpdateMemberCommissionApi(new RNGCasinoUpdateMemberComMetaModel
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
            var result = _customerCommDataAccess.RemoveCustomersFromGroup(metaModel);
            var callApiSuccess = true;
            if (result.IsSuccessful && metaModel.IsRemovePlayercomm && isAllowedRNGCasino && !string.IsNullOrWhiteSpace(result.ExtendedData))
            {
                var customerIds = result.ExtendedData.Split(',');
                if (customerIds.Length > 0)
                {
                    var user = _sessionService.UserSession;
                    callApiSuccess = UpdateMemberCommissionApi(new RNGCasinoUpdateMemberComMetaModel
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
    }
}