namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Model.Dashboard.MetaModel;

    public class TurnoverByPlatformController : BaseDashboardApiController
    {
        private readonly ILicenseeDataByPlatformDataAccess _dataAccess;
        
        public TurnoverByPlatformController(ILicenseeDataByPlatformDataAccess licenseeDataByPlatformService)
        {
            this._dataAccess = licenseeDataByPlatformService;
        }

        [HttpGet]
        public async Task<WidgetByPlatformViewModel> GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new WidgetByPlatformViewModel();
            }

            AssignAuthProperties(metaModel);
            var now = DateTime.Now.Date;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            return await _dataAccess.GetTurnover(metaModel).ConfigureAwait(false);
        }
    }
}