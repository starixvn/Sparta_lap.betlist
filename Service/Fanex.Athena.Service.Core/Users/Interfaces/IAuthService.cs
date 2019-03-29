using System.Threading.Tasks;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    public interface IAuthService
    {
        Task<int> LoginOnUserAsync(User requestUser, string netSessionId, int serverId, string ip);

        Task LogoutAsync(int onuserId);

        Task<IResultInfo<User>> ValidateUserForLrfAsync(User requestUser, string loginToken);
    }
}