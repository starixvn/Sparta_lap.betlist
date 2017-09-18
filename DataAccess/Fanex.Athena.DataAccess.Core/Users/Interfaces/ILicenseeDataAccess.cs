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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IResultInfo IsAllowUpdateMemberBAStatus(Request request, int licenseeId, out bool isAllow);

        IList<Licensee> GetByUser(int userId);

        Licensee GetLicenseeByName(string licenseeName);
    }
}