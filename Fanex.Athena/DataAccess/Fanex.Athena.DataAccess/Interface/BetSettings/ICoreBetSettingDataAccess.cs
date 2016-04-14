// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;

    public interface ICoreBetSettingDataAccess
    {
        IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId);
    }
}