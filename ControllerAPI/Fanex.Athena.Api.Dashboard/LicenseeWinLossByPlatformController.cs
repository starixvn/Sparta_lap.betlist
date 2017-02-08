namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;

    public class LicenseeWinLossByPlatformController : BaseDashboardApiController
    {
        private readonly IWidgetByPlatformService _licenseeWinLossByPlatformService;

        private readonly IBalanceUpService _balanceUpService;

        public LicenseeWinLossByPlatformController() : this(null, null)
        {
            _balanceUpService = new BalanceUpService();
        }

        public LicenseeWinLossByPlatformController(IWidgetByPlatformService licenseeWinLossByPlatformService, IBalanceUpService balanceUpService)
        {
            this._licenseeWinLossByPlatformService = licenseeWinLossByPlatformService ?? new LicenseeWinLossByPlatformService();
            this._balanceUpService = balanceUpService ?? new BalanceUpService();
        }

        [HttpGet]
        public WidgetByPlatformViewModel GetData([FromUri] DashboardBaseModel metaModel)
        {
            AssignAuthProperties(metaModel);
            var now = DateTime.Now.Date.AddMonths(-1);
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = DateTime.Now.Date;
            metaModel.MoveBetsDay = _balanceUpService.BalanceUpInfo.MoveBetsDay;
            return _licenseeWinLossByPlatformService.GetData(metaModel);
        }
    }
}