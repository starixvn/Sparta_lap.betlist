// -----------------------------------------------------------------------
// <copyright file="IUserDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Users
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;

    /// <summary>
    ///     The i user data access.
    /// </summary>
    public interface IUserDataAccess
    {
        #region Public Methods and Operators

        IResultInfo ChangePassword(User requestUser, string oldPassword, string newPassword, string salt);

        List<User> GetByLicensee(int licenseeId, bool isInternal);

        User GetUserInfo(int licenseeId, int userId);

        object GetUserSalt(string userName);

        bool IsUsernameExited(string userName);

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

        #endregion Public Methods and Operators
    }
}