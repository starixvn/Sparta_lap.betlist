// <copyright file="ReportController.CustomerWinloss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new CustomerWinLossMetaModel
            {
                DrawFilter = DrawFilterType.Normal,
                IsExcludedTestData = true
            };

            SetConvetedUSDCurrencyName(metaModel);
            var productAccess = ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.CustomerWinLoss);
            var productOption = productAccess.ProductList
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            metaModel.ProductOptions = productOption;
            var model = new CustomerWinLossViewModel
            {
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = userSession.CurrencyList,
                Param = metaModel,
                IsHistory = false,
            };

            return View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }
    }
}