// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel">
// The  user service.
// </copyright>
// <summary>
//   The  user service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Users
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;

    /// <summary>
    ///     The i user service.
    /// </summary>
    public interface IUserService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The change password.
        /// </summary>
        /// <param name="userName">
        ///     The user name.
        /// </param>
        /// <param name="oldPassword">
        ///     The old password.
        /// </param>
        /// <param name="newPassword">
        ///     The new password.
        /// </param>
        /// <param name="ip">
        ///     The IP address.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IResultInfo ChangePassword(string userName, string oldPassword, string newPassword, string ip);

        /// <summary>
        ///     The get by licensee.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="isInternal">
        ///     The is internal.
        /// </param>
        /// <returns>
        /// The user list.
        /// </returns>
        List<User> GetByLicensee(int licenseeId, bool isInternal);

        /// <summary>
        ///     The get user info.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="userId">
        ///     The user id.
        /// </param>
        /// <returns>
        /// The user info.
        /// </returns>
        User GetUserInfo(int licenseeId, int userId);

        /// <summary>
        ///     The is user name existed.
        /// </summary>
        /// <param name="userName">
        ///     The user name.
        /// </param>
        /// <returns>
        ///     True if user exist.
        /// </returns>
        bool IsUserNameExisted(string userName);

        /// <summary>
        ///     The login on user.
        /// </summary>
        /// <param name="requestUser">
        ///     The request user.
        /// </param>
        /// <param name="netSessionId">
        ///     The net session id.
        /// </param>
        /// <param name="serverId">
        ///     The server id.
        /// </param>
        /// <param name="ip">
        ///     The IP address.
        /// </param>
        /// <returns>
        ///    The user id.
        /// </returns>
        int LoginOnUser(User requestUser, string netSessionId, int serverId, string ip);

        /// <summary>
        ///     The logout.
        /// </summary>
        /// <param name="onuserId">
        ///     The on user id.
        /// </param>
        void Logout(int onuserId);

        bool RecheckIn(int onuserId);

        /// <summary>
        ///     The reset password.
        /// </summary>
        /// <param name="user">
        ///     The user info.
        /// </param>
        /// <param name="resetPwdBy">
        ///     The user who reset password.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IResultInfo ResetPassword(User user, int resetPwdBy);

        /// <summary>
        ///     The update info.
        /// </summary>
        /// <param name="user">
        ///     The user info.
        /// </param>
        /// <param name="updatedBy">
        ///     The updated by.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IResultInfo UpdateInfo(User user, int updatedBy);

        /// <summary>
        ///     The validate internal user.
        /// </summary>
        /// <param name="requestUser">
        ///     The request user.
        /// </param>
        /// <param name="licenseeName">
        ///     The licensee name.
        /// </param>
        /// <param name="timeStamp">
        ///     The time stamp.
        /// </param>
        /// <param name="resultInfo">
        ///     The result info.
        /// </param>
        /// <returns>
        /// The user info.
        /// </returns>
        User ValidateInternalUser(User requestUser, string licenseeName, long timeStamp, out IResultInfo resultInfo);

        /// <summary>
        ///     The validate user.
        /// </summary>
        /// <param name="requestUser">
        ///     The request user.
        /// </param>
        /// <param name="resultInfo">
        ///     The result.
        /// </param>
        /// <returns>
        /// The user info.
        /// </returns>
        User ValidateUser(User requestUser, out IResultInfo resultInfo);

        /// <summary>
        ///     The validate user for LRF system.
        /// </summary>
        /// <param name="requestUser">
        ///     The request user.
        /// </param>
        /// <param name="resultInfo">
        ///     The result.
        /// </param>
        /// <returns>
        /// The user info.
        /// </returns>
        User ValidateUserForLrfSystem(User requestUser, out IResultInfo resultInfo);

        #endregion Public Methods and Operators
    }
}