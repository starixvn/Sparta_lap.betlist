namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using Model.Core.DTO.OneAuth;

    public interface IOneBookSecurityWrapper
    {
        OneAuthToken Authenticate(string token);
    }
}