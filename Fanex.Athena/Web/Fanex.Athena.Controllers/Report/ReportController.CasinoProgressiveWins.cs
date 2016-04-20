// -----------------------------------------------------------------------
// <copyright file="ReportController.CasinoProgressiveWins.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        private const string CasinoProgressiveWinsName = "CasinoProgressiveWins";

        [HttpGet]
        [FunctionAuthorization]
        public ActionResult CasinoProgressiveWins()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, CasinoProgressiveWinsName);
            var metaModel = new CasinoProgressiveWinsMetaModel
                {
                    FromDate = DateTime.Now.AddYears(-1),
                    ToDate = DateTime.Now,
                    LicenseeId = licenseeSession.LicenseeId,
                    ProductOptions = productAccess.ProductList.Select(i => new ProductOption
                        {
                            Code = i.ProductCode,
                            Name = i.ProductName,
                            Status = i.ProductCode == Product.MGCRNG
                        }).ToList(),
                };
           
            SetConvetedUSDCurrencyId(metaModel);

            var model = new CasinoProgressiveWinsViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    CurrencyList = GetUserCurrencies(),
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    ProductAccess = productAccess,
                };

            return View("~/Views/Report/CasinoProgressiveWins/Main.cshtml", model);
        }
    }
}