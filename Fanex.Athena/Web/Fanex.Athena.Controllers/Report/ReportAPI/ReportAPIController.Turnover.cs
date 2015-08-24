// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class ReportAPIController : DefaultCultureBaseController
    {
        private const string TurnoverName = "Turnover";

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult Turnover(TurnoverMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            metaModel.Username = string.IsNullOrWhiteSpace(metaModel.Username) ||
                metaModel.Username.Trim() == ViewResources.ExternalIdOrSystemId ? null : metaModel.Username;
            var turnoverService = new TurnoverService();
            var dataList = turnoverService.GetDataList(metaModel);

            return Json(new
            {
                TotalData = turnoverService.SumTotalData(dataList),
                Data = dataList
            });
        }
    }
}
