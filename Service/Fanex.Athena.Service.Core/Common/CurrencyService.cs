// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrencyService.cs" company="">
// The currency service.
// </copyright>
// <summary>
//   The currency service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Core.MetaModel.Account;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Utility;

    /// <summary>
    ///     The currency service.
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyDataAccess _currencyDataAccess;

        public CurrencyService()
        {
            this._currencyDataAccess = new CurrencyDataAccess();
        }

        public CurrencyService(ICurrencyDataAccess currencyDataAccess)
        {
            this._currencyDataAccess = currencyDataAccess;
        }

        public IList<CurrencyUser> GetCurrencies(int licenseeId)
        {
            return _currencyDataAccess.GetCurrencies(licenseeId);
        }

        public IDictionary<int, string> GetActiveCurrenciesByUser(int licenseeId, int userId)
        {
            var currencies = GetCurrenciesByUser(licenseeId, userId);
            if (licenseeId == LicenseeIdentityDefinition.KeenOcean)
            {
                currencies = KeanOceanBusinessHelper.ModifyCurrenciesForKeenOcean(currencies);
            }

            return currencies
                .Where(i => i.IsActive)
                .Select(p => new { p.CurrencyId, p.CurrencyName })
                .ToDictionary(kvp => kvp.CurrencyId, kvp => kvp.CurrencyName);
        }

        /// <summary>
        /// Get currency by user identity.
        /// No have caching.
        /// </summary>
        /// <param name="licenseeId">The licensee identity.</param>
        /// <param name="userId">The user identity.</param>
        /// <returns>The currency list.</returns>
        public IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId)
        {
            return _currencyDataAccess.GetCurrenciesByUser(licenseeId, userId);
        }

        public IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request)
        {
            return _currencyDataAccess.AssignCurrenciesByUser(request);
        }
    }
}