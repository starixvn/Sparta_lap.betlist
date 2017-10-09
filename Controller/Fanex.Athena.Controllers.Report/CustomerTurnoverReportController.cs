// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerTurnover.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Newtonsoft.Json;
    using Resources;
    using Service.Report.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportCustomerTurnover)]
    public class CustomerTurnoverReportController : ReportBaseController
    {
        private readonly ISportTypeDataAccess _sportTypeDataAccess;

        private readonly ICustomerTurnoverService _customerTurnoverService;

        public CustomerTurnoverReportController()
        {
            _sportTypeDataAccess = new SportTypeDataAccess();
            _customerTurnoverService = new CustomerTurnoverService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var model = new CustomerTurnoverViewModel
            {
                Param = new CustomerTurnoverMetaModel { LicenseeId = licenseeSession.LicenseeId },
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = SessionService.UserSession.CurrencyList,
            };

            return View("~/Views/Report/CustomerTurnover/Main.cshtml", model);
        }

        [HttpGet]
        [AjaxRequest]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        public ActionResult GetSportsAndProducts()
        {
            return Json(
                _sportTypeDataAccess.GetSportsAndProducts(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        [ValidateInput(false)]
        [CustomValidateAntiForgery]
        public ActionResult Excel(string excelParam)
        {
            const string FileName = "CustomerTurnover";

            if (string.IsNullOrEmpty(excelParam))
            {
                return null;
            }

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = JsonConvert.DeserializeObject<CustomerTurnoverMetaModel>(excelParam);
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            if (!metaModel.IsValid)
            {
                Response.Redirect("~/Utility/Error?errCode=400");
            }

            metaModel.IsExportExcel = true;

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerTurnover.xls");
            var formatDateTime = metaModel.ReportBy == 1 ? "MM/dd/yyyy" : "MM/yyyy";

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };
            var currencyName = userSession.CurrencyList
                .Where(i => i.Key == metaModel.CurrencyId).Select(i => i.Value).SingleOrDefault();

            var result = _customerTurnoverService.GetDataList(metaModel, userSession.Language);

            var usernameExcelTitle = string.IsNullOrEmpty(metaModel.Username) ?
                string.Empty
                : "- " + ViewResources.Username + ": " + metaModel.Username;
            var excel = new CustomerTurnoverExcel(templatePath, showingOption, result.DataList, result.TotalData, formatDateTime)
            {
                ReportTitle = string.Format(
                "{0}, {1} --> {2} - {3}: {4} - {5}: {6} {7}",
                                            ViewResources.Customer + " " + ViewResources.Turnover,
                                            metaModel.FromDate.ToShortDateString(),
                                            metaModel.ToDate.ToShortDateString(),
                                            ViewResources.Currency,
                                            currencyName,
                                            ViewResources.MinimumOdds,
                                            metaModel.Odds,
                                            usernameExcelTitle)
            };
            excel.Export();
            return new ExcelResult(excel.Workbook, FileName);
        }
    }
}