// -----------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportColossusBetsWinLoss)]
    public partial class ColossusBetsWinLossReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;
            var productList = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            if (!productList.Any(x => x.ProductID == ProductTypes.ColossusBets))
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            var metaModel = new ThirdPartyWinLossMetaModel
            {
                FromDate = date,
                ToDate = date,
                ReportBy = 1,
                IsExcludedTestData = true
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new ThirdPartyWinLossViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList
            };

            return View("~/Views/Report/ColossusBetsWinLoss/Main.cshtml", model);
        }
    }
}