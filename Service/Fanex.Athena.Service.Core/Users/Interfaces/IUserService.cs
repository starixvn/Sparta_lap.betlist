// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="">
// The  user service.
// </copyright>
// <summary>
//   The  user service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;

    /// <summary>
    ///     The i user service.
    /// </summary>
    public interface IUserService
    {
        #region Public Methods and Operators

        IResultInfo ChangePassword(string userName, string oldPassword, string newPassword, string ip);

        IList<User> GetByLicensee(int licenseeId, bool isInternal);

        User GetUserInfo(int licenseeId, int userId);

        bool IsUserNameExisted(string userName);

        int LoginOnUser(User requestUser, string netSessionId, int serverId, string ip);

        void Logout(int onuserId);

        bool RecheckIn(int onuserId);

        IResultInfo ResetPassword(User user, int resetPwdBy);

        IResultInfo UpdateInfo(User user, int updatedBy);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateInternalUser(User requestUser, string licenseeName, long timeStamp, out IResultInfo resultInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateUserForLrfSystem(User requestUser, string loginToken, out IResultInfo resultInfo);

        User GetByUsername(string userName, int licenseeId = 0);

        IResultInfo SyncUserInfo(string licenseeName,string userName);
        #endregion Public Methods and Operators
    }
}