using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    public interface IAuthDataAccess
    {
        int LoginOnUser(User requestUser, string netSessionId, int serverId);

        void Logout(int userId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateInternalUser(User requestUser, long timeStamp, string licenseeName, out IResultInfo resultInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateUserForLrfSystem(User requestUser, out IResultInfo resultInfo);
    }
}