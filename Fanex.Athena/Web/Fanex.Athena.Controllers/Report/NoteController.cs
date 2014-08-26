// -----------------------------------------------------------------------
// <copyright file="NoteController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Models.ViewModel.Note;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Configuration;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class NoteController : ReadonlyBaseController
    {
        #region Fields

        private readonly IBalanceUpService _balanceUpService;
        private readonly IProductService _productService;

        #endregion Fields

        public NoteController()
        {
            this._productService = new ProductService();
            this._balanceUpService = new BalanceUpService();
        }

        #region Action

        /// <summary>
        /// Note Page.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        /// <param name="isHistory">Equals True if The report has history note.</param>
        /// <returns>The action result.</returns>
        [ChildActionOnly]
        public ActionResult Index(string viewName, bool isHistory = false)
        {
            var balanceUpInfo = this._balanceUpService.BalanceUpInfo;
            var licenseeId = this.SessionService.LicenseeSession.LicenseeId;
            var productList = this._productService.GetProductList(licenseeId, SessionService.UserSession.Language);
            var model = new NoteViewModel
                {
                    BalanceUpDate = balanceUpInfo.BalanceUpDay,
                    MoveBetsDate = balanceUpInfo.MoveBetsDay,
                    ProductList = productList,
                    IsHistory = isHistory,
                    HasEffectiveBettingValueColumn = GetHasBetEffectiveColumn(licenseeId)
                };

            var viewPath = string.Format("~/Views/Note/{0}.cshtml", viewName);
            return View(viewPath, model);
        }

        #endregion Action
    }
}