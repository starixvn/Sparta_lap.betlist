// -----------------------------------------------------------------------
// <copyright file="ReportController.MobileWinLoss.cs" company="Nexcel">
// ReportController copyright.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Mobile Win Loss.
    /// </summary>
    public partial class ReportController
    {
        #region Web Controller

        /// <summary>
        ///     Mobile win loss default controller.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult MobileWinloss()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var licenseeId = licenseeSession.LicenseeId;
            var metaModel = new MobileWinLossMetaModel { LicenseeId = licenseeId };
            var productAccess = _productService.GetProductAccess(licenseeId, SessionService.UserSession.Language, IgnoreCasinoAndBingoAndKeno);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.Sb
                                             }).ToList();

            SetConvetedUSDCurrencyId(metaModel);

            metaModel.ProductOptions = productOption;

            var model = new MobileWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = SessionService.UserSession.CurrencyList,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm
            };

            return View("~/Views/Report/MobileWinLoss/Main.cshtml", model);
        }

        #endregion Web Controller
    }
}