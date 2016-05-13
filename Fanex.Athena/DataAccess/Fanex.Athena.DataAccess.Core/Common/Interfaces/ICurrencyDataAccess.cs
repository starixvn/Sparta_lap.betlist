// -----------------------------------------------------------------------
// <copyright file="ICurrencyDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Created</change>
// </history>
// -----------------------------------------------------------------------
namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Models.MetaModel.Account;

    /// <summary>
    /// The i currency data access.
    /// </summary>
    public interface ICurrencyDataAccess
    {
        #region Public Methods and Operators

        IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId);

        IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request);

        #endregion Public Methods and Operators
    }
}