// -----------------------------------------------------------------------
// <copyright file="WinLossWareHouseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.Warehouse;
    using DataAccess.Warehouse.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Warehouse;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Fanex.Athena.Model.Warehouse.ViewModel;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Model.Warehouse.DTO;
    using Resources;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.WarehouseWinLoss)]
    public class WinLossWareHouseController : WareHouseBaseController
    {
        private readonly IWinLossDataAccess _winLossDataAccess;

        public WinLossWareHouseController()
        {
            _winLossDataAccess = new WinLossDataAccess();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var licenseeId = licenseeSession.LicenseeId;
            var currencyList = userSession.CurrencyList;
            var metaModel = new WinLossMetaModel { LicenseeId = licenseeId };
            metaModel.FromDate = metaModel.ToDate = GetBalanceUp().MoveBetsDay;

            var productAccess = ProductService.GetProductAccess(licenseeId, userSession.Language);

            var model = new WinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = currencyList,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            ViewBag.MaxDate = GetBalanceUp().MoveBetsDay;
            return View("~/Views/WareHouse/WinLoss/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(WinLossMetaModel metaModel)
        {
            const string FileName = "WinLoss";
            const string FormatTitle = "{0} {1} {2} {3} --> {4} - {5}";
            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            var templatePath = Server.MapPath("~/App_Data/Template/DataWareHouse/WinLoss.xls");
            var showingOption = GetShowingOption();
            var viewByCurrency =
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName);
            var groupingList = GroupingMetaModel.GroupingList;
            var titleText = string.Format(
                FormatTitle,
                ViewResources.ReportBy,
                 groupingList[metaModel.ReportBy.ToString(CultureInfo.InvariantCulture)],
                  ViewResources.From,
                 metaModel.FromDate.ToString("MM/dd/yyyy"),
                 metaModel.ToDate.ToString("MM/dd/yyyy"),
                 viewByCurrency);
            IDictionary<string, decimal> totalValues = new Dictionary<string, decimal>();
            var exportData = GetDataList(metaModel, ref totalValues);

            var winLossExcel = new TopWinLossExcel(templatePath, showingOption, exportData, totalValues, titleText)
            {
                GroupingId = metaModel.ReportBy
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

            model.MainResult = GetDataList(metaModel, ref totalValues);
            model.TotalValues = totalValues;

            ViewBag.MaxDate = GetBalanceUp().MoveBetsDay;
            return View("~/Views/WareHouse/WinLoss/Main.cshtml", model);
        }

        private static IDictionary<string, decimal> GetWinlossTotalInfo(IEnumerable<WinLoss> dataList)
        {
            var totalValues = new Dictionary<string, decimal>
                    {
                        { "betcount", 0 },
                        { "turnover", 0 },
                        { "memberwinloss", 0 },
                        { "memberwinlosscomm", 0 },
                        { "licenseewinloss", 0 },
                        { "bacomm", 0 },
                        { "totalrows", 0 }
                    };

            if (dataList != null)
            {
                foreach (var entity in dataList)
                {
                    totalValues["betcount"] += entity.BetCount;
                    totalValues["turnover"] += entity.Turnover;
                    totalValues["memberwinloss"] += entity.MemberWinLoss;
                    totalValues["memberwinlosscomm"] += entity.MemberWinLossCom;
                    totalValues["licenseewinloss"] += entity.LicenseeWinloss;
                    totalValues["bacomm"] += entity.BAComm;
                }

                totalValues["totalrows"] = dataList.Count();
            }

            return totalValues;
        }

        private IList<WinLoss> GetDataList(WinLossMetaModel model, ref IDictionary<string, decimal> totalValues)
        {
            var dataList = model.IsValid
                               ? this._winLossDataAccess.GetDataList(model)
                               : new List<WinLoss>();
            totalValues = GetWinlossTotalInfo(dataList);

            return dataList.OrderBy(x => x.GroupByString).ToList();
        }
    }
}