// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICurrencyService.cs" company="">
// The i currency service.
// </copyright>
// <summary>
//   The i currency service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Core.MetaModel.Account;

    public interface ICurrencyService
    {
        IList<CurrencyUser> GetCurrencies(int licenseeId);

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

        IDictionary<int, string> GetActiveCurrenciesByUser(int licenseeId, int userId);
    }
}