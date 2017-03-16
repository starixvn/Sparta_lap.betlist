// -----------------------------------------------------------------------
// <copyright file="IUserDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;

    /// <summary>
    ///     The i user data access.
    /// </summary>
    public interface IUserDataAccess
    {
        #region Public Methods and Operators

        IResultInfo ChangePassword(User requestUser, string oldPassword, string newPassword, string salt);

        List<User> GetByLicensee(int licenseeId, bool isInternal);

        User GetByUsername(string userName, int licenseeId = 0);

        User GetUserInfo(int licenseeId, int userId);

        object GetUserSalt(string userName);

        bool IsUsernameExisted(string userName);

        int LoginOnUser(User requestUser, string netSessionId, int serverId);

        void Logout(int userId);

        bool RecheckIn(int userId);

        IResultInfo ResetPassword(User user, string salt, int resetPwdBy);

        IResultInfo UpdateInfo(User user, int updatedBy);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateInternalUser(User requestUser, long timeStamp, string licenseeName, out IResultInfo resultInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        User ValidateUserForLrfSystem(User requestUser, out IResultInfo resultInfo);

        IResultInfo SyncUserInfo(string licenseeName, string userName);

        #endregion Public Methods and Operators
    }
}