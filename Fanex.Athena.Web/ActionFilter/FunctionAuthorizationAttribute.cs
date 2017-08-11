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
        public FunctionAuthorizationAttribute(string permissionKey)
        {
            PermissionKeys = new string[] { permissionKey };
        }

        public FunctionAuthorizationAttribute(string permissionKey1, string permissionKey2)
        {
            PermissionKeys = new string[] { permissionKey1, permissionKey2 };
        }

        public FunctionAuthorizationAttribute(string permissionKey1, string permissionKey2, string permissionKey3)
        {
            PermissionKeys = new string[] { permissionKey1, permissionKey2, permissionKey3 };
        }

        public string[] PermissionKeys { get; set; }

        #region Override

        /// <summary>
        /// Check authorized function.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
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