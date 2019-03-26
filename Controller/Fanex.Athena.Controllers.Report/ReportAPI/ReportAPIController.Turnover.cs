// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Resources;

    public partial class ReportAPIController : NonCultureBaseController
    {
        [HttpPost]
        [FunctionAuthorization(PermissionKeys.ReportTurnover)]
        [CustomValidateAntiForgery]
        public ActionResult Turnover(TurnoverMetaModel metaModel)
        {
            var placeholderUsername = _appSettings.FakeLicenseeList.Contains(SessionService.LicenseeSession.LicenseeId) ?
                ViewResources.ExternalId :
                ViewResources.ExternalIdOrSystemId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);

            metaModel.Username = string.IsNullOrWhiteSpace(metaModel.Username) ||
                metaModel.Username.Trim() == placeholderUsername ? null : metaModel.Username;
            var dataList = _turnoverService.GetDataList(metaModel);

            return Json(new
            {
                TotalData = _turnoverService.SumTotalData(dataList),
                Data = dataList
            });
        }
    }
}