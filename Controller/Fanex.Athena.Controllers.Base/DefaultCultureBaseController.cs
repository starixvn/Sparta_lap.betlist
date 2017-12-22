namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Service.Core.Helpers;
    using Web.ActionFilter;

    [CustomAuthorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class DefaultCultureBaseController : Controller
    {
        private readonly ISessionService _sessionService = new SessionService();
        private readonly IAppSettings _appSettings = new AppSettings();

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            var userSession = SessionService.UserSession;

            var option = userSession != null && userSession.ViewType != ClientViewType.None ?
                userSession.ViewType : (ClientViewType)int.Parse(_appSettings.ClientViewType);

            var viewByOption = ClientViewHelper.GetViewPath(viewName, option);
            if (viewByOption.IsNavigationView)
            {
                var returnPath = $"{this.RouteData.Values["Controller"]}/{this.RouteData.Values["Action"]}";
                return base.View(viewByOption.ViewPath, null, returnPath);
            }

            return base.View(viewByOption.ViewPath, masterName, model);
        }
    }
}