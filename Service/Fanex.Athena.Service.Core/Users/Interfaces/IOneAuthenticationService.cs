namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Web;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IOneAuthenticationService
    {
        LicenseeSiteName SingleLicenseeSiteName(string siteName);

        IResultInfo SignIn(HttpContextBase currentContext, int licenseeId, OneAuthenticationToken tokenInfo);

        OneAuthenticationToken GetTokenInfo(string token);
    }
}