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
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Session.Enum;
    using Session.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FunctionAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly string[] _permissionKeys;

        public FunctionAuthorizationAttribute()
        {
        }

        public FunctionAuthorizationAttribute(params string[] permissionKeys)
        {
            _permissionKeys = permissionKeys;
        }

        public ISessionStorage SessionStorage { get; set; }

        public IAuthorizationService AuthorizationServiceInstance { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            var licenseeSession = SessionStorage.LicenseeSession;
            var userSession = SessionStorage.UserSession;
            var authorized = userSession != null && licenseeSession != null;
            if (authorized)
            {
                authorized = userSession.ApplicationMode == AppMode.Parasitic ?
                              AuthorizationServiceInstance.HasPermissions(userSession.PortalFunctionList, _permissionKeys) :
                              AuthorizationServiceInstance.HasPermissions(userSession.FunctionIdList, _permissionKeys);
            }

            if (!authorized)
            {
                if (context.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    filterContext.HttpContext.Response.ContentType = "application/json";
                    filterContext.Result = new JsonResult
                    {
                        Data = new { ErrCode = (int)HttpStatusCode.Forbidden, ErrMsg = "Access Denied." },
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
    }
}