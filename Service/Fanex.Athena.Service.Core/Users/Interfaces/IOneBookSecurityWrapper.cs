namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;

    public interface IOneBookSecurityWrapper
    {
        OneAuthToken Authenticate(string token);
    }
}