namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using ONEbook.UM.Filters;
    using Service.Core.Helpers;
    using Web.ActionFilter;

    [CustomAuthorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class DefaultCultureBaseController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IClientViewHelper _clientViewHelper;

        public DefaultCultureBaseController(ISessionService sessionService, IClientViewHelper clientViewHelper)
        {
            _sessionService = sessionService;
            _clientViewHelper = clientViewHelper;
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            var viewPath = _clientViewHelper.GetViewPath(viewName);

            return base.View(viewPath, masterName, model);
        }
    }
}