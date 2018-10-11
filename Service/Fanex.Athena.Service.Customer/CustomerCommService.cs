// -----------------------------------------------------------------------
// <copyright file="CustomerService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using Casino.Interfaces;
    using DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;

    public class CustomerCommService : ICustomerCommService
    {
        private readonly ICasinoCustomerService _casinoCustomerService;

        private readonly ICustomerCommDataAccess _customerCommDataAccess;

        private readonly ISessionService _sessionService;

        private readonly IGroupComDataAccess _groupComDataAccess;

        public CustomerCommService(
            ICasinoCustomerService casinoCustomerService,
            ICustomerCommDataAccess customerCommDataAccess,
            ISessionService sessionService,
            IGroupComDataAccess groupComDataAccess)
        {
            _casinoCustomerService = casinoCustomerService;
            _customerCommDataAccess = customerCommDataAccess;
            _sessionService = sessionService;
            _groupComDataAccess = groupComDataAccess;
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

        private bool CallApiToUpdateComm(string customerIdList, decimal commission, string ip)
        {
            var customerIds = customerIdList.Split(',');
            if (customerIds.Length > 0)
            {
                var userSession = _sessionService.UserSession;
                return _casinoCustomerService.UpdateCommission(new RNGCasinoUpdateMemberComMetaModel
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