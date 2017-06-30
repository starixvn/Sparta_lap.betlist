// -----------------------------------------------------------------------
// <copyright file="CustomerOutstandingBetListController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class CustomerOutstandingBetListController : BetListBaseController
    {
        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";
        private const string BetListCustomerOutstandingRunningName = "BetList_CustomerOutstandingRunning";

        public ActionResult Index(BetListMetaModel metaModel)
        {
            var isRunningBetlist = metaModel.BetListName.Equals("betlistrunning", StringComparison.OrdinalIgnoreCase);
            metaModel = CreateDefaultMetaModel(metaModel);

            metaModel.FromDate = isRunningBetlist ? new DateTime(DateTime.Now.Year - 1, 1, 1) : metaModel.MoveBetsDay.AddDays(1);

            var productAccess = this.ProductService.GetProductAccess(
                metaModel.LicenseeId,
                SessionService.UserSession.Language,
                isRunningBetlist ? BetListCustomerOutstandingRunningName : BetListCustomerOutstandingName);

            var viewModel = new BetListBaseViewModel
            {
                Param = metaModel,
                ProductOptions = GetProductOptions(metaModel.ProductCodeList, productAccess.ProductList)
            };
            return View("~/Views/BetList/CustomerOutstanding/Main.cshtml", viewModel);
        }

        private static IEnumerable<ProductOption> GetProductOptions(string productCodeList, IEnumerable<Product> allProducts)
        {
            var isFirstTimeLoading = string.IsNullOrEmpty(productCodeList);
            var previousCheckedProducts = isFirstTimeLoading ? new string[] { } : productCodeList.Split(',');

            var productOptions = new List<ProductOption>();

            foreach (var product in allProducts)
            {
                productOptions.Add(new ProductOption
                {
                    Code = product.ProductCode,
                    Name = product.ProductName,
                    Status = isFirstTimeLoading || previousCheckedProducts.Any(p => p.Equals(product.ProductCode, StringComparison.OrdinalIgnoreCase))
                });
            }

            return productOptions;
        }
    }
}