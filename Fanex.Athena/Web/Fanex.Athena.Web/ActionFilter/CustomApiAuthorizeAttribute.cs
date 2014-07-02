// -----------------------------------------------------------------------
// <copyright file="CustomApiAuthorizeAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Fanex.Athena.Service.Implement.Users;

    /// <summary>
    /// Class: AuthenticationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomApiAuthorizeAttribute : AuthorizeAttribute
    {
        #region Override

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            return licenseeSession != null && userSession != null;
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("Unauthorized.") };
        }

        #endregion Override
    }
}