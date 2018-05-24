// -----------------------------------------------------------------------
// <copyright file="FunctionAuthorizationAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Athena.Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Session;
    using Session.Enum;
    using Session.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FunctionAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly ISessionStorage _sessionStorage;

        private readonly IAuthorizationService _authorizationService;

        public FunctionAuthorizationAttribute()
             : this(sessionStorage: null, authorizationService: null, permissionKeys: null)
        {
        }

        public FunctionAuthorizationAttribute(params string[] permissionKeys)
            : this(sessionStorage: null, authorizationService: null, permissionKeys: permissionKeys)
        {
        }

        public FunctionAuthorizationAttribute(
            ISessionStorage sessionStorage,
            IAuthorizationService authorizationService,
            params string[] permissionKeys)
        {
            _sessionStorage = sessionStorage ?? new SessionStorage();
            _authorizationService = authorizationService ?? new AuthorizationService();
            PermissionKeys = permissionKeys;
        }

        public string[] PermissionKeys { get; set; }

        #region Override

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            var licenseeSession = _sessionStorage.LicenseeSession;
            var userSession = _sessionStorage.UserSession;
            var authorized = userSession != null && licenseeSession != null;
            if (authorized)
            {
                authorized = userSession.ApplicationMode == AppMode.Parasitic ?
                              _authorizationService.HasPermissions(userSession.PortalFunctionList, PermissionKeys) :
                              _authorizationService.HasPermissions(userSession.FunctionIdList, PermissionKeys);
            }

            if (!authorized)
            {
                if (context.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    filterContext.HttpContext.Response.ContentType = "application/json";
                    filterContext.Result = new JsonResult
                    {
                        Data = new { ErrCode = 403, ErrMsg = "Access Denied." },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    var routeValue = new
                    {
                        controller = "Utility",
                        action = "AccessDenied",
                        message = "AccessDenied"
                    };

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
                }
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion Override
    }
}