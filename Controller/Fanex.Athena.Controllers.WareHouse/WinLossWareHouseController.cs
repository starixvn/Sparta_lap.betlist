// -----------------------------------------------------------------------
// <copyright file="WinLossWareHouseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Warehouse;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Fanex.Athena.Model.Warehouse.ViewModel;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Resources;
    using Service.Warehouse;
    using Service.Warehouse.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.WarehouseWinLoss)]
    public class WinLossWareHouseController : WareHouseBaseController
    {
        private readonly IWinLossService _winLossService;

        public WinLossWareHouseController()
        {
            _winLossService = new WinLossService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var licenseeId = licenseeSession.LicenseeId;
            var currencyList = userSession.CurrencyList;
            var metaModel = new WinLossMetaModel { LicenseeId = licenseeId };
            metaModel.FromDate = metaModel.ToDate = BalanceUpService.BalanceUpInfo.MoveBetsDay;

            var productAccess = ProductService.GetProductAccess(licenseeId, userSession.Language);

            var model = new WinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = currencyList,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            ViewBag.MaxDate = BalanceUpService.BalanceUpInfo.MoveBetsDay;
            return View("~/Views/WareHouse/WinLoss/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(WinLossMetaModel metaModel)
        {
            const string FileName = "WinLoss";
            const string FormatTitle = "{0} {1} {2} {3} --> {4} - {5}";
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var templatePath = Server.MapPath("~/App_Data/Template/DataWareHouse/WinLoss.xls");
            var productAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language);
            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB
            };
            var viewByCurrency =
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);
            var groupingList = GroupingMetaModel.GetCollection();
            var titleText = string.Format(
                FormatTitle,
                ViewResources.ReportBy,
                 groupingList[metaModel.ReportBy.ToString(CultureInfo.InvariantCulture)],
                  ViewResources.From,
                 metaModel.FromDate.ToString("MM/dd/yyyy"),
                 metaModel.ToDate.ToString("MM/dd/yyyy"),
                 viewByCurrency);
            IDictionary<string, decimal> totalValues = new Dictionary<string, decimal>();
            var exportData = _winLossService.GetDataList(metaModel, ref totalValues);

            var currencyConvert = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };
            var winLossExcel = new TopWinLoss(templatePath, FileName, currencyConvert, showingOption, exportData, totalValues)
            {
                GroupingId = metaModel.ReportBy,
                ReportTitle = titleText
            };

            winLossExcel.Export();
            return new ExcelResult(winLossExcel.Workbook, FileName);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(WinLossMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            if (metaModel.IsExportExcel)
            {
                return Excel(metaModel);
            }

            var model = new WinLossViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            IDictionary<string, decimal> totalValues = new Dictionary<string, decimal>();

            model.MainResult = _winLossService.GetDataList(metaModel, ref totalValues);
            model.TotalValues = totalValues;

            ViewBag.MaxDate = BalanceUpService.BalanceUpInfo.MoveBetsDay;
            return View("~/Views/WareHouse/WinLoss/Main.cshtml", model);
        }
    }
}