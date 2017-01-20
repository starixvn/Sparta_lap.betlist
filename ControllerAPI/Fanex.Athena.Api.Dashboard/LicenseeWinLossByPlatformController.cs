using System.Web.Http;
using Fanex.Athena.Model.Dashboard.MetaModel;
using Fanex.Athena.Model.Dashboard.ViewModel;
using Fanex.Athena.Service.Dashboard;
using Fanex.Athena.Service.Dashboard.Interfaces;

namespace Fanex.Athena.Api.Dashboard
{
    public class LicenseeWinLossByPlatformController : ApiController
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
        public WidgetByPlatformViewModel Index(DashboardBaseModel metaModel)
        {
            return _licenseeWinLossByPlatformService.GetData(metaModel);
        }
    }
}
