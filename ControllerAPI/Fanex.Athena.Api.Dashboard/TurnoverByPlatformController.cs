namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Model.Dashboard.MetaModel;

    public class TurnoverByPlatformController : BaseDashboardApiController
    {
        private readonly IWidgetByPlatformDataAccess _dataAccess;

        public TurnoverByPlatformController() : this(null)
        {
        }

        public TurnoverByPlatformController(IWidgetByPlatformDataAccess turnoverByPlatformService)
        {
            this._dataAccess = turnoverByPlatformService ?? new TurnoverByPlatformDataAccess();
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
            return _dataAccess.GetData(metaModel);
        }
    }
}