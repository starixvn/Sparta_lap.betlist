// -----------------------------------------------------------------------
// <copyright file="NoteController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Report.ViewModel.Note;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using ONEbook.UM.Filters;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class NoteController : BaseController
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IProductService _productService;

        public NoteController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess)
            : base(languageService, sessionService, appSetting, clientViewHelper)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _productService = productService;
        }

        [ChildActionOnly]
        public ActionResult Index(string viewName)
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