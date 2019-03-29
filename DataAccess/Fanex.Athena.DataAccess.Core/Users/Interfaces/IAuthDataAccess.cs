using System.Threading.Tasks;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    public interface IAuthDataAccess
    {
        Task<int> LoginOnUserAsync(User requestUser, string netSessionId, int serverId);

        Task LogoutAsync(int userId);

        Task<IResultInfo<User>> ValidateUserForLrfAsync(User requestUser);
    }
}