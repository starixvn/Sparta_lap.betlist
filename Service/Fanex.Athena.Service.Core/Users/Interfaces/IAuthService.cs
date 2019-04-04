using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    public interface IAuthService
    {
        int LoginOnUser(User requestUser, string netSessionId, int serverId, string ip);

        void Logout(int onuserId);

        IResultInfo<User> ValidateUserForLrf(User requestUser, string loginToken);
    }
}