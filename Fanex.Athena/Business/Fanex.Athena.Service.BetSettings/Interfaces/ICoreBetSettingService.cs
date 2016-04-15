// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetSettings;

    public interface ICoreBetSettingService
    {
        IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId);
    }
}