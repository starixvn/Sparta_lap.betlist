using Fanex.Athena.Model.Core.DTO.Common;

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    public interface IExchangeCommApiService
    {
        IResultInfo<double> GetCommission(int licenseeId, int custId);

        IResultInfo<double> UpdateCommission(int licenseeId, int userId, int custId, double oldCommission, double newCommission);
    }
}