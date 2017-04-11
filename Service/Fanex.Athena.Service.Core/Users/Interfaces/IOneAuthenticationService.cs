namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Web;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;

    public interface IOneAuthenticationService
    {
        IResultInfo SignIn(HttpContextBase currentContext, OneAuthToken tokenInfo);

        OneAuthToken GetTokenInfo(string token);
    }
}