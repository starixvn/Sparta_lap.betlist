// -----------------------------------------------------------------------
// <copyright file="CustomApiAuthorizeAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.ActionFilter
{
    using System.Web.Http;
    using Fanex.Athena.Service.Implement.Users;

    /// <summary>
    /// Class: AuthenticationAttribute.
    /// </summary>
    public class CustomApiAuthorizeAttribute : AuthorizeAttribute
    {
        #region Override

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            return licenseeSession != null && userSession != null;
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                throw new HttpResponseException(challengeMessage);
            }
        }

        #endregion Override
    }
}