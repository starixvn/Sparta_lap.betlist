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

        IList<User> GetByLicensee(int licenseeId, bool isInternal);

        User GetByUsername(string userName, bool isInternal, int licenseeId = 0);

        User GetUserInfo(int licenseeId, int userId);

        bool RecheckIn(int userId);

        IResultInfo ResetPassword(User user, string salt, int resetPwdBy);

        IResultInfo UpdateInfo(User user, int updatedBy);

        #endregion Public Methods and Operators
    }
}