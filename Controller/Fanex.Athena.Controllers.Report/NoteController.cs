// -----------------------------------------------------------------------
// <copyright file="NoteController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Model.Report.ViewModel.Note;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using ONEbook.UM.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class NoteController : BaseController
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly IProductService _productService;

        public NoteController()
        {
            this._productService = new ProductService();
            this._balanceUpService = new BalanceUpService();
        }

        [ChildActionOnly]
        public ActionResult Index(string viewName, bool isHistory = false)
        {
            var balanceUpInfo = this._balanceUpService.BalanceUpInfo;
            var licenseeId = this.SessionService.LicenseeSession.LicenseeId;
            var productList = this._productService.GetProductList(licenseeId, SessionService.UserSession.Language);
            var model = new NoteViewModel
            {
                MoveBetsDate = balanceUpInfo.MoveBetsDay,
                ProductList = productList
            };

            var viewPath = $"~/Views/Note/{viewName}.cshtml";
            return View(viewPath, model);
        }
    }
}