// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerTurnover.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Newtonsoft.Json;

    /// <summary>
    /// The customer turnover controller.
    /// </summary>
    public partial class ReportController
    {
        #region Web Controller

        /// <summary>
        /// Customer turnover report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CustomerTurnover()
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

        #endregion Web Controller

        #region Export excel

        /// <summary>
        /// Customer turnover excel report.
        /// </summary>
        /// <param name="excelParam">The view model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        [ValidateInput(false)]
        public ActionResult CustomerTurnoverExcel(string excelParam)
        {
            const string FileName = "CustomerTurnover";

            if (string.IsNullOrEmpty(excelParam))
            {
                return null;
            }

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            CustomerTurnoverMetaModel metaModel = JsonConvert.DeserializeObject<CustomerTurnoverMetaModel>(excelParam);
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;
            if (metaModel == null || !metaModel.IsValid)
            {
                Response.Redirect("~/Utility/Error?errCode=400");
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.IsExportExcel = true;
            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerTurnover.xls");
            var formatDateTime = metaModel.ReportBy == 1 ? "MM/dd/yyyy" : "MM/yyyy";

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };
            var currencyName = userSession.CurrencyList
                .Where(i => i.Value == metaModel.CurrencyId).Select(i => i.Key).SingleOrDefault();
              
            CustTurnover totalData = new CustTurnover();
            var totalRow = 0;
            var data = GetData(metaModel, out totalData, out totalRow);
            var usernameExcelTitle = string.IsNullOrEmpty(metaModel.Username) ?
                string.Empty
                : "- " + ViewResources.Username + ": " + metaModel.Username;
            var excel = new CustomerTurnoverExcel(templatePath, FileName, showingOption, data, totalData, formatDateTime)
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

        #endregion Export excel

        #region Utilities

        private IList<CustTurnover> GetData(CustomerTurnoverMetaModel metalModel, out CustTurnover totalData, out int totalRow)
        {
            totalData = new CustTurnover();
            totalRow = 0;
            var custTurnoverService = new CustomerTurnoverService();

            return custTurnoverService.GetDataList(metalModel, SessionService.UserSession.Language, out totalData, out totalRow);
        }

        #endregion Utilities
    }
}