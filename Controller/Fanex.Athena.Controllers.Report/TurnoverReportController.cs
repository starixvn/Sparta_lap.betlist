// -----------------------------------------------------------------------
// <copyright file="ReportController.Turnover.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Service.Core.Utility;

    [FunctionAuthorization(PermissionKeys.ReportTurnover)]
    public class TurnoverReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new TurnoverMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId,
            };

            SetConvetedUSDCurrencyName(metaModel);
            SetConvetedUSDCurrencyId(metaModel);

            var productAccess = ProductService.GetProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.Turnover);
            var productOption = productAccess.ProductList.Where(a => a.ProductCode.Equals(Product.BA) == false)
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            metaModel.ProductOptions = productOption;

            var model = new TurnoverViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                LicenseeTypeList = licenseeSession.LicenseeType,
                CurrencyList = GetCurrenciesForTurnoverReport(licenseeSession.LicenseeId),
                IsAllowedCasinoTransfer = licenseeSession.LicenseeId == 1
            };

            return View("~/Views/Report/Turnover/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(TurnoverMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            metaModel.Username = string.IsNullOrWhiteSpace(metaModel.Username) ||
                metaModel.Username.Trim() == ViewResources.ExternalIdOrSystemId ? null : metaModel.Username;
            var turnoverService = new TurnoverService();
            var dataList = turnoverService.GetDataList(metaModel);

            var model = new TurnoverViewModel
            {
                DataList = dataList,
                TotalData = turnoverService.SumTotalData(dataList),
                Param = metaModel,
                ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.Turnover),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                LicenseeTypeList = licenseeSession.LicenseeType,
                CurrencyList = GetCurrenciesForTurnoverReport(licenseeSession.LicenseeId),
                IsAllowedCasinoTransfer = licenseeSession.LicenseeId == 1
            };

            return ExportExcel(model);
        }

        private ActionResult ExportExcel(TurnoverViewModel viewModel)
        {
            const string FileName = "Turnover";

            var templatePath = Server.MapPath("~/App_Data/Template/Reports/Turnover.xls");

            var productAccess = ProductService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language, FunctionNames.Turnover);

            var convertingOption = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm,
                BAVisible = productAccess.IsAllowBA
            };

            viewModel.Param.CurrencyName = viewModel.Param.IsConvert ?
                viewModel.Param.BaseCurrency
                : viewModel.CurrencyList.FirstOrDefault(a => a.Value == viewModel.Param.CurrencyId).Key;

            var turnoverExcel = new TurnoverExcel(templatePath, FileName, convertingOption, showingOption, viewModel) { LicenseeId = viewModel.Param.LicenseeId };

            turnoverExcel.Export();

            return new ExcelResult(turnoverExcel.Workbook, FileName);
        }

        private IDictionary<string, int> GetCurrenciesForTurnoverReport(int licenseeId)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            KeanOceanBusinessHelper.AddHKDCurrencyForTurnOverReport(currencyList, licenseeId);
            return currencyList;
        }
    }
}