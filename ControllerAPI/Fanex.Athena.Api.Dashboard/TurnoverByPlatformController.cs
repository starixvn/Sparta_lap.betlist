namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class TurnoverByPlatformController : BaseDashboardApiController
    {
        private readonly IWidgetByPlatformService _turnoverByPlatformService;

        public TurnoverByPlatformController() : this(null)
        {
        }

        public TurnoverByPlatformController(IWidgetByPlatformService turnoverByPlatformService)
        {
            this._turnoverByPlatformService = turnoverByPlatformService ?? new TurnoverByPlatformService();
        }

        [HttpGet]
        public WidgetByPlatformViewModel Index(DashboardBaseModel metaModel)
        {
            return _turnoverByPlatformService.GetData(metaModel);
        }
    }
}