// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class ReportAPIController : DefaultCultureBaseController
    {
        private const string CustomerTurnoverReportUrl = "CustomerTurnoverReport";

        [HttpPost]
        [AjaxRequest]
        [FunctionAuthorization(ParentUrl = CustomerTurnoverReportUrl)]
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
        [FunctionAuthorization(ParentUrl = CustomerTurnoverReportUrl)]
        [CustomValidateAntiForgery]
        public ActionResult CustomerTurnover(CustomerTurnoverMetaModel metaModel)
        {
            var total = new CustTurnover();
            var totalRow = 0;
            IList<CustTurnover> data = new List<CustTurnover>();

            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            if (metaModel != null && metaModel.IsValid)
            {
                data = GetData(metaModel, out total, out totalRow);
            }

            return Json(new { Data = data, Total = total, TotalRow = totalRow });
        }

        private IList<CustTurnover> GetData(CustomerTurnoverMetaModel metalModel, out CustTurnover totalData, out int totalRow)
        {
            totalData = new CustTurnover();
            totalRow = 0;
            var custTurnoverService = new CustomerTurnoverService();

            return custTurnoverService.GetDataList(metalModel, SessionService.UserSession.Language, out totalData, out totalRow);
        }
    }
}