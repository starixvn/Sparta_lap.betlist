// -----------------------------------------------------------------------
// <copyright file="ReportController.WinLossByBetType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;

    [FunctionAuthorization(PermissionKeys.ReportWinLossByBetType)]
    public class WinLossByBetTypeReportController : ReportBaseController
    {
        private readonly IWinLossByBetTypeDataAccess _winLossByBetTypeDataAccess;

        public WinLossByBetTypeReportController()
        {
            _winLossByBetTypeDataAccess = new WinLossByBetTypeDataAccess();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var balanceUp = GetBalanceUp();
            var metaModel = new WinLossByBetTypeMetaModel
            {
                BalanceUpDay = balanceUp.BalanceUpDay,
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new WinLossByBetTypeViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                BalanceUpdate = balanceUp.BalanceUpDay.ToShortDateString(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
            };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(WinLossByBetTypeMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var reportData = metaModel.IsValid
                       ? _winLossByBetTypeDataAccess.GetDataList(metaModel)
                       : new SortableList<WinLossByBetType>();

            reportData.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase));
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);
            var total = GetTotal(reportData);
            var model = new WinLossByBetTypeViewModel
            {
                Param = metaModel,
                MainResult = reportData,
                BalanceUpdate = GetBalanceUp().BalanceUpDay.ToShortDateString(),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
                ColumnCount = GetColumnCount(productAccess, licenseeSession.IsAllowedCustomerComm),
                Total = total
            };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return metaModel.IsExportExcel ?
                Excel(model) :
                View("~/Views/Report/WinLossByBetType/Main.cshtml", model);
        }

        private static WinLossByBetType GetTotal(IEnumerable<WinLossByBetType> dataList)
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
            const string FileName = nameof(WinLossByBetType);

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

            var winLossByBetTypeExcel = new WinLossByBetTypeExcel(templatePath, currencyConvert, showingOption, viewModel);

            winLossByBetTypeExcel.Export();

            return new ExcelResult(winLossByBetTypeExcel.Workbook, FileName);
        }
    }
}