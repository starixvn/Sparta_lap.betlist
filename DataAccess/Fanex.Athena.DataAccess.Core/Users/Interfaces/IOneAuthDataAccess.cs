using Fanex.Athena.Model.Core.DTO.OneAuth;

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    public interface IOneAuthDataAccess
    {
        OneAuthUserInfo GetUserInfo(int userSEQ);
    }
}