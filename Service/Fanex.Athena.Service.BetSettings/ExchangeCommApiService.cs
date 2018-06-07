// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System;
    using System.Collections.Generic;
    using CitiXchangeLibrary;
    using Constant.DataDomain;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
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

        private readonly IDcsBetSettingDataAccess _betSettingDataAccess;
        
        public ExchangeCommApiService(
            IAuthorizationService authorizationService,
            ILogger logger,
            IUserLogDataAccess userLogDataAccess,
            IDcsBetSettingDataAccess betSettingDataAccess,
            IAppSettings appSetting,
            ICitiXchangeClient apiClient)
            : base(authorizationService)
        {
            _logger = logger;
            _userLogDataAccess = userLogDataAccess;
            _betSettingDataAccess = betSettingDataAccess;
            _appSetting = appSetting;
            _apiClient = apiClient ?? (ICitiXchangeClient)Activator.GetObject(typeof(ICitiXchangeClient), _appSetting.ExchangeProductApiHost);
        }

        public IResultInfo<double> GetCommission(int licenseeId, int custId)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return CreateAccessDeniedResult();
            }

            try
            {
                var result = _apiClient.getProfile(custId, EnvironmentMap[_appSetting.Environment]);
                return CreateApiResult(result.status, (double)result.commission);
            }
            catch (Exception exception)
            {
                _logger.Error($"Exchange Api getProfile (custid: {custId}): " + exception.Message);
                return CreateApiErrorResult();
            }
        }

        public IResultInfo<double> UpdateCommission(int licenseeId, int userId, int custId, double oldCommission, double newCommission)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return CreateAccessDeniedResult();
            }

            var isEnable = _betSettingDataAccess.GetStatus(custId, ProductTypes.Exchange);

            if (isEnable != true)
            {
                return CreateAccessDeniedResult();
            }

            try
            {
                var result = _apiClient.updateCommission(custId, newCommission, EnvironmentMap[_appSetting.Environment]);
                _userLogDataAccess.WriteLog(new UserLogMetaModel
                {
                    ActionID = UserLogAction.ExchangeCommission,
                    UserId = userId,
                    CustID = custId,
                    Doing = $"<ChargeCommission OldValue='{oldCommission}' NewValue='{newCommission}' />"
                });
                return CreateApiResult(result.status, result.commission);
            }
            catch (Exception exception)
            {
                _logger.Error("Exchange Api updateCommission: " + exception.Message);
                return CreateApiErrorResult();
            }
        }

        private static IResultInfo<double> CreateApiResult(CitiStatus status, double commission)
        {
            var isSuccess = status == CitiStatus.Success;
            return new ResultInfo<double>
            {
                Code = isSuccess ? 0 : (status == 0 ? 1 : (int)status),
                Message = isSuccess ? ViewResources.UpdateSuccess : ViewResources.ErrorProcessingMsg,
                ExtendedData = commission
            };
        }

        private static IResultInfo<double> CreateAccessDeniedResult()
        {
            return new ResultInfo<double>
            {
                Code = 1,
                Message = ViewResources.FunctionDeniedMsg
            };
        }

        private static IResultInfo<double> CreateApiErrorResult()
        {
            return new ResultInfo<double>
            {
                Code = 102,
                Message = ViewResources.ErrorProcessingMsg
            };
        }
    }
}