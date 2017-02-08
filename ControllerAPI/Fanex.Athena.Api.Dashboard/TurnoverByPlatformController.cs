namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;

    public class TurnoverByPlatformController : BaseDashboardApiController
    {
        private readonly IWidgetByPlatformService _turnoverByPlatformService;

        private readonly IBalanceUpService _balanceUpService;

        public TurnoverByPlatformController() : this(null, null)
        {
            _balanceUpService = new BalanceUpService();
        }

        public TurnoverByPlatformController(IWidgetByPlatformService turnoverByPlatformService, IBalanceUpService balanceUpService)
        {
            this._turnoverByPlatformService = turnoverByPlatformService ?? new TurnoverByPlatformService();
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
            return _turnoverByPlatformService.GetData(metaModel);
        }
    }
}