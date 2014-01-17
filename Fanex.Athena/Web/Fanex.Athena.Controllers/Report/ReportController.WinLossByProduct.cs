// -----------------------------------------------------------------------
// <copyright file="ReportController.WinLossByProduct.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
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
        /// Win loss by product report.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult WinLossByProduct()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var productAccess = this._productService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, false);
            var productList = productAccess.ProductList;
            var metaModel = new WinLossByProductMetaModel
                {
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    LicenseeId = licenseeSession.LicenseeId,                    
                    IsExcludedTestData = true,
                    DrawFilter = 1,
                    ReportBy = 1,
                    ProductOptions = productList.Select(i => new ProductOption
                        {
                            Code = i.ProductCode,
                            Name = i.ProductName,
                            Status = i.ProductCode == Product.Sb
                        }).ToList(),
                };
            metaModel.CurrencyId = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? -2 : 0;

            var model = new WinLossByProductViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                    ProductAccess = productAccess,
                };

            return View("~/Views/Report/WinLossByProduct/Main.cshtml", model);
        }
    }
}