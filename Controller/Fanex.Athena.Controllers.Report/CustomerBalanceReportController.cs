// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerBalance.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Globalization;
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
    using Resources;
    using Service.Report.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportCustomerBalance)]
    public class CustomerBalanceReportController : ReportBaseController
    {
        private readonly ICustomerBalanceService _custBalanceService;

        public CustomerBalanceReportController()
        {
            _custBalanceService = new CustomerBalanceService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var metaModel = new CustomerBalanceMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                CurrencyId = 0
            };

            var productAccess = ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.CustomerBalance);
            var productOption = productAccess.ProductList.Where(i => i.ProductCode != Product.BA)
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            metaModel.ProductOptions = productOption;
            var model = new CustomerBalanceViewModel
            {
                ShowEmptyContent = true,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                CurrencyList = userSession.CurrencyList,
                Param = metaModel,
                IsValidTimeRequest = true
            };

            var orderItemList = GenerateOrderItemList(metaModel.OrderItem);
            ViewBag.TopCountList = GenerateTopCountList(metaModel.TopCount);
            ViewBag.OrderItemList = orderItemList;
            var firstItem = orderItemList.FirstOrDefault(i => i.Value == metaModel.OrderItem.ToString(CultureInfo.InvariantCulture));
            if (firstItem != null)
            {
                this.ViewBag.SortByText = firstItem.Text;
            }

            return View("~/Views/Report/CustomerBalance/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(CustomerBalanceMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var totalData = new CustBalance();
            var totalRows = 0;
            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.CustomerBalance);
            var model = new CustomerBalanceViewModel
            {
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                CurrencyList = userSession.CurrencyList,
                Param = metaModel
            };
            var orderItemList = GenerateOrderItemList(metaModel.OrderItem);
            ViewBag.TopCountList = GenerateTopCountList(metaModel.TopCount);
            ViewBag.OrderItemList = orderItemList;
            var firstOrDefault = orderItemList.FirstOrDefault(i => i.Value == metaModel.OrderItem.ToString(CultureInfo.InvariantCulture));
            if (firstOrDefault != null)
            {
                this.ViewBag.SortByText = firstOrDefault.Text;
            }

            var isExportedExcel = Request.Form["exporttoexcel.x"];
            if (metaModel.FromDate.AddMonths(2) < metaModel.ToDate)
            {
                model.IsValidTimeRequest = false;
                return View("~/Views/Report/CustomerBalance/Main.cshtml", model);
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);
            if (isExportedExcel != null)
            {
                metaModel.PageSize = PageSizeExcel;
                var dataListExcel = _custBalanceService.GetDataList(metaModel, ref totalData, ref totalRows);
                model.DataList = dataListExcel;
                model.TotalData = totalData;
                return Excel(model);
            }

            var dataList = _custBalanceService.GetDataList(metaModel, ref totalData, ref totalRows);
            ViewBag.TotalRows = totalRows;
            model.DataList = dataList;
            model.TotalData = totalData;
            model.IsValidTimeRequest = true;

            return View("~/Views/Report/CustomerBalance/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(CustomerBalanceViewModel viewModel)
        {
            const string FileName = "CustomerBalance";

            var templatePath = Server.MapPath(@"~/App_Data/Template/CustomerReports/CustomerBalance.xls");

            var productAccess = ProductService.GetSingleWalletProductAccess(
                viewModel.Param.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.CustomerBalance);

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                LCVisible = productAccess.IsAllowLC,
                SBVisible = productAccess.IsAllowSB,
                CSVisible = productAccess.IsAllowCS,
                KNVisible = productAccess.IsAllowKN,
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

        private static IList<SelectListItem> GenerateTopCountList(int topCount)
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

        private static IEnumerable<SelectListItem> GenerateOrderItemList(short orderItem)
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
                        },
                    new SelectListItem
                        {
                            Value = "6",
                            Text = ViewResources.OpeningBalance,
                            Selected = orderItem == 6,
                        },
                    new SelectListItem
                        {
                            Value = "7",
                            Text = ViewResources.ClosingBalance,
                            Selected = orderItem == 7,
                        }
                };
            return result;
        }
    }
}