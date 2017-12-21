// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using CitiXchangeLibrary;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Constants;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Interfaces;
    using Logging;
    using Model.Core.Constants;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Common;
    using Resources;

    public class ExchangeCommApiService : BetSettingBaseService, IExchangeCommApiService
    {
        private static readonly IDictionary<string, Enviro> EnvironmentMap =
            new Dictionary<string, Enviro>(StringComparer.OrdinalIgnoreCase)
        {
            { SystemEnvironment.Production, Enviro.PRD },
            { SystemEnvironment.UAT, Enviro.UAT },
            { SystemEnvironment.SIT, Enviro.SIT }
        };

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

        public IResultInfo<decimal> GetCommission(int licenseeId, int custId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return CreateInvalidParamResult();
            }

            try
            {
                _logger.Info(licenseeId + ";" + custId);
                var result = _apiClient.getProfile(custId, EnvironmentMap[_appSetting.Environment]);
                _logger.Info(new JavaScriptSerializer().Serialize(result));
                return CreateApiResult(result.status, result.commission);
            }
            catch (Exception exception)
            {
                _logger.Error("Exchange Api getProfile: " + exception.Message);
                return CreateApiErrorResult();
            }
        }

        public IResultInfo<decimal> UpdateCommission(int licenseeId, int userId, int custId, decimal commission)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return CreateInvalidParamResult();
            }

            try
            {
                _logger.Info(licenseeId + ";" + custId + ";" + commission);
                var result = _apiClient.updateCommission(custId, (int)(commission * 100), EnvironmentMap[_appSetting.Environment]);

                _logger.Info(new JavaScriptSerializer().Serialize(result));
                _userLogDataAccess.WriteLog(new UserLogMetaModel
                {
                    ActionID = UserLogAction.ExchangeCommission,
                    UserId = userId,
                    CustID = custId
                });
                return CreateApiResult(result.status, result.commission);
            }
            catch (Exception exception)
            {
                _logger.Error("Exchange Api updateCommission: " + exception.Message);
                return CreateApiErrorResult();
            }
        }

        private static IResultInfo<decimal> CreateApiResult(CitiStatus status, decimal commission)
        {
            var isSuccess = status == CitiStatus.Success;
            return new ResultInfo<decimal>
            {
                Code = isSuccess ? 0 : (int)status,
                Message = isSuccess ? ViewResources.UpdateSuccess : ViewResources.ErrorProcessingMsg,
                ExtendedData = commission
            };
        }

        private static IResultInfo<decimal> CreateInvalidParamResult()
        {
            return new ResultInfo<decimal>
            {
                Code = 101,
                Message = ViewResources.InvalidParams
            };
        }

        private static IResultInfo<decimal> CreateApiErrorResult()
        {
            return new ResultInfo<decimal>
            {
                Code = 102,
                Message = ViewResources.ErrorProcessingMsg
            };
        }
    }
}