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
    using System.Web.Hosting;
    using System.Web.Mvc;
    using DataAccess.Core.Configuration.Interfaces;
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
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportWinLossByBetType)]
    public class WinLossByBetTypeReportController : ReportBaseController
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IWinLossByBetTypeDataAccess _winLossByBetTypeDataAccess;

        public WinLossByBetTypeReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            IWinLossByBetTypeDataAccess winLossByBetTypeDataAccess)
            : base(languageService, sessionService, appSetting, productService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _winLossByBetTypeDataAccess = winLossByBetTypeDataAccess;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new WinLossByBetTypeMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId
            };

            SetConvetedUSDCurrencyName(metaModel);

            var model = new WinLossByBetTypeViewModel
            {
                BalanceUpDay = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay,
                ShowEmptyContent = true,
                Param = metaModel,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language),
            };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return View("~/Views/Desktop/Report/WinLossByBetType/Main.cshtml", model);
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
                BalanceUpDay = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay,
                Param = metaModel,
                MainResult = reportData,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
                ColumnCount = GetColumnCount(productAccess, licenseeSession.IsAllowedCustomerComm),
                Total = total
            };

            ViewBag.IsShowCustComm = licenseeSession.IsAllowedCustomerComm;

            return metaModel.IsExportExcel ?
                Excel(model) :
                View("~/Views/Desktop/Report/WinLossByBetType/Main.cshtml", model);
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

            if (isAllowedCustomerComm)
            {
                columnCount += 2;
            }

            return columnCount;
        }

        private ActionResult Excel(WinLossByBetTypeViewModel viewModel)
        {
            const string FileName = nameof(WinLossByBetType);

            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/WinLossByBetType.xls");

            var currencyConvert = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var productAccess = ProductService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB,
                VSVisible = productAccess.IsAllowVS,
                CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            var excel = CreateExcel(templatePath, viewModel, currencyConvert, showingOption);

            excel.Export();

            return new ExcelResult(excel.Workbook, FileName);
        }

        protected internal virtual WinLossByBetTypeExcel CreateExcel(
            string templatePath,
            WinLossByBetTypeViewModel viewModel,
            ConvertingOption currencyConvert,
            ShowingOption showingOption)
        {
            return new WinLossByBetTypeExcel(templatePath, currencyConvert, showingOption, viewModel);
        }
    }
}