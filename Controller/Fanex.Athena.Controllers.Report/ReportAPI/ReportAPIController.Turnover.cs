// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Resources;

    public partial class ReportAPIController : DefaultCultureBaseController
    {
        [HttpPost]
        [FunctionAuthorization(PermissionKeys.ReportTurnover)]
        [CustomValidateAntiForgery]
        public ActionResult Turnover(TurnoverMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

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