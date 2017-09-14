// -----------------------------------------------------------------------
// <copyright file="RunningBettypesBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Linq;
    using System.Web.Mvc;
    using Excel.Base.Helpers;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    #endregion Using

    /// <summary>
    /// RunningBetList contains bet list:.
    /// Mix Parlay              - bet type: 9.
    /// Clean Sheet             - bet type: 13.
    /// Double Chance           - bet type: 24.
    /// B/O/N team to score     - bet type: 26.
    /// To win to nil           - bet type: 27.
    /// 3-Way Handicap          - bet type: 28.
    /// </summary>
    public class RunningBettypesBetListController : BetListBaseController
    {
        private const string DoubleChangeBettypes = "24,151,186,410,431";

        private const string BONTeamToScoreBettypes = "26,145,146,170,188,427,433";

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetMixParlay)]
        public ActionResult MixParlay(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = ((int)BetType.CombinationMixParlay).ToString();
            ViewBag.ReportTitle = Menu.Menu_12;
            ViewBag.UserGuideName = "TotalBetsMixParlay";
            return ProcessRequest(metaModel);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetDoubleChance)]
        public ActionResult DoubleChange(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = DoubleChangeBettypes;
            ViewBag.ReportTitle = ViewResources.DoubleChance;
            ViewBag.UserGuideName = "TotalBetsDoubleChance";
            return ProcessRequest(metaModel);
        }

        [HttpGet]
        [FunctionAuthorization(PermissionKeys.TotalBetBONTeamToScore)]
        public ActionResult BONTeamToScore(BetListMetaModel metaModel)
        {
            metaModel.MultipleBettype = BONTeamToScoreBettypes;
            ViewBag.ReportTitle = ViewResources.BONTeamToScore;
            ViewBag.UserGuideName = "TotalBetsBON";
            return ProcessRequest(metaModel);
        }

        private ActionResult ProcessRequest(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel = CreateDefaultMetaModel(metaModel);
            var currencies = userSession.CurrencyList;

            if (metaModel.IsExportExcel)
            {
                if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
                {
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                    return Excel(metaModel);
                }

                return EmptyExcel();
            }

            SetConvetedUSDCurrencyName(metaModel);

            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = currencies,
                ProductAccess = this.ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/RunningBetListMultipleBettypes/Main.cshtml", model);
        }

        private ActionResult Excel(BetListMetaModel metaModel)
        {
            var ticketResult = this.BetListService.GetBetListData(metaModel, "Lrf_BetList_SelRunningByBettypeList");

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };

            var firstBettype = metaModel.MultipleBettype.Split(',').FirstOrDefault();
            var title = string.Format(
               "{0} - {1}",
               CacheRef.GetBettype(firstBettype),
               string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, HiddenColumnIndexes, true);

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RunningBetList");
        }
    }
}