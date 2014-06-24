// -----------------------------------------------------------------------
// <copyright file="ReportController.RacingCommission.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// The racing commission report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult RacingCommission()
        {
            var metaModel = new RacingCommissionMetaModel
                {
                    BalanceUpDay = _balanceUpService.BalanceUpInfo.BalanceUpDay
                };

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            SetConvetedUSDCurrencyName(metaModel);

            var model = new RacingCommissionViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    CurrencyList = userSession.CurrencyList,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                };

            return View("~/Views/Report/RacingCommission/Main.cshtml", model);
        }

        /// <summary>
        /// The racing commission report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult RacingCommission(RacingCommissionMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            if (metaModel.IsExportExcel)
            {
                metaModel.PageSize = PageSizeExcel;
            }

            var total = new RacingCommission();
            var racingCommissionService = new RacingCommissionService();
            var reportData = racingCommissionService.GetDataList(metaModel, ref total);

            reportData.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));

            var model = new RacingCommissionViewModel
                {
                    RacingCommissionList = reportData,
                    Param = metaModel,
                    LicenseeTypeList = licenseeSession.LicenseeType,
                    CurrencyList = userSession.CurrencyList,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    Total = total,
                };

            return metaModel.IsExportExcel
                       ? RacingCommissionExcel(model)
                       : View("~/Views/Report/RacingCommission/Main.cshtml", model);
        }

        #region Export Excel

        /// <summary>
        /// The racing commission excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        private ActionResult RacingCommissionExcel(RacingCommissionViewModel viewModel)
        {
            const string FileName = "RacingCommission";

            var currencyConvert = new ConvertingOption
                {
                    IsConvert = viewModel.Param.IsConvert,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.IsConvert ? string.Empty : viewModel.Param.CurrencyName
                };

            var showingOption = new ShowingOption
                {
                    CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm
                };

            var templatePath = Server.MapPath("~/App_Data/Template/Reports/RacingCommission.xls");

            var racingWinLossExcel = new RacingCommissionExcel(
                    templatePath,
                    FileName,
                    currencyConvert,
                    showingOption,
                    viewModel);
            racingWinLossExcel.Export();
            return new ExcelResult(racingWinLossExcel.Workbook, FileName);
        }

        #endregion Export Excel
    }
}