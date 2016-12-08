// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="">
// The user service.
// </copyright>
// <summary>
//   The user service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Core.Users;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Exception;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Common;
    using Resources;

    /// <summary>
    ///     The user service.
    /// </summary>
    public class UserService : IUserService
    {
        #region Constants and Fields

        private const int UserNotExistError = 100;
        private const int WrongUserNameOrPasswordError = 104;

        /// <summary>
        ///     The _user data access.
        /// </summary>
        private readonly IUserDataAccess _userDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        public UserService()
        {
            this._userDataAccess = new UserDataAccess();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserService" /> class.
        /// </summary>
        /// <param name="userDataAccess">
        ///     The user data access.
        /// </param>
        public UserService(IUserDataAccess userDataAccess)
        {
            this._userDataAccess = userDataAccess;
        }

        #endregion Constructors and Destructors

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
        ///     The request IP.
        /// </param>
        /// <returns>
        /// The result information.
        /// </returns>
        public IResultInfo ChangePassword(string userName, string oldPassword, string newPassword, string ip)
        {
            // if old password is valid, process update password
            // create salt to hash new password
            var oldSalt = this._userDataAccess.GetUserSalt(userName).ToString();
            var salt = Cryptography.CreateSalt();
            newPassword = Cryptography.CreateHash(newPassword, salt);
            oldPassword = Cryptography.CreateHash(oldPassword, oldSalt);

            var user = new User { Request = new Request { Ip = ip }, Username = userName };

            return this._userDataAccess.ChangePassword(user, oldPassword, newPassword, salt);
        }

        public IList<User> GetByLicensee(int licenseeId, bool isInternal)
        {
            return this._userDataAccess.GetByLicensee(licenseeId, isInternal);
        }

        public User GetByUsername(string userName, int licenseeId = 0)
        {
            return _userDataAccess.GetByUsername(userName, licenseeId);
        }

        public User GetUserInfo(int licenseeId, int userId)
        {
            return this._userDataAccess.GetUserInfo(licenseeId, userId);
        }

        public bool IsUserNameExisted(string userName)
        {
            return this._userDataAccess.IsUsernameExisted(userName);
        }

        /// <summary>
        ///   If returned value greater than 0, User login successfully.
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
        ///     The request IP.
        /// </param>
        /// <returns>
        ///     The integer value.
        /// </returns>
        public int LoginOnUser(User requestUser, string netSessionId, int serverId, string ip)
        {
            requestUser.Request = new Request { Ip = ip };
            return this._userDataAccess.LoginOnUser(requestUser, netSessionId, serverId);
        }

        /// <summary>
        ///     The logout.
        /// </summary>
        /// <param name="onuserId">
        ///     The on user id.
        /// </param>
        public void Logout(int onuserId)
        {
            this._userDataAccess.Logout(onuserId);
        }

        /// <summary>
        ///     The re check in.
        /// </summary>
        /// <param name="onuserId">
        ///     The on user id.
        /// </param>
        /// <returns>
        ///     True if re-check is successful.
        /// </returns>
        public bool RecheckIn(int onuserId)
        {
            return this._userDataAccess.RecheckIn(onuserId);
        }

        public IResultInfo ResetPassword(User user, int resetPwdBy)
        {
            var salt = Cryptography.CreateSalt();
            if (string.IsNullOrEmpty(salt))
            {
                throw new ServiceException("Salt cannot be null or empty", "User Service");
            }

            user.Password = Cryptography.CreateHash(user.Password, salt);
            return this._userDataAccess.ResetPassword(user, salt, resetPwdBy);
        }

        public IResultInfo SyncUserInfo(string licenseeName, string userName)
        {
            return _userDataAccess.SyncUserInfo(licenseeName, userName);
        }

        public IResultInfo UpdateInfo(User user, int updatedBy)
        {
            return this._userDataAccess.UpdateInfo(user, updatedBy);
        }

        public User ValidateInternalUser(User requestUser, string licenseeName, long timeStamp, out IResultInfo resultInfo)
        {
            return this._userDataAccess.ValidateInternalUser(requestUser, timeStamp, licenseeName, out resultInfo);
        }

        public User ValidateUserForLrfSystem(User requestUser, string loginToken, out IResultInfo resultInfo)
        {
            var user = GetByUsername(requestUser.Username);

            if (user != null)
            {
                var hashPassword = Cryptography.CreateHash(user.Password + loginToken, user.UserSalt);
                if (requestUser.Password.Equals(hashPassword, StringComparison.OrdinalIgnoreCase))
                {
                    user = this._userDataAccess.ValidateUserForLrfSystem(requestUser, out resultInfo);
                }
                else
                {
                    resultInfo = new ResultInfo { Code = WrongUserNameOrPasswordError, Message = ViewResources.WrongUserNamePassword };
                }
            }
            else
            {
                resultInfo = new ResultInfo { Code = UserNotExistError, Message = ViewResources.UserNotExist };
            }

            return user;
        }

        #endregion Public Methods and Operators
    }
}