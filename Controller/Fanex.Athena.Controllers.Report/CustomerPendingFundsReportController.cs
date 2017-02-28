// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerPendingFunds.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportCustomerPendingFunds)]
    public class CustomerPendingFundsReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new CustomerPendingFundsMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId
            };
            SetConvetedUSDCurrencyName(metaModel);
            var productAccess = ProductService.GetSingleWalletProductAccess(
                metaModel.LicenseeId,
                userSession.Language,
                FunctionNames.CustomerPendingFunds);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            metaModel.ProductOptions = productOption;

            var model = new CustomerPendingFundsViewModel
            {
                ShowEmptyContent = true,
                DataList = new List<CustPendingFunds>(),
                TotalData = new CustPendingFunds(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                CurrencyList = userSession.CurrencyList,
                Param = metaModel,
                TotalRows = 0,
            };

            return View("~/Views/Report/CustomerPendingFunds/Main.cshtml", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CustomValidateAntiForgery]
        public ActionResult Index(CustomerPendingFundsMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = userSession.UserId;

            var totalData = new CustPendingFunds();
            var totalRows = 0;

            var model = new CustomerPendingFundsViewModel
            {
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = ProductService.GetSingleWalletProductAccess(metaModel.LicenseeId, userSession.Language, FunctionNames.CustomerPendingFunds),
                CurrencyList = userSession.CurrencyList,
                Param = metaModel,
                TotalRows = totalRows,
            };
            if (metaModel.IsExportExcel)
            {
                metaModel.PageSize = PageSizeExcel;
            }

            var custPendingFundsService = new CustomerPendingFundsService();
            var dataList = custPendingFundsService.GetDataList(metaModel, ref totalData, ref totalRows);
            dataList.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            model.TotalRows = totalRows;
            model.DataList = dataList;
            model.TotalData = totalData;

            if (metaModel.IsExportExcel)
            {
                return Excel(model);
            }

            model.DataList = dataList;
            model.TotalData = totalData;

            return View("~/Views/Report/CustomerPendingFunds/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(CustomerPendingFundsViewModel viewModel)
        {
            const string FileName = "CustomerPendingFunds";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerPendingFunds.xls");
            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException(templatePath);
            }

            var convertingOption = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var custPendingFundsExcel = new CustomerPendingFundsExcel(templatePath, FileName, convertingOption, viewModel);
            custPendingFundsExcel.Export();

            return new ExcelResult(custPendingFundsExcel.Workbook, FileName);
        }
    }
}