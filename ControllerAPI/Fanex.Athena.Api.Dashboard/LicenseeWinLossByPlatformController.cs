namespace Fanex.Athena.Api.Dashboard
{
    using Fanex.Athena.Api.Base.Attributes;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard;
    using Fanex.Athena.Service.Dashboard.Interfaces;

    [ApiOneAuthorization]
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

        public WidgetByPlatformViewModel Index(DashboardBaseModel metaModel)
        {
            AssignAuthProperties(metaModel);
            return _licenseeWinLossByPlatformService.GetData(metaModel);
        }
    }
}