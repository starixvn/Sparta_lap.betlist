// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICurrencyService.cs" company="Nexcel">
// The i currency service.
// </copyright>
// <summary>
//   The i currency service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Models.MetaModel.Account;

    /// <summary>
    ///     The i currency service.
    /// </summary>
    public interface ICurrencyService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Get currencies by user.
        /// Include more info which indicate what currencies is being assigned to user.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>Currency list.</returns>
        IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId);

        /// <summary>
        /// Assign Currencies To User.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The result info.</returns>
        IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request);

        /// <summary>
        /// Get currency by user id.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The currencies.</returns>
        IDictionary<string, int> GetCurrencyMapByUser(int licenseeId, int userId);

        #endregion Public Methods and Operators
    }
}