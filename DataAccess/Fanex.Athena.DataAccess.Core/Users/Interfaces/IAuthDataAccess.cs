using System.Threading.Tasks;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    public interface IAuthDataAccess
    {
        int LoginOnUser(User requestUser, string netSessionId, int serverId);

        void Logout(int userId);

        IResultInfo<User> ValidateUserForLrf(User requestUser);
    }
}