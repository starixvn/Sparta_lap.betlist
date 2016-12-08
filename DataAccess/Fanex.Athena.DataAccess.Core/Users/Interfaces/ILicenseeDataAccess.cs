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
    using Fanex.Athena.Model.Core.DTO.Users;

    /// <summary>
    /// The i licensee data access.
    /// </summary>
    public interface ILicenseeDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IResultInfo IsAllowUpdateMemberBAStatus(Request request, int licenseeId, out bool isAllow);

        int GetBelongingStatus(int licenseeId, int custId);

        IList<Licensee> GetByUser(int userId);

        IList<LicenseeCustomer> GetCustByLicensee(int licenseeId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        string GetNumberOfOnlineUser();

        Licensee GetLicenseeByName(string licenseeName);
    }
}