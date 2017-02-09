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

        public LicenseeWinLossByPlatformController() : this(null)
        {
        }

        public LicenseeWinLossByPlatformController(IWidgetByPlatformService licenseeWinLossByPlatformService)
        {
            this._licenseeWinLossByPlatformService = licenseeWinLossByPlatformService ?? new LicenseeWinLossByPlatformService();
        }

        [HttpGet]
        public WidgetByPlatformViewModel GetData([FromUri] DashboardBaseModel metaModel)
        {
            AssignAuthProperties(metaModel);
            var now = DateTime.Now.Date.AddMonths(-1);
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = DateTime.Now.Date;
            return _licenseeWinLossByPlatformService.GetData(metaModel);
        }
    }
}