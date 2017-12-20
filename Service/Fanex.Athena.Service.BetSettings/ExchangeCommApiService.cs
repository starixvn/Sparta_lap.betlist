// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System;
    using CitiXchangeLibrary;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Constants;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Interfaces;
    using Model.Core.MetaModel.Common;

    public class ExchangeCommApiService : BetSettingBaseService, IExchangeCommApiService
    {
        private readonly IUserLogDataAccess _userLogDataAccess;

        private readonly IAppSettings _appSetting;

        private readonly ICitiXchangeClient _apiClient;

        public ExchangeCommApiService() : this(null, null, null)
        {
        }

        public ExchangeCommApiService(
            IUserLogDataAccess userLogDataAccess,
            IAppSettings appSetting,
            ICitiXchangeClient apiClient)
        {
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

            var result = _apiClient.getProfile(custId, currencyId, Enviro.SIT);
            return result.commission;
        }

        public decimal UpdateCommission(int licenseeId, int userId, int custId, decimal commission)
        {
            if (!HasPermissionOnData(licenseeId, custId))
            {
                return 0;
            }

            _userLogDataAccess.WriteLog(new UserLogMetaModel
            {
                ActionID = UserLogAction.ExchangeCommission,
                UserId = userId,
                CustID = custId
            });
            var result = _apiClient.updateCommission(custId, (int)commission, Enviro.SIT);
            return result.commission;
        }
    }
}