namespace Fanex.Athena.Api.Dashboard
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base.Attributes;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    [ApiOneAuthorization]
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

        [HttpPost]
        public IDictionary<string, int> GetCurrencyList()
        {
            return _currencyService.GetCurrencyMapByUser(this.LicenseeId, this.UserId);
        }
    }
}