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
    using Session;
    using Session.Interfaces;

    /// <summary>
    /// Class: AuthenticationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomApiAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly ISessionStorage _sessionStorage;

        public CustomApiAuthorizeAttribute() : this(null)
        {
        }

        public CustomApiAuthorizeAttribute(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage ?? new SessionStorage();
        }

        #region Override

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            return _sessionStorage.LicenseeSession != null && _sessionStorage.UserSession != null;
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("Unauthorized.") };
        }

        #endregion Override
    }
}