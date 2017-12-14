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
        }

        public decimal GetCommission(int ibcCustId, int currencyId)
        {
            // var result = _apiClient.getProfile(ibcCustId, currencyId, Enviro.PRD);
            // return result.commission;
            return 100;
        }

        public decimal UpdateCommission(int ibcCustId, int commission)
        {
            // var result = _apiClient.updateCommission(ibcCustId, commission, Enviro.PRD);
            // return result.commission;
            return 100;
        }
    }
}