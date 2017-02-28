namespace Fanex.Athena.Api.Dashboard
{
    using System.Collections.Generic;
    using System.Linq;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

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
            var currencyList = _currencyService.GetActiveCurrenciesByUser(this.LicenseeId, this.UserId);

            return new
            {
                CurrencyList = currencyList.ToArray(),
                BaseCurrencyList = new List<string> { "EUR", "USD", "GBP", "MYR", "CNY" }
            };
        }
    }
}