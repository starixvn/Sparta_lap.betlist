// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrencyService.cs" company="">
// The currency service.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Core.MetaModel.Account;
    using Fanex.Athena.Service.Core.Common.Interfaces;

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

        public IEnumerable<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId) => _currencyDataAccess.GetCurrenciesByUser(licenseeId, userId);

        public IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request)
        {
            return _currencyDataAccess.AssignCurrenciesByUser(request);
        }
    }
}