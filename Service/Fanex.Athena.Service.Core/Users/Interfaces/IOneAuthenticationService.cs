namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Threading.Tasks;
    using System.Web;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;

    public interface IOneAuthenticationService
    {
        Task<IResultInfo> SignIn(HttpContextBase currentContext, OneAuthToken tokenInfo);

        OneAuthToken GetTokenInfo(string token);
    }
}