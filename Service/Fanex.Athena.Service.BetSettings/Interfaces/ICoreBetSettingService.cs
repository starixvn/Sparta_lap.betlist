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
    using Session.Model;

    public interface ICoreBetSettingService
    {
        CustomerInfo GetCustomerInfo(int custId, int licenseeId);

        bool IsBetSettingEditable(UserSession userSession);

        Task<IEnumerable<BetSettingProduct>> GetBetSettingProductsAsync(int custId, int licenseeId);
    }
}