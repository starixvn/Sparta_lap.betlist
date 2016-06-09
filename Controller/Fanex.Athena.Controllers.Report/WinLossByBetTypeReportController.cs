// -----------------------------------------------------------------------
// <copyright file="ReportController.WinLossByBetType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DTO.Common;

    public class WinLossByBetTypeReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new WinLossByBetTypeMetaModel
            {
                BalanceUpDay = BalanceUpService.BalanceUpInfo.BalanceUpDay,
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new WinLossByBetTypeViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                BalanceUpdate = BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = GetUserCurrencies(),
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
            };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult Index(WinLossByBetTypeMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = licenseeSession.UserId;
            var winLossByBetTypeService = new WinLossByBetTypeService();
            var reportData = winLossByBetTypeService.GetDataList(metaModel);

            reportData.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            var total = GetTotal(reportData);
            var model = new WinLossByBetTypeViewModel
            {
                Param = metaModel,
                MainResult = reportData,
                BalanceUpdate = BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = GetUserCurrencies(),
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = productAccess,
                ColumnCount = GetColumnCount(productAccess, licenseeSession.IsAllowedCustomerComm),
                Total = total
            };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return metaModel.IsExportExcel ?
                Excel(model) :
                View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        private static WinLossByBetType GetTotal(IList<WinLossByBetType> dataList)
        {
            return new WinLossByBetType
            {
                Turnover = dataList.Sum(item => item.Turnover),
                CustomerWinLoss = dataList.Sum(item => item.CustomerWinLoss),
                CustomerComm = dataList.Sum(item => item.CustomerComm),
                BACommission = dataList.Sum(item => item.BACommission),
                AmtDueToBA = dataList.Sum(item => item.AmtDueToBA),
                RacingRevenue = dataList.Sum(item => item.RacingRevenue),
                RBComm = dataList.Sum(item => item.RBComm),
                AmtDueToRacing = dataList.Sum(item => item.AmtDueToRacing),
                LicenseeWL = dataList.Sum(i => i.LicenseeWL),
            };
        }

        private static int GetColumnCount(ProductAccess productAccess, bool isAllowedCustomerComm)
        {
            var columnCount = 6;
            if (productAccess.IsAllowBA)
            {
                columnCount += 2;
            }

            if (productAccess.IsAllowRB)
            {
                columnCount += 2;
            }

            if (isAllowedCustomerComm)
            {
                columnCount += 2;
            }

            return columnCount;
        }

        private ActionResult Excel(WinLossByBetTypeViewModel viewModel)
        {
            const string FileName = "WinLossByBetType";

            var templatePath = Server.MapPath("~/App_Data/Template/Reports/WinLossByBetType.xls");

            var currencyConvert = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var productAccess = ProductService.GetProductAccess(viewModel.Param.LicenseeId, SessionService.UserSession.Language);

            var showingOption = new ShowingOption
            {
                RBVisible = productAccess.IsAllowRB,
                BAVisible = productAccess.IsAllowBA,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB,
                LCVisible = productAccess.IsAllowLC,
                VSVisible = productAccess.IsAllowVS,
                CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            var winLossByBetTypeExcel = new WinLossByBetTypeExcel(templatePath, FileName, currencyConvert, showingOption, viewModel);

            winLossByBetTypeExcel.Export();

            return new ExcelResult(winLossByBetTypeExcel.Workbook, FileName);
        }
    }
}