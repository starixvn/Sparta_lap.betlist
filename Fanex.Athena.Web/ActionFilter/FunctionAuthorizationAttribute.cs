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
    using Model.Core.DataDomain;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FunctionAuthorizationAttribute : ActionFilterAttribute
    {
        public FunctionAuthorizationAttribute()
        {
        }

        public FunctionAuthorizationAttribute(string permissionKey)
        {
            PermissionKeys = new[] { permissionKey };
        }

        public string[] PermissionKeys { get; set; }

        #region Override

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            var authorization = new AuthorizationService();
            var authorized = true;
            if (userSession != null && licenseeSession != null)
            {
                authorized =
                userSession.ApplicationMode == AppMode.Parasitic ?
                      authorization.HasPermissions(userSession.PortalFunctionList, PermissionKeys) :
                      authorization.HasPermissions(userSession.FunctionIdList, PermissionKeys);
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