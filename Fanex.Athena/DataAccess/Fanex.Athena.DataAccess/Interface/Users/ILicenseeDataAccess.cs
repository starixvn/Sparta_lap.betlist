// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseeDataAccess.cs" company="Nexcel">
// a
// </copyright>
// <summary>
//   The i licensee data access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Users
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;

    /// <summary>
    /// The i licensee data access.
    /// </summary>
    public interface ILicenseeDataAccess
    {
        #region Public Methods and Operators

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IResultInfo IsAllowUpdateMemberBaStatus(Request request, int licenseeId, out bool isAllow);

        int GetBelongingStatus(int licenseeId, int custId);

        IList<Licensee> GetByUser(int userId);

        IList<Currency> GetCurrencies(int licenseeId, int userId);

        IList<LicenseeCustomer> GetCustByLicensee(int licenseeId);

        IList<int> GetByResellerId(int userId, int resellerId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        string GetNumberOfOnlineUser();

        #endregion Public Methods and Operators
    }
}