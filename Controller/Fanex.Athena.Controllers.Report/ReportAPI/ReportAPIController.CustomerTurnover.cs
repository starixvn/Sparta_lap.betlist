// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;

    public partial class ReportAPIController : DefaultCultureBaseController
    {
        [HttpPost]
        [AjaxRequest]
        [FunctionAuthorization(PermissionKeys.ReportCustomerTurnover)]
        [CustomValidateAntiForgery]
        public ActionResult GetBettypesBySport(IList<SportProductItem> sports)
        {
            if (sports == null || sports.Count == 0)
            {
                return Json(new List<SportProductItem>());
            }

            var data = _betTypeService.GetBettypes(SessionService.LicenseeSession.LicenseeId, sports, SessionService.UserSession.Language);

            return Json(data);
        }

        [HttpPost]
        [FunctionAuthorization(PermissionKeys.ReportCustomerTurnover)]
        [CustomValidateAntiForgery]
        public ActionResult CustomerTurnover(CustomerTurnoverMetaModel metaModel)
        {
            metaModel.IsExportExcel = false;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var result = _customerTurnoverService.GetDataList(metaModel, SessionService.UserSession.Language);

            return Json(new { Data = result.DataList, Total = result.TotalData, TotalRow = result.TotalRows });
        }
    }
}