using System;
using Fanex.Athena.Model.Casino.MetaModel;
using Fanex.Athena.Model.Customer.MetaModel;
using Fanex.Athena.Service.Casino.Interfaces;
using Fanex.Athena.Service.Casino.Wrappers;
using Fanex.Athena.Service.Core.Exception;
using Fanex.Logging;

namespace Fanex.Athena.Service.Casino
{
    public class CasinoCustomerService : ICasinoCustomerService
    {
        private readonly ILogger _logger;

        private readonly ICustomerApiWrapper _customerApiWrapper;

        public CasinoCustomerService() : this(null, null)
        {
        }

        public CasinoCustomerService(ILogger logger, ICustomerApiWrapper customerApiWrapper)
        {
            _logger = logger ?? Logger.Log;
            _customerApiWrapper = customerApiWrapper ?? new CustomerApiWrapper();
        }

        public bool UpdateSuspend(CustomerStatusMetaModel metaModel)
        {
            try
            {
                return _customerApiWrapper.UpdateCustSuspend(metaModel);
            }
            catch (Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustSuspend. Message: " + e.Message, "casino api service");
            }
        }

        public bool UpdateClosed(CustomerStatusMetaModel metaModel)
        {
            try
            {
                return _customerApiWrapper.UpdateCustClosed(
                         metaModel);
            }
            catch (Exception e)
            {
                throw new ServiceException("Error when call casino api UpdateCustClosed. Message: " + e.Message, "Casino api service");
            }
        }

        public bool UpdateCommission(RNGCasinoUpdateMemberComMetaModel metaModel)
        {
            const string API_ERROR_MSG = "Found errors when calling casino API to update customer commission.\n customers=";
            const string AdditionErrorMsgPattern = "\n IP={0},CustomerId={1},ModifyBy={2},ModifyBy={3},Site={4}";
            var additionalErrorMsg = string.Format(
                AdditionErrorMsgPattern,
                metaModel.FromIp,
                string.Join(",", metaModel.CustomerId),
                metaModel.ModifyBy,
                metaModel.ModifyUserName,
                metaModel.SiteName);
            var success = false;
            try
            {
                success = _customerApiWrapper.UpdateComm(metaModel);
                if (!success)
                {
                    _logger.Error(API_ERROR_MSG + string.Join(",", metaModel.CustomerId) + additionalErrorMsg);
                }
            }
            catch (Exception exception)
            {
                _logger.Error("UpdateCommission:" + exception.Message + additionalErrorMsg, exception);
            }

            return success;
        }
    }
}