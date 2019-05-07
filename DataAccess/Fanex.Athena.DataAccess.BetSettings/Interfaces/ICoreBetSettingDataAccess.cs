// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;

    public interface ICoreBetSettingDataAccess
    {
        Task<IEnumerable<BetSettingProduct>> GetBetSettingProductsAsync(int licenseeId);
    }
}