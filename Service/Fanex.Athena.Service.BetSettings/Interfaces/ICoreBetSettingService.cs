// -----------------------------------------------------------------------
// <copyright file="ICoreBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Model.Customer.DTO;

    public interface ICoreBetSettingService
    {
        CustomerInfo GetCustomerInfo(int custId);

        Task<bool> IsBetSettingEditableAsync();

        Task<IEnumerable<BetSettingProduct>> GetBetSettingProductsAsync(int custId);
    }
}