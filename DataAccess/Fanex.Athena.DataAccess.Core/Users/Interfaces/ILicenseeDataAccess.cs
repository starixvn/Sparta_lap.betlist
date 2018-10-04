// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseeDataAccess.cs" company="">
// a
// </copyright>
// <summary>
//   The i licensee data access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ILicenseeDataAccess
    {
        IResultInfo<bool> IsAllowUpdateMemberBAStatus(Request request, int licenseeId);

        bool IsLicenseeBelongToUser(int licenseeId, int userId);

        IList<Licensee> GetByUser(int userId);

        Licensee GetLicenseeByName(string licenseeName);
    }
}