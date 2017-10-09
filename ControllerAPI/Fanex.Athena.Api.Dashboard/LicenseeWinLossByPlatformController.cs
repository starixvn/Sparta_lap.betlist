namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public class LicenseeWinLossByPlatformController : BaseDashboardApiController
    {
        private readonly IWidgetByPlatformDataAccess _dataAccess;

        public LicenseeWinLossByPlatformController() : this(null)
        {
        }

        public LicenseeWinLossByPlatformController(IWidgetByPlatformDataAccess dataAccess)
        {
            this._dataAccess = dataAccess ?? new LicenseeWinLossByPlatformDataAccess();
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