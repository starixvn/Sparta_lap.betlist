using System;
using System.Threading.Tasks;
using Fanex.Athena.DataAccess.Core.Users.Interfaces;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;
using Fanex.Athena.Resources;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Fanex.Common;

namespace Fanex.Athena.Service.Core.Users
{
    public class AuthService : IAuthService
    {
        private const int UserNotExistError = 100;
        private const int WrongUserNameOrPasswordError = 104;

        private readonly IUserDataAccess _userDataAccess;

        private readonly IAuthDataAccess _authenticationDataAccess;

        public AuthService(
            IUserDataAccess userDataAccess,
            IAuthDataAccess authenticationDataAccess)
        {
            _userDataAccess = userDataAccess;
            _authenticationDataAccess = authenticationDataAccess;
        }

        public async Task<int> LoginOnUserAsync(User requestUser, string netSessionId, int serverId, string ip)
        {
            requestUser.Request = new Request { Ip = ip };
            return await this._authenticationDataAccess.LoginOnUserAsync(requestUser, netSessionId, serverId).ConfigureAwait(false);
        }

        public async Task LogoutAsync(int onuserId)
        {
            await this._authenticationDataAccess.LogoutAsync(onuserId).ConfigureAwait(false);
        }

        public async Task<IResultInfo<User>> ValidateUserForLrfAsync(User requestUser, string loginToken)
        {
            var user = _userDataAccess.GetByUsername(requestUser.Username, isInternal: false);

            if (user == null)
            {
                return new ResultInfo<User> { Code = UserNotExistError, Message = ViewResources.UserNotExist };
            }

            var hashPassword = Cryptography.CreateHash(user.Password + loginToken, user.UserSalt);
            if (!requestUser.Password.Equals(hashPassword, StringComparison.OrdinalIgnoreCase))
            {
                return new ResultInfo<User> { Code = WrongUserNameOrPasswordError, Message = ViewResources.WrongUserNamePassword };
            }

            return await _authenticationDataAccess.ValidateUserForLrfAsync(requestUser).ConfigureAwait(false);
        }
    }
}