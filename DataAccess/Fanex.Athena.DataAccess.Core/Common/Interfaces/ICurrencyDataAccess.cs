// -----------------------------------------------------------------------
// <copyright file="ICurrencyDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Core.MetaModel.Account;

    public interface ICurrencyDataAccess
    {
        IList<CurrencyUser> GetCurrencies(int licenseeId);

        IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId);

        IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request);
    }
}