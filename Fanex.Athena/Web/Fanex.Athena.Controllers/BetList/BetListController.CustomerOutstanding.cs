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
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.MetaModel.Product;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Action Get for Customer Outstanding.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The action result.</returns>
        public ActionResult CustomerOutstanding(BetListMetaModel metaModel)
        {
            metaModel = CreateDefaultMetaModel(metaModel);

            var minDate = DateTime.Today.AddYears(-1);

            minDate = new DateTime(minDate.Year, 1, 1);
            metaModel.FromDate = metaModel.BetListName == "betlistrunning" ? minDate : metaModel.MoveBetsDay;

            var productAccess = this._productService.GetProductAccess(metaModel.LicenseeId, SessionService.UserSession.Language);

            metaModel.ProductOptions = new List<ProductOption>();
            for (int i = 0, j = productAccess.ProductList.Count; i < j; i++)
            {
                var status = true;
                var statusURL = "productoptions[" + i + "].status";
                if (Request.QueryString[statusURL] != null)
                {
                    bool.TryParse(Request.QueryString[statusURL], out status);
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