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

    public class UserService : IUserService
    {
        private const int UserNotExistError = 100;
        private const int WrongUserNameOrPasswordError = 104;

        private readonly IUserDataAccess _userDataAccess;

        public UserService()
        {
            this._userDataAccess = new UserDataAccess();
        }

        public UserService(IUserDataAccess userDataAccess)
        {
            this._userDataAccess = userDataAccess;
        }

        #region Public Methods and Operators

        public IResultInfo ChangePassword(string userName, string oldPassword, string newPassword, string ip)
        {
            var user = _userDataAccess.GetByUsername(userName, false);
            if (user == null)
            {
                return new ResultInfo { Code = UserNotExistError, Message = ViewResources.UserNotExist };
            }

            var oldSalt = user.UserSalt;
            var salt = Cryptography.CreateSalt();
            var hashedNewPassword = Cryptography.CreateHash(newPassword, salt);
            var hashedOldPassword = Cryptography.CreateHash(oldPassword, oldSalt);

            var requestUser = new User { Request = new Request { Ip = ip }, Username = userName };

            return this._userDataAccess.ChangePassword(requestUser, hashedOldPassword, hashedNewPassword, salt);
        }

        public IList<User> GetByLicensee(int licenseeId, bool isInternal)
        {
            return this._userDataAccess.GetByLicensee(licenseeId, isInternal);
        }

        public User GetByUsername(string userName, bool isInternal, int licenseeId = 0)
        {
            return _userDataAccess.GetByUsername(userName, isInternal, licenseeId);
        }

        public User GetUserInfo(int licenseeId, int userId)
        {
            return this._userDataAccess.GetUserInfo(licenseeId, userId);
        }

        public int LoginOnUser(User requestUser, string netSessionId, int serverId, string ip)
        {
            requestUser.Request = new Request { Ip = ip };
            return this._userDataAccess.LoginOnUser(requestUser, netSessionId, serverId);
        }

        public void Logout(int onuserId)
        {
            this._userDataAccess.Logout(onuserId);
        }

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
            var user = GetByUsername(requestUser.Username, isInternal: false);

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