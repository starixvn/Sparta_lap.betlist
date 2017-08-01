// -----------------------------------------------------------------------
// <copyright file="ReportController.CustomerBalance.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Resources;

    [FunctionAuthorization(PermissionKeys.ReportCustomerBalance)]
    public class CustomerBalanceReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var metaModel = new CustomerBalanceMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                CurrencyId = 0
            };

            var productAccess = ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.CustomerBalance);
            var productOption = productAccess.ProductList.Where(i => i.ProductCode != Product.BA)
                                             .Select(i => new ProductOption
                                             {
                                                 Code = i.ProductCode,
                                                 Name = i.ProductName,
                                                 Status = i.ProductCode == Product.SB
                                             }).ToList();

            metaModel.ProductOptions = productOption;
            var model = new CustomerBalanceViewModel
            {
                ShowEmptyContent = true,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                CurrencyList = userSession.CurrencyList,
                Param = metaModel
            };

            ViewBag.TopCountList = GenerateTopCountList(metaModel.TopCount);

            return View("~/Views/Report/CustomerBalance/Main.cshtml", model);
        }

        private static IList<SelectListItem> GenerateTopCountList(int topCount)
        {
            const string Label100 = "100";
            const string Label500 = "500";
            var result = new List<SelectListItem>
                {
                    new SelectListItem
                        {
                            Value = "0",
                            Text = ViewResources.All,
                            Selected = topCount == 0,
                        },
                    new SelectListItem
                        {
                            Value = Label100,
                            Text = Label100,
                            Selected = topCount == 100,
                        },
                    new SelectListItem
                        {
                            Value = Label500,
                            Text = Label500,
                            Selected = topCount == 500,
                        }
                };

            return result;
        }
    }
}