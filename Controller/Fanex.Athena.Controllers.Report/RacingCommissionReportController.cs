// -----------------------------------------------------------------------
// <copyright file="ReportController.RacingCommission.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportRacingCommission)]
    public class RacingCommissionReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = new RacingCommissionMetaModel
            {
                BalanceUpDay = BalanceUpService.BalanceUpInfo.BalanceUpDay
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
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
            };

            return View("~/Views/Report/RacingCommission/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(RacingCommissionMetaModel metaModel)
        {
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
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
                CurrencyList = SessionService.UserSession.CurrencyList,
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                Total = total,
            };

            return metaModel.IsExportExcel
                       ? Excel(model)
                       : View("~/Views/Report/RacingCommission/Main.cshtml", model);
        }

        private ActionResult Excel(RacingCommissionViewModel viewModel)
        {
            const string FileName = nameof(RacingCommission);

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
    }
}