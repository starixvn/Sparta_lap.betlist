namespace Fanex.Athena.Api.Dashboard
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Base.Attributes;
    using Model.Core.DataDomain;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

    [AllowAnonymous]
    [ApiOneAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                CurrencyList = currencyList.ToArray(),
                BaseCurrencyList = SystemConstants.BaseCurrencyList
            };
        }
    }
}