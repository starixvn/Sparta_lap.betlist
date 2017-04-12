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
    using Model.Core.DTO.Common;
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

            var isFirstTimeLoading = !metaModel.ProductOptions.Any();
            var previousCheckedProducts = metaModel.ProductOptions.Where(x => x.Status).ToList();
            var productOptions = new HashSet<ProductOption>();

            foreach (var product in productAccess.ProductList)
            {
                productOptions.Add(new ProductOption
                {
                    Code = product.ProductCode,
                    Name = product.ProductName,
                    Status = isFirstTimeLoading || previousCheckedProducts.Any(p => p.Code == product.ProductCode)
                });
            }

            metaModel.ProductOptions = productOptions;

            if (metaModel.ProductOptions.Any(p => p.Code == Product.SB) && ShouldCheckSportsbook(previousCheckedProducts))
            {
                var sportbookProduct = metaModel.ProductOptions.FirstOrDefault(p => p.Code == Product.SB);
                sportbookProduct.Status = true;
            }

            return View("~/Views/BetList/CustomerOutstanding/Main.cshtml", metaModel);
        }

        private static bool ShouldCheckSportsbook(IEnumerable<ProductOption> previousCheckedProducts)
        {
            return previousCheckedProducts.All(i => IgnoredProductInBetListRunning.Contains(i.Code));
        }
    }
}