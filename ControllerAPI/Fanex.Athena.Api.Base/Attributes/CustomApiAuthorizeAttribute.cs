// -----------------------------------------------------------------------
// <copyright file="CustomApiAuthorizeAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Extensions;
    using Session.Interfaces;

    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var scope = actionContext.Request.GetDependencyScope();
            var sessionStorage = scope.Resolve<ISessionStorage>();
            return sessionStorage.LicenseeSession != null && sessionStorage.UserSession != null;
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("Unauthorized.") };
        }
    }
}