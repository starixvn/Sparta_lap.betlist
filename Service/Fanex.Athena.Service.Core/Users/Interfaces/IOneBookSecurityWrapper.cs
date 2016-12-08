namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IOneBookSecurityWrapper
    {
        OneAuthenticationToken Authenticate(string token);
    }
}