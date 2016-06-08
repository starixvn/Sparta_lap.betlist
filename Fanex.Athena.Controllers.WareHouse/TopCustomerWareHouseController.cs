// -----------------------------------------------------------------------
// <copyright file="TopCustomerWareHouseController.cs" company="nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Warehouse;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Fanex.Athena.Model.Warehouse.ViewModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Service.Warehouse;
    using Service.Warehouse.Interfaces;

    public class TopCustomerWareHouseController : WareHouseBaseController
    {
        private readonly ISportTypeService _sportTypeService;

        private readonly ITopCustomerService _topCustomerService;

        public TopCustomerWareHouseController()
        {
            _sportTypeService = new SportTypeService();
            _topCustomerService = new TopCustomerService();
        }

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var licenseeId = licenseeSession.LicenseeId;
            var metaModel = new TopCustomerMetaModel { LicenseeId = licenseeId };
            metaModel.FromDate = metaModel.ToDate = BalanceUpService.BalanceUpInfo.MoveBetsDay;
            var currencyList = userSession.CurrencyList;
            var sportList = _sportTypeService.GetAllWHSports();
            var productAccess = ProductService.GetProductAccess(licenseeId, userSession.Language);

            var model = new TopCustomerViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                ProductAccess = productAccess,
                CurrencyList = currencyList,
                SportList = sportList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            ViewBag.MaxDate = BalanceUpService.BalanceUpInfo.MoveBetsDay;
            return View("~/Views/WareHouse/TopCustomer/Main.cshtml", model);
        }

        [HttpPost]
        [FunctionAuthorization]
        public ActionResult Index(TopCustomerMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;

            if (metaModel.IsExportExcel)
            {
                return Excel(metaModel);
            }

            var model = new TopCustomerViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm,
                SportList = _sportTypeService.GetAllWHSports(),
            };

            var totalValues = new Dictionary<string, decimal>();

            ViewBag.BalanceUpDay = BalanceUpService.BalanceUpInfo.BalanceUpDay.ToShortDateString();
            model.MainResult = _topCustomerService.GetTopCustomerList(metaModel, ref totalValues);
            model.TotalValues = totalValues;

            ViewBag.MaxDate = BalanceUpService.BalanceUpInfo.MoveBetsDay;
            return View("~/Views/WareHouse/TopCustomer/Main.cshtml", model);
        }

        [HttpPost]
        public ActionResult Excel(TopCustomerMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            const string FileName = "TopCustomer";
            const string FormatTitle = "{0} {1} {2} {3} {4} --> {5} - {6}";
            metaModel.UserId = userSession.UserId;
            var templatePath = Server.MapPath("~/App_Data/Template/DataWareHouse/TopCustomer.xls");
            var productAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language);
            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB
            };
            var currencyConvert = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };
            var viewByCurrency =
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
            var titleText = string.Format(
                FormatTitle,
                ViewResources.Top,
                metaModel.TopRow,
                ViewResources.Customer,
                ViewResources.From,
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                viewByCurrency);

            var exportData = _topCustomerService.GetDataForExcel(metaModel);

            var topCustExcel = new TopCustomer(templatePath, FileName, currencyConvert, showingOption, exportData)
            {
                ReportTitle = titleText
            };

            topCustExcel.Export();
            return new ExcelResult(topCustExcel.Workbook, FileName);
        }
    }
}