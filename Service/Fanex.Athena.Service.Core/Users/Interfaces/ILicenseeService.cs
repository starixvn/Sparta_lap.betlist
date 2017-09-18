// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseeService.cs" company="">
// The licensee service.
// </copyright>
// <summary>
//   The licensee service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ILicenseeService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IResultInfo IsAllowUpdateMemberBAStatus(Request request, int licenseeId, out bool isAllow);

        bool IsLicenseeBelongToUser(int licenseeId, int userId);

        IList<Licensee> GetByUser(int userId);

        Licensee GetLicenseeByName(string licenseeName);
    }
}