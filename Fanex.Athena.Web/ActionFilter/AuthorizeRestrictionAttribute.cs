namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Session;
    using Session.Enum;
    using Session.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AuthorizeRestrictionAttribute : AuthorizeAttribute
    {
        private readonly ISessionStorage _sessionStorage;

        private readonly AppMode _appMode;

        public AuthorizeRestrictionAttribute(AppMode mode)
        {
            _sessionStorage = new SessionStorage();
            _appMode = mode;
        }

        public AuthorizeRestrictionAttribute(ISessionStorage sessionStorage, AppMode mode)
        {
            _sessionStorage = sessionStorage;
            _appMode = mode;
        }

        #region Override

        /// <summary>
        /// On authorization.
        /// </summary>
        /// <param name="filterContext"> Parameter value.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
                var userSession = _sessionStorage.UserSession;
                var authorized = userSession != null && userSession.ApplicationMode == _appMode;
                if (!authorized)
                {
                    var routeValue = new
                    {
                        controller = "Utility",
                        action = "OneTokenRestriction",
                        code = "700"
                    };

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
                }
            }
        }

        #endregion Override
    }
}