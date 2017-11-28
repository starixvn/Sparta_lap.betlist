using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    public interface IAuthService
    {
        int LoginOnUser(User requestUser, string netSessionId, int serverId, string ip);

        void Logout(int onuserId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateInternalUser(User requestUser, string licenseeName, long timeStamp, out IResultInfo resultInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateUserForLrfSystem(User requestUser, string loginToken, out IResultInfo resultInfo);
    }
}