// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System;
    using CitiXchangeLibrary;
    using Extension.Utility;
    using Interfaces;

    public class ExchangeCommApiService : IExchangeCommApiService
    {
        private readonly IAppSettings _appSetting;

        private readonly ICitiXchangeClient _apiClient;

        public ExchangeCommApiService() : this(null, null)
        {
        }

        public ExchangeCommApiService(IAppSettings appSetting, ICitiXchangeClient apiClient)
        {
            _appSetting = appSetting ?? new AppSettings();
            _apiClient = apiClient ?? (ICitiXchangeClient)Activator.GetObject(typeof(ICitiXchangeClient), _appSetting.ExchangeProductApiHost);
        }

        public decimal GetCommission(int ibcCustId, int currencyId)
        {
            var result = _apiClient.getProfile(ibcCustId, currencyId, Enviro.SIT);
            return result.commission;
        }

        public decimal UpdateCommission(int ibcCustId, decimal commission)
        {
            var result = _apiClient.updateCommission(ibcCustId, (int)commission, Enviro.SIT);
            return result.commission;
        }
    }
}