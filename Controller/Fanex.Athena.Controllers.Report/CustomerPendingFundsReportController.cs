// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerPendingFunds.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportCustomerPendingFunds)]
    public class CustomerPendingFundsReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new CustomerPendingFundsMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId
            };
            SetConvetedUSDCurrencyName(metaModel);
            var productAccess = ProductService.GetSingleWalletProductAccess(
                metaModel.LicenseeId,
                userSession.Language,
                FunctionNames.CustomerPendingFunds);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            metaModel.ProductOptions = productOption;

            var model = new CustomerPendingFundsViewModel
            {
                CurrencyList = userSession.CurrencyList,
                Param = metaModel
            };

            return View("~/Views/Report/CustomerPendingFunds/Main.cshtml", model);
        }
    }
}