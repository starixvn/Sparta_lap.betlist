// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILicenseeService.cs" company="Nexcel">
// The licensee service.
// </copyright>
// <summary>
//   The licensee service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Users
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;

    /// <summary>
    ///     The i licensee service.
    /// </summary>
    public interface ILicenseeService
    {
        #region Public Methods and Operators

        IResultInfo IsAllowUpdateMemberBaStatus(Request request, int licenseeId, out bool isAllow);

        bool IsLicenseeBelongToUser(int licenseeId, int userId);

        IList<Licensee> GetByUser(int userId);

        IList<Currency> GetCurrencies(int licenseeId, int userId);

        IList<LicenseeCustomer> GetCustByLicensee(int licenseeId);

        IList<int> GetByResellerId(int userId, int resellerId);

        string GetNumberOfOnlineUser();

        #endregion Public Methods and Operators
    }
}