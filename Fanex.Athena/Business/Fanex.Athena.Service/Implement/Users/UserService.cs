// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel">
// The user service.
// </copyright>
// <summary>
//   The user service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Users;
    using Fanex.Athena.DataAccess.Interface.Users;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Exception;
    using Fanex.Athena.Service.Interface.Users;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging;

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
            try
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
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "ChangePassword");
            }
        }

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
        /// The system user list.
        /// </returns>
        public List<User> GetByLicensee(int licenseeId, bool isInternal)
        {
            try
            {
                return this._userDataAccess.GetByLicensee(licenseeId, isInternal);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "GetByLicensee");
            }
        }

        /// <summary>
        /// Get by username.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <returns>User value.</returns>
        public User GetByUsername(string userName)
        {
            var param = new
            {
                Username = userName,
            };
            try
            {
                using (var objectDb = new ObjectDb("Lrf_UserInfoSelByUserName"))
                {
                    return objectDb.Query<User>(param).FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "GetByUsername");                
            }
        }

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
        /// The system User.
        /// </returns>
        public User GetUserInfo(int licenseeId, int userId)
        {
            try
            {
                return this._userDataAccess.GetUserInfo(licenseeId, userId);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "GetUserInfo");
            }
        }

        /// <summary>
        ///     The is user name existed.
        /// </summary>
        /// <param name="userName">
        ///     The user name.
        /// </param>
        /// <returns>
        ///     Return True if user exist.
        /// </returns>
        public bool IsUserNameExisted(string userName)
        {
            try
            {
                return this._userDataAccess.IsUsernameExited(userName);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "IsUserNameExisted");
            }
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
            try
            {
                requestUser.Request = new Request { Ip = ip };
                return this._userDataAccess.LoginOnUser(requestUser, netSessionId, serverId);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "LoginOnUser");
            }
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
            try
            {
                return this._userDataAccess.RecheckIn(onuserId);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "ReCheckin");
            }
        }

        /// <summary>
        ///     The reset password.
        /// </summary>
        /// <param name="user">
        ///     The system user.
        /// </param>
        /// <param name="resetPwdBy">
        ///      The user reset password.
        /// </param>
        /// <returns>
        /// The result information.
        /// </returns>
        public IResultInfo ResetPassword(User user, int resetPwdBy)
        {
            try
            {
                var salt = Cryptography.CreateSalt();
                if (string.IsNullOrEmpty(salt))
                {
                    throw new ServiceException("Salt cannot be null or empty", "User Service");
                }

                user.Password = Cryptography.CreateHash(user.Password, salt);
                return this._userDataAccess.ResetPassword(user, salt, resetPwdBy);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "ResetPassword");
            }
        }

        /// <summary>
        ///     The update information.
        /// </summary>
        /// <param name="user">
        ///     The system user.
        /// </param>
        /// <param name="updatedBy">
        ///     The updated by.
        /// </param>
        /// <returns>
        /// The result information.
        /// </returns>
        public IResultInfo UpdateInfo(User user, int updatedBy)
        {
            try
            {
                return this._userDataAccess.UpdateInfo(user, updatedBy);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "UpdateInfo");
            }
        }

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
        ///     The result information.
        /// </param>
        /// <returns>
        /// The system user.
        /// </returns>
        /// <exception cref="ServiceException">
        /// The service exception.
        /// </exception>
        public User ValidateInternalUser(User requestUser, string licenseeName, long timeStamp, out IResultInfo resultInfo)
        {
            try
            {
                return this._userDataAccess.ValidateInternalUser(requestUser, timeStamp, licenseeName, out resultInfo);
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "ValidateInternalUser");
            }
        }

        /// <summary>
        ///     The validate user for  system.
        /// </summary>
        /// <param name="requestUser">
        ///     The request user.
        /// </param>
        /// <param name="loginToken">
        ///     The login token.
        /// </param>
        /// <param name="resultInfo">
        ///     The result information.
        /// </param>
        /// <returns>
        /// The system user.
        /// </returns>
        public User ValidateUserForLrfSystem(User requestUser, string loginToken, out IResultInfo resultInfo)
        {
            try
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
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
                throw new ServiceException(exception.Message, "ValidateUserForLrfSystem");
            }
        }

        #endregion Public Methods and Operators
    }
}