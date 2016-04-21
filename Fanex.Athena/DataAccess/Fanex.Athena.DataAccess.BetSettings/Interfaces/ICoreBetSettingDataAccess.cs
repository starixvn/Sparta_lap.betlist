// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;

    public interface ICoreBetSettingDataAccess
    {
        IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId);
    }
}