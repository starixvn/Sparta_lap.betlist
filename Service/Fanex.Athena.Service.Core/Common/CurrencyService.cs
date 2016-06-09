// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrencyService.cs" company="Nexcel">
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

        /// <summary>
        ///  Only get active currency.
        /// Having caching.
        /// </summary>
        /// <param name="licenseeId">The licensee identity.</param>
        /// <param name="userId">The user identity.</param>
        /// <returns>The currency dictionary.</returns>
        public IDictionary<string, int> GetCurrencyMapByUser(int licenseeId, int userId)
        {
            var currencies = GetCurrenciesByUser(licenseeId, userId);
            if (licenseeId == Licensees.KeenOcean)
            {
                currencies = ModifyCurrenciesForKeenOcean(currencies);
            }

            return currencies
                .Where(i => i.IsActive == true)
                .Select(p => new { p.CurrencyName, p.CurrencyId })
                .ToDictionary(kvp => kvp.CurrencyName, kvp => kvp.CurrencyId);
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

        private static IList<CurrencyUser> ModifyCurrenciesForKeenOcean(IList<CurrencyUser> currencies)
        {
            currencies = currencies.Where(x => x.CurrencyId != CurrencyType.Cny).ToList();

            var currency = currencies.Where(x => x.CurrencyId == CurrencyType.Hkd && x.CurrencyName.Equals("CNY") == false)
                                     .FirstOrDefault();
            if (currency != null)
            {
                currency.CurrencyName = "CNY";
            }

            return currencies;
        }
    }
}