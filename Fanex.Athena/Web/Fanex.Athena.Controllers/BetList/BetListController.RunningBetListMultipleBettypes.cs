// -----------------------------------------------------------------------
// <copyright file="BetListController.RunningBetList.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.BetList;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.BetList;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Fanex.Common;

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
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for running bet list.
        /// [Used in TOTAL BETS].
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult RunningBetListMultipleBettypes(BetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            metaModel = CreateDefaultMetaModel(metaModel);
            
            if (metaModel.IsExportExcel)
            {
                var currencyList = userSession.CurrencyList;

                if (metaModel.CurrencyId == BaseCurrencyType.All || currencyList.Values.Contains(metaModel.CurrencyId))
                {
                    return RunningBetListMultipleBettypesExcel(metaModel);
                }

                return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
            }

            SetConvetedUSDCurrencyName(metaModel);
           
            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                LicenseeTypeList = licenseeSession.LicenseeType,
                ProductAccess = this._productService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/RunningBetListMultipleBettypes/Main.cshtml", model);
        }

        /// <summary>
        /// Used in TOTAL BETS.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [NonAction]
        [FunctionAuthorization]
        public ActionResult RunningBetListMultipleBettypesExcel(BetListMetaModel metaModel)
        {
            var ticketResult = this._betListService.GetBetListData(metaModel, "Lrf_BetList_SelRunningByBettypeList");

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
            };
            
            var firstBettype = metaModel.MultipleBettype.Split(',').FirstOrDefault();
            var title = string.Format(
               "{0} - {1}",
               CacheRef.GetBettype(firstBettype),
               string.Format(ViewResources.Viewin, metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName));

            var excel = new BetListExcel(showingOption, metaModel.LicenseeId == 1, new List<int> { 7, 8 });

            var workbook = excel.Export(ticketResult, title);

            return new ExcelResult(workbook, "RunningBetList");
        }
    }
}