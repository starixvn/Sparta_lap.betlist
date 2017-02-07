namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard;
    using Fanex.Athena.Service.Dashboard.Interfaces;

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
            return _licenseeWinLossByPlatformService.GetData(metaModel);
        }
    }
}