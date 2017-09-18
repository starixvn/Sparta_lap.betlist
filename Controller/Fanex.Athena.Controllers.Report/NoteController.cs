// -----------------------------------------------------------------------
// <copyright file="NoteController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.Report.ViewModel.Note;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using ONEbook.UM.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class NoteController : BaseController
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IProductService _productService;

        public NoteController()
        {
            this._productService = new ProductService();
            this._balanceUpDataAccess = new BalanceUpDataAccess();
        }

        [ChildActionOnly]
        public ActionResult Index(string viewName, bool isHistory = false)
        {
            var balanceUp = this._balanceUpDataAccess.GetBalanceUp();
            var licenseeId = this.SessionService.LicenseeSession.LicenseeId;
            var productList = this._productService.GetProductList(licenseeId, SessionService.UserSession.Language);
            var model = new NoteViewModel
            {
                MoveBetsDate = balanceUp.MoveBetsDay,
                ProductList = productList
            };

            var viewPath = $"~/Views/Note/{viewName}.cshtml";
            return View(viewPath, model);
        }
    }
}