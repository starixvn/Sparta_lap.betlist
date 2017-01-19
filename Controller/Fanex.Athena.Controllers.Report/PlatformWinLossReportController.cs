// -----------------------------------------------------------------------
// <copyright file="ReportController.PlatformWinLoss.cs" company="">
// ReportController copyright.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;

    [FunctionAuthorization(PermissionKeys.ReportPlatformWinLoss)]
    public class PlatformWinLossReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            AddNotificationMessageToViewBag();

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var licenseeId = licenseeSession.LicenseeId;
            var metaModel = new PlatformWinLossMetaModel { LicenseeId = licenseeId };
            var productAccess = ProductService.GetSingleWalletProductAccess(
                licenseeId,
                userSession.Language,
                FunctionNames.PlatformWinloss);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            SetConvetedUSDCurrencyId(metaModel);
            SetConvetedUSDCurrencyName(metaModel);

            metaModel.ProductOptions = productOption;

            var model = new PlatformWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Report/PlatformWinLoss/Main.cshtml", model);
        }
    }
}