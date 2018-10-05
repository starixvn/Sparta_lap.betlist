namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using ONEbook.UM.Filters;
    using Service.Core.Helpers;
    using Session.Enum;
    using Web.ActionFilter;

    [CustomAuthorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class DefaultCultureBaseController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IAppSettings _appSettings;
        private readonly IClientViewHelper _clientViewHelper;

        public DefaultCultureBaseController(ISessionService sessionService, IAppSettings appSetting, IClientViewHelper clientViewHelper)
        {
            _sessionService = sessionService;
            _appSettings = appSetting;
            _clientViewHelper = clientViewHelper;
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            var userSession = SessionService.UserSession;

            var option = userSession != null && userSession.ViewType != ClientViewType.None ?
                userSession.ViewType : (ClientViewType)int.Parse(_appSettings.ClientViewType);

            var viewByOption = _clientViewHelper.GetViewPath(viewName, option);
            if (viewByOption.IsNavigationView)
            {
                var returnPath = RouteData.Values["Controller"] + "/" + RouteData.Values["Action"];
                return base.View(viewByOption.ViewPath, null, returnPath);
            }

            return base.View(viewByOption.ViewPath, masterName, model);
        }
    }
}