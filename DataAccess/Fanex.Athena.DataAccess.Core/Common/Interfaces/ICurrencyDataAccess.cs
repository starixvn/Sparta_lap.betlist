// -----------------------------------------------------------------------
// <copyright file="ICurrencyDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Core.MetaModel.Account;

    public interface ICurrencyDataAccess
    {
        Task<IList<CurrencyUser>> GetCurrenciesByUser(int licenseeId, int userId);

        Task<IResultInfo> AssignCurrenciesByUser(AssignCurrencyMetaModel request);
    }
}