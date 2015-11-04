// -----------------------------------------------------------------------
// <copyright file="AjaxFunctionAuthorizationAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.Athena.Service.Implement.Users;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AjaxApiFunctionAuthorizationAttribute : ActionFilterAttribute
    {
        public string ParentUrl { get; set; }

        #region Override

        public override void OnActionExecuting(HttpActionContext actionContext)
        {            
            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            var authorization = new AuthorizationService();
            var authorized = true;
            if (userSession != null && licenseeSession != null)
            {
                if (!string.IsNullOrWhiteSpace(ParentUrl))
                {
                    authorized = authorization.AuthorizePath(licenseeSession.LicenseeId, userSession.FunctionIdList, ParentUrl);
                }

                if (authorized)
                {
                    authorized = authorization.AuthorizePath(licenseeSession.LicenseeId, userSession.FunctionIdList, actionContext.Request.RequestUri.AbsolutePath);
                }
            }

            if (!authorized)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(actionContext);
        }

        #endregion Override
    }
}