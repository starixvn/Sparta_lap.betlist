// -----------------------------------------------------------------------
// <copyright file="ReportController.ActiveCustomer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Product;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// Default licenseeWL action.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult ActiveCustomerSummary()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, IgnoreAgCasino);
            var productList = productAccess.ProductList;
            var defaultDate = DateTime.Now.AddDays(-1);
            var metaModel = new ActiveCustomerMetaModel
                {
                    FromDate = defaultDate,
                    ToDate = defaultDate,
                    LicenseeId = licenseeSession.LicenseeId,                                        
                    ProductOptions = productList.Select(i => new ProductOption
                        {
                            Code = i.ProductCode,
                            Name = i.ProductName,
                            Status = i.ProductCode == Product.SB
                        }).ToList(),
                };
           
            SetConvetedUSDCurrencyId(metaModel);
           
            var model = new ActiveCustomerViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    CurrencyList = userSession.CurrencyList,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    ProductAccess = productAccess,
                };

            return View("~/Views/Report/ActiveCustomerSummary/Main.cshtml", model);
        }
    }
}