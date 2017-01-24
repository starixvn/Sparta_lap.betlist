namespace Fanex.Athena.Api.Dashboard
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base;
    using Base.Attributes;
    using Model.Core.DataDomain;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    [AllowAnonymous]
    [ApiOneAuthorization]
    [AllowCrossSiteJson]
    [XFrameOptionApi]
    public class DashboardController : BaseDashboardApiController
    {
        private readonly ICurrencyService _currencyService;

        public DashboardController() : this(null)
        {
        }

        public DashboardController(ICurrencyService currencyService)
        {
            _currencyService = currencyService ?? new CurrencyService();
        }

        public object GetCurrencyList()
        {
            var currencyList = _currencyService.GetCurrencyMapByUser(this.LicenseeId, this.UserId);
            return new
            {
                CurrentList = currencyList,
                BaseCurrencyList = SystemConstants.BaseCurrencyList
            };
        }
    }
}