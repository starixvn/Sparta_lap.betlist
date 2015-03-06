// -----------------------------------------------------------------------
// <copyright file="ReportController.Turnover.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    /// The turn over controller.
    /// </summary>
    public partial class ReportController
    {
        #region Web Controller

        /// <summary>
        ///    Turn over report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Turnover()
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

            var productAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, IgnoreBingoAndAgCasinoAndPlaytech);
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
                    CurrencyList = userSession.CurrencyList,
                    IsAllowedCasinoTransfer = licenseeSession.LicenseeId == 1
                };

            return View("~/Views/Report/Turnover/Main.cshtml", model);
        }

        /// <summary>
        /// Turn over report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
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

            if (metaModel.IsExportExcel)
            {
                var model = new TurnoverViewModel
                    {
                        DataList = dataList,
                        TotalData = turnoverService.SumTotalData(dataList),
                        Param = metaModel,
                        ProductAccess = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, IgnoreBingoAndPlaytech),
                        IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                        LicenseeTypeList = licenseeSession.LicenseeType,
                        CurrencyList = userSession.CurrencyList,
                        IsAllowedCasinoTransfer = licenseeSession.LicenseeId == 1
                    };

                return TurnoverExcel(model);
            }
            else
            {
                return Json(new
                {
                    TotalData = turnoverService.SumTotalData(dataList),
                    Data = dataList
                });
            }
        }

        #endregion Web Controller

        #region Export excel

        /// <summary>
        /// Turn over excel report.
        /// </summary>
        /// <param name="viewModel">The metal model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult TurnoverExcel(TurnoverViewModel viewModel)
        {
            const string FileName = "Turnover";

            var templatePath = Server.MapPath("~/App_Data/Template/Reports/Turnover.xls");

            var productAccess = _productService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language, IgnoreBingoAndPlaytech);

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

        #endregion Export excel
    }
}