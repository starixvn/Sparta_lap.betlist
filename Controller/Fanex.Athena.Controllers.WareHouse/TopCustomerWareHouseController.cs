// -----------------------------------------------------------------------
// <copyright file="TopCustomerWareHouseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Warehouse;
    using DataAccess.Warehouse.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Warehouse;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Fanex.Athena.Model.Warehouse.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Model.Core.DataDomain;
    using Model.Warehouse.DTO;
    using Resources;

    [FunctionAuthorization(PermissionKeys.WarehouseTopCustomer)]
    public class TopCustomerWareHouseController : WareHouseBaseController
    {
        private readonly ISportTypeDataAccess _sportTypeDataAccess;

        private readonly ITopCustomerDataAccess _topCustomerDataAccess;

        public TopCustomerWareHouseController()
        {
            _sportTypeDataAccess = new SportTypeDataAccess();
            _topCustomerDataAccess = new TopCustomerDataAccess();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var licenseeId = licenseeSession.LicenseeId;
            var metaModel = new TopCustomerMetaModel { LicenseeId = licenseeId };
            metaModel.FromDate = metaModel.ToDate = GetBalanceUp().MoveBetsDay;
            var currencyList = userSession.CurrencyList;
            var sportList = _sportTypeDataAccess.GetAllWHSports();
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

            ViewBag.MaxDate = GetBalanceUp().MoveBetsDay;
            return View("~/Views/WareHouse/TopCustomer/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(TopCustomerMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
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
                SportList = _sportTypeDataAccess.GetAllWHSports(),
            };

            var totalValues = new Dictionary<string, decimal>();

            ViewBag.BalanceUpDay = GetBalanceUp().BalanceUpDay.ToShortDateString();
            model.MainResult = GetTopCustomerList(metaModel, ref totalValues);
            model.TotalValues = totalValues;

            ViewBag.MaxDate = GetBalanceUp().MoveBetsDay;
            return View("~/Views/WareHouse/TopCustomer/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(TopCustomerMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            const string FileName = "TopCustomer";
            const string FormatTitle = "{0} {1} {2} {3} {4} --> {5} - {6}";
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
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

            var viewByCurrency =
                string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName);
            var titleText = string.Format(
                FormatTitle,
                ViewResources.Top,
                metaModel.TopRow,
                ViewResources.Customer,
                ViewResources.From,
                metaModel.FromDate.ToShortDateString(),
                metaModel.ToDate.ToShortDateString(),
                viewByCurrency);

            var exportData = metaModel.IsValid
                       ? _topCustomerDataAccess.GetDataForExcel(metaModel)
                       : new DataTable();

            var topCustExcel = new TopCustomerExcel(templatePath, showingOption, exportData)
            {
                ReportTitle = titleText
            };

            topCustExcel.Export();
            return new ExcelResult(topCustExcel.Workbook, FileName);
        }

        private static Dictionary<string, decimal> GetTopCustomerTotalInfo(IEnumerable<TopCustomer> dataList)
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
                totalValues["betcount"] = dataList.Sum(x => x.BetCount);
                totalValues["turnover"] = dataList.Sum(x => x.Turnover);
                totalValues["memberwinloss"] = dataList.Sum(x => x.MemberWinLoss);
                totalValues["memberwinlosscomm"] = dataList.Sum(x => x.MemberWinLossCom);
                totalValues["licenseewinloss"] = dataList.Sum(x => x.LicenseeWinLoss);
                totalValues["bacomm"] = dataList.Sum(x => x.BAComm);

                totalValues["totalrows"] = dataList.Count();
            }

            return totalValues;
        }

        private IList<TopCustomer> GetTopCustomerList(TopCustomerMetaModel model, ref Dictionary<string, decimal> totalValues)
        {
            var dataList = model.IsValid
                               ? _topCustomerDataAccess.GetDataList(model)
                               : new List<TopCustomer>();
            totalValues = GetTopCustomerTotalInfo(dataList);
            return dataList;
        }
    }
}