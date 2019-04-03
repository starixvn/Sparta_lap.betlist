namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;

    public class LicenseeWinLossByPlatformController : BaseDashboardApiController
    {
        private readonly ILicenseeDataByPlatformDataAccess _dataAccess;

        public LicenseeWinLossByPlatformController(ILicenseeDataByPlatformDataAccess dataAccess)
        {
            this._dataAccess = dataAccess;
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
            return await _dataAccess.GetWinLoss(metaModel).ConfigureAwait(false);
        }
    }
}