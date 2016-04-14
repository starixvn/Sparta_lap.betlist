// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.BetSettings
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;

    public interface ICoreBetSettingService
    {
        IEnumerable<BetSettingProduct> GetBetSettingProducts(int licenseeId);
    }
}