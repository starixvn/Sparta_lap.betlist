// -----------------------------------------------------------------------
// <copyright file="BetListController.CustomerOutstanding.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.MetaModel.Product;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        private const string BetListCustomerOutstandingName = "BetList_CustomerOutstanding";
        private const string BetListCustomerOutstandingRunningName = "BetList_CustomerOutstandingRunning";

        /// <summary>
        /// Action Get for Customer Outstanding.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "Reviewd.")]
        public ActionResult CustomerOutstanding(BetListMetaModel metaModel)
        {
            var isRunningBetlist = metaModel.BetListName.Equals("betlistrunning", StringComparison.OrdinalIgnoreCase);

            metaModel = CreateDefaultMetaModel(metaModel);

            var minDate = DateTime.Today.AddYears(-1);

            minDate = new DateTime(minDate.Year, 1, 1);
            metaModel.FromDate = isRunningBetlist ? minDate : metaModel.MoveBetsDay.AddDays(1);

            var productAccess = this._productService.GetProductAccess(
                metaModel.LicenseeId, 
                SessionService.UserSession.Language,
                isRunningBetlist ? BetListCustomerOutstandingRunningName : BetListCustomerOutstandingName);

            metaModel.ProductOptions = new HashSet<ProductOption>();
            for (int i = 0, j = productAccess.ProductList.Count; i < j; i++)
            {
                var status = true;
                var statusURL = "productoptions[" + i + "].status";
                var statusText = Request.QueryString[statusURL];
                if (statusText != null)
                {
                    bool.TryParse(statusText, out status);
                }

                metaModel.ProductOptions.Add(new ProductOption
                                             {
                                                 Code = productAccess.ProductList[i].ProductCode,
                                                 Name = productAccess.ProductList[i].ProductName,
                                                 Status = status
                                             });
            }

            return View("~/Views/BetList/CustomerOutstanding/Main.cshtml", metaModel);
        }
    }
}