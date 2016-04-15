// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseeService.cs" company="Nexcel">
// The licensee service.
// </copyright>
// <summary>
//   The licensee service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;

    /// <summary>
    ///     The i licensee service.
    /// </summary>
    public interface ILicenseeService
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IResultInfo IsAllowUpdateMemberBAStatus(Request request, int licenseeId, out bool isAllow);

        bool IsLicenseeBelongToUser(int licenseeId, int userId);

        IList<Licensee> GetByUser(int userId);

        IList<Currency> GetCurrencies(int licenseeId, int userId);

        IList<LicenseeCustomer> GetCustByLicensee(int licenseeId);

        IEnumerable<int> GetByResellerId(int userId, int resellerId);

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        string GetNumberOfOnlineUser();

        #endregion Public Methods and Operators
    }
}