// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System;
    using System.Web.Script.Serialization;
    using CitiXchangeLibrary;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Constants;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Interfaces;
    using Logging;
    using Model.Core.MetaModel.Common;

    public class ExchangeCommApiService : BetSettingBaseService, IExchangeCommApiService
    {
        private readonly ILogger _logger;

        private readonly IUserLogDataAccess _userLogDataAccess;

        private readonly IAppSettings _appSetting;

        private readonly ICitiXchangeClient _apiClient;

        public ExchangeCommApiService() : this(null, null, null, null)
        {
        }

        public ExchangeCommApiService(
            ILogger logger,
            IUserLogDataAccess userLogDataAccess,
            IAppSettings appSetting,
            ICitiXchangeClient apiClient)
        {
            _logger = logger ?? Logger.Log;
            _userLogDataAccess = userLogDataAccess ?? new UserLogDataAccess();
            _appSetting = appSetting ?? new AppSettings();
            _apiClient = apiClient ?? (ICitiXchangeClient)Activator.GetObject(typeof(ICitiXchangeClient), _appSetting.ExchangeProductApiHost);
        }

        public decimal GetCommission(int licenseeId, int custId, int currencyId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return 0;
            }

            try
            {
                _logger.Info(licenseeId + ";" + custId + ";" + currencyId);
                var result = _apiClient.getProfile(custId, currencyId, Enviro.SIT);
                _logger.Info(new JavaScriptSerializer().Serialize(result));
                return result.commission;
            }
            catch (Exception exception)
            {
                _logger.Error("Exchange Api getProfile: " + exception.Message);
                return 0;
            }
        }

        public decimal UpdateCommission(int licenseeId, int userId, int custId, decimal commission)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return 0;
            }

            try
            {
                _logger.Info(licenseeId + ";" + custId + ";" + commission);
                var result = _apiClient.updateCommission(custId, (int)commission, Enviro.SIT);
                _logger.Info(new JavaScriptSerializer().Serialize(result));
                _userLogDataAccess.WriteLog(new UserLogMetaModel
                {
                    ActionID = UserLogAction.ExchangeCommission,
                    UserId = userId,
                    CustID = custId
                });
                return result.commission;
            }
            catch (Exception exception)
            {
                _logger.Error("Exchange Api getProfile: " + exception.Message);
                return 0;
            }
        }
    }
}