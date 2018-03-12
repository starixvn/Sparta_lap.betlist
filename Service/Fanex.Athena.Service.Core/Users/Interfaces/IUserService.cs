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

    public interface IUserService
    {
        IResultInfo ChangePassword(string userName, string oldPassword, string newPassword, string ip);

        IList<User> GetByLicensee(int licenseeId, bool isInternal);

        User GetUserInfo(int licenseeId, int userId);

        bool RecheckIn(int onuserId);

        IResultInfo ResetPassword(User user, int resetPwdBy);

        IResultInfo UpdateInfo(User user, int updatedBy);

        User GetByUsername(string userName, bool isInternal, int licenseeId = 0);
    }
}