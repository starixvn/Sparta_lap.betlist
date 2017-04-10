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
    using Model.Core.DataDomain;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class CustomerOutstandingBetListController : BetListBaseController
    {
        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";
        private const string BetListCustomerOutstandingRunningName = "BetList_CustomerOutstandingRunning";

        private static readonly string[] IgnoredProductInBetListRunning = new string[] { "BT", "CO" };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "Reviewd.")]
        public ActionResult Index(BetListMetaModel metaModel)
        {
            var isRunningBetlist = metaModel.BetListName.Equals("betlistrunning", StringComparison.OrdinalIgnoreCase);
            metaModel = CreateDefaultMetaModel(metaModel);

            var minDate = DateTime.Today.AddYears(-1);
            minDate = new DateTime(minDate.Year, 1, 1);
            metaModel.FromDate = isRunningBetlist ? minDate : metaModel.MoveBetsDay.AddDays(1);

            var productAccess = this.ProductService.GetProductAccess(
                metaModel.LicenseeId,
                SessionService.UserSession.Language,
                isRunningBetlist ? BetListCustomerOutstandingRunningName : BetListCustomerOutstandingName);

            var isFirstTimeLoading = metaModel.ProductOptions.Count() == 0;
            var previousCheckedProducts = metaModel.ProductOptions.Where(x => x.Status).ToList();
            var shouldCheckSportsbook = ShouldCheckSportsbook(previousCheckedProducts);
            var productOptions = new HashSet<ProductOption>();

            foreach (var product in productAccess.ProductList)
            {
                var productOption = new ProductOption()
                {
                    Code = product.ProductCode,
                    Name = product.ProductName
                };

                if (isFirstTimeLoading)
                {
                    productOption.Status = true;
                }
                else
                {
                    var status = previousCheckedProducts.Any(p => p.Code == product.ProductCode);
                    productOption.Status = status;
                }

                productOptions.Add(productOption);
            }

            metaModel.ProductOptions = productOptions;

            if (shouldCheckSportsbook)
            {
                var defaultProduct = metaModel.ProductOptions.FirstOrDefault(p => p.Code == "SB");
                defaultProduct.Status = true;
            }

            return View("~/Views/BetList/CustomerOutstanding/Main.cshtml", metaModel);
        }

        private static bool ShouldCheckSportsbook(IEnumerable<ProductOption> previousCheckedProducts)
        {
            var hasCheckedIgnoredProduct = previousCheckedProducts.Any(p => IgnoredProductInBetListRunning.Contains(p.Code));
            var hasCheckedOtherProducts = previousCheckedProducts.Any(p => !IgnoredProductInBetListRunning.Contains(p.Code));

            return hasCheckedIgnoredProduct && !hasCheckedOtherProducts;
        }
    }
}