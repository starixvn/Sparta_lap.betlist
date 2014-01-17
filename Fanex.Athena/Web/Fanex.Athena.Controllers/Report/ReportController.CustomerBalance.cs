// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerBalance.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    /// Customer balance controller.
    /// </summary>
    public partial class ReportController
    {
        #region Web Controller

        /// <summary>
        ///     The balance report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CustomerBalance()
        {
            var metaModel = new CustomerBalanceMetaModel
                {
                    LicenseeId = this.SessionService.LicenseeSession.LicenseeId,
                    CurrencyId = 0
                };

            var productAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            var productOption = productAccess.ProductList.Where(i => i.ProductCode != Product.Ba)
                                             .Select(i => new ProductOption
                                                 {
                                                     Code = i.ProductCode,
                                                     Name = i.ProductName,
                                                     Status = i.ProductCode == Product.Sb
                                                 }).ToList();

            metaModel.ProductOptions = productOption;
            var currencyList = SessionService.UserSession.CurrencyList;
            var model = new CustomerBalanceViewModel
                {
                    ShowEmptyContent = true,
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                    CurrencyList = currencyList,
                    Param = metaModel,
                    IsValidTimeRequest = true,
                };

            var orderItemList = GenerateOrderItemList(metaModel.OrderItem);
            ViewBag.TopCountList = GenerateTopCountList(metaModel.TopCount);
            ViewBag.OrderItemList = orderItemList;
            var firstItem = orderItemList.FirstOrDefault(i => i.Value == metaModel.OrderItem.ToString(CultureInfo.InvariantCulture));
            if (firstItem
                != null)
            {
                this.ViewBag.SortByText =
                    firstItem.Text;
            }

            return View("~/Views/Report/CustomerBalance/Main.cshtml", model);
        }

        /// <summary>
        /// Customer balance report.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization]
        public ActionResult CustomerBalance(CustomerBalanceMetaModel metaModel)
        {
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;
            var totalData = new CustBalance();
            var totalRows = 0;
            var model = new CustomerBalanceViewModel
                {
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    LicenseeTypeList = SessionService.LicenseeSession.LicenseeType,
                    ProductAccess = _productService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language),
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    Param = metaModel
                };

            var orderItemList = GenerateOrderItemList(metaModel.OrderItem);
            ViewBag.TopCountList = GenerateTopCountList(metaModel.TopCount);
            ViewBag.OrderItemList = orderItemList;
            var firstOrDefault = orderItemList.FirstOrDefault(i => i.Value == metaModel.OrderItem.ToString(CultureInfo.InvariantCulture));
            if (firstOrDefault
                != null)
            {
                this.ViewBag.SortByText =
                    firstOrDefault.Text;
            }

            var isExportedExcel = Request.Form["exporttoexcel.x"];

            if (metaModel.FromDate.AddMonths(2) < metaModel.ToDate)
            {
                model.IsValidTimeRequest = false;
                return View("~/Views/Report/CustomerBalance/Main.cshtml", model);
            }

            if (isExportedExcel != null)
            {
                metaModel.PageSize = PageSizeExcel;
                var dataListExcel = _custBalanceService.GetDataList(metaModel, ref totalData, ref totalRows);
                model.DataList = dataListExcel;
                model.TotalData = totalData;
                return CustomerBalanceExcel(model);
            }

            var dataList = _custBalanceService.GetDataList(metaModel, ref totalData, ref totalRows);
            ViewBag.TotalRows = totalRows;
            model.DataList = dataList;
            model.TotalData = totalData;
            model.IsValidTimeRequest = true;

            return View("~/Views/Report/CustomerBalance/Main.cshtml", model);
        }

        #endregion Web Controller

        #region Export excel

        /// <summary>
        /// Customer balance excel report.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The excel file.</returns>
        [HttpPost]
        public ActionResult CustomerBalanceExcel(CustomerBalanceViewModel viewModel)
        {
            const string FileName = "CustomerBalance";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerBalance.xls");

            var productAccess = _productService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language);

            var showingOption = new ShowingOption
                {
                    BaVisible = productAccess.IsAllowBa,
                    RbVisible = productAccess.IsAllowRb,
                    NgVisible = productAccess.IsAllowNg,
                    LcVisible = productAccess.IsAllowLc,
                    SbVisible = productAccess.IsAllowSB,
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm
                };

            var convertingOption = new ConvertingOption
                {
                    IsConvert = false,
                    BaseCurrency = viewModel.Param.BaseCurrency,
                    CurrencyName = viewModel.Param.CurrencyName
                };

            var custBalanceExcel = new CustomerBalanceExcel(templatePath, FileName, viewModel, showingOption, convertingOption, ViewBag.SortByText);
            custBalanceExcel.Export();

            return new ExcelResult(custBalanceExcel.Workbook, FileName);
        }

        #endregion Export excel

        #region Utilities

        /// <summary>
        /// Generate top count list.
        /// </summary>
        /// <param name="topCount">The current top count.</param>
        /// <returns>The select list item.</returns>
        private List<SelectListItem> GenerateTopCountList(int topCount)
        {
            const string Label100 = "100";
            const string Label500 = "500";
            var result = new List<SelectListItem>
                {
                    new SelectListItem
                        {
                            Value = "0",
                            Text = ViewResources.All,
                            Selected = topCount == 0,
                        },
                    new SelectListItem
                        {
                            Value = Label100,
                            Text = Label100,
                            Selected = topCount == 100,
                        },
                    new SelectListItem
                        {
                            Value = Label500,
                            Text = Label500,
                            Selected = topCount == 500,
                        }
                };

            return result;
        }

        /// <summary>
        /// Generate ordered item list.
        /// </summary>
        /// <param name="orderItem">The order item.</param>
        /// <returns>The select list item.</returns>
        private IEnumerable<SelectListItem> GenerateOrderItemList(short orderItem)
        {
            var result = new List<SelectListItem>
                {
                    new SelectListItem
                        {
                            Value = "1",
                            Text = ViewResources.Deposit,
                            Selected = orderItem == 1,
                        },
                    new SelectListItem
                        {
                            Value = "2",
                            Text = ViewResources.Withdrawal,
                            Selected = orderItem == 2,
                        },
                    new SelectListItem
                        {
                            Value = "3",
                            Text = ViewResources.Turnover,
                            Selected = orderItem == 3,
                        },
                    new SelectListItem
                        {
                            Value = "4",
                            Text = ViewResources.Win,
                            Selected = orderItem == 4,
                        },
                    new SelectListItem
                        {
                            Value = "5",
                            Text = ViewResources.Loss,
                            Selected = orderItem == 5,
                        }
                };
            return result;
        }

        #endregion Utilities
    }
}