using Fanex.Athena.Model.Core.DTO.Common;

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    public interface IOneAuthenticationService
    {
        LicenseeSiteName SingleLicenseeSiteName(string siteName);

        IResultInfo InsertLicenseeUser(OneAuthenticationToken tokenInfo);

        IResultInfo SignIn(int licenseeId, OneAuthenticationToken tokenInfo);

        OneAuthenticationToken GetTokenInfo(string token);
    }
}