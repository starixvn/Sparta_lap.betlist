using Fanex.Athena.Model.Core.DTO.Common;

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    public interface IExchangeCommApiService
    {
        IResultInfo<decimal> GetCommission(int licenseeId, int custId, int currencyId);

        IResultInfo<decimal> UpdateCommission(int licenseeId, int userId, int custId, decimal commission);
    }
}