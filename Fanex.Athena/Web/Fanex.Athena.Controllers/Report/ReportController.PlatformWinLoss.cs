// -----------------------------------------------------------------------
// <copyright file="ReportController.PlatformWinLoss.cs" company="Nexcel">
// ReportController copyright.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    public partial class ReportController
    {
        private const string PlatformWinlossName = "PlatformWinloss";

        #region Web Controller

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult MobileWinloss()
        {
            AddNotificationMessageToViewBag();

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var licenseeId = licenseeSession.LicenseeId;
            var metaModel = new PlatformWinLossMetaModel { LicenseeId = licenseeId };
            var productAccess = _productService.GetProductAccess(licenseeId, userSession.Language, PlatformWinlossName);
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
                CurrencyList = GetUserCurrencies(),
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Report/PlatformWinLoss/Main.cshtml", model);
        }

        #endregion Web Controller
    }
}