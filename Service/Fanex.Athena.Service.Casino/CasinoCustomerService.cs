using System;
using System.Threading.Tasks;
using Fanex.Athena.Model.Casino.MetaModel;
using Fanex.Athena.Model.Customer.MetaModel;
using Fanex.Athena.Service.Casino.Interfaces;
using Fanex.Athena.Service.Casino.Wrappers;
using Fanex.Athena.Service.Core.Exception;
using Fanex.Athena.Service.Core.Utility.Interfaces;

namespace Fanex.Athena.Service.Casino
{
    public class CasinoCustomerService : ICasinoCustomerService
    {
        private readonly ICoreLogger _logger;

        private readonly ICustomerApiWrapper _customerApiWrapper;

        public CasinoCustomerService(ICoreLogger logger, ICustomerApiWrapper customerApiWrapper)
        {
            _logger = logger;
            _customerApiWrapper = customerApiWrapper;
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

        public async Task<bool> UpdateCommission(RNGCasinoUpdateMemberComMetaModel metaModel)
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
                    await _logger.ErrorAsync(API_ERROR_MSG + string.Join(",", metaModel.CustomerId) + additionalErrorMsg).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "UpdateCommission:" + exception.Message + additionalErrorMsg);
            }

            return success;
        }
    }
}