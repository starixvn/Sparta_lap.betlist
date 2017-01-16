// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWinsReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportCasinoProgressiveWins)]
    public class CasinoProgressiveWinsReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.CasinoProgressiveWins);
            var metaModel = new CasinoProgressiveWinsMetaModel
            {
                FromDate = DateTime.Now.AddMonths(-3),
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
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
            };

            return View("~/Views/Report/CasinoProgressiveWins/Main.cshtml", model);
        }
    }
}