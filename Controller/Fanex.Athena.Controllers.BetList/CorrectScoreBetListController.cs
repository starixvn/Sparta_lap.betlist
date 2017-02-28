// -----------------------------------------------------------------------
// <copyright file="CorrectScoreBetListController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System.Web.Mvc;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.BetList.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;

    #endregion Using

    [FunctionAuthorization(PermissionKeys.TotalBetCorrectScore)]
    public class CorrectScoreBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = CreateDefaultMetaModel();

            SetConvetedUSDCurrencyName(metaModel);

            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var model = new BetListBaseViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = this.ProductService.GetProductAccess(metaModel.LicenseeId, userSession.Language),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/BetList/CorrectScoreBetList/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var currencies = SessionService.UserSession.CurrencyList;
            if (metaModel.CurrencyId == BaseCurrencyType.All || currencies.ContainsKey(metaModel.CurrencyId))
            {
                const string UspKey = "Lrf_BetList_CorrectScore";
                metaModel = CreateDefaultMetaModel(metaModel);

                var ticketResult = this.BetListService.GetBetListData(metaModel, UspKey);

                var showingOption = new ShowingOption
                {
                    CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                };

                var title = ViewResources.CorrectScore + " - " + string.Format(
                    ViewResources.Viewin,
                    metaModel.IsConvert ? metaModel.BaseCurrency.ToUpper() : metaModel.CurrencyName);

                var excel = new Excel.BetList.BetListExcel(
                    showingOption, metaModel.LicenseeId == 1, HiddenColumnIndexes, true);

                var workbook = excel.Export(ticketResult, title);

                return new ExcelResult(workbook, "CorrectScoreBetList");
            }

            return File(ExcelBase.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }
    }
}