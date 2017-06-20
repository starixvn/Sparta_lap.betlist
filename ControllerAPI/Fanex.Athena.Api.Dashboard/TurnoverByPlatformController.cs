namespace Fanex.Athena.Api.Dashboard
{
    using System;
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
        public WidgetByPlatformViewModel GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new WidgetByPlatformViewModel();
            }

            AssignAuthProperties(metaModel);
            var now = DateTime.Now.Date;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            return _turnoverByPlatformService.GetData(metaModel);
        }
    }
}