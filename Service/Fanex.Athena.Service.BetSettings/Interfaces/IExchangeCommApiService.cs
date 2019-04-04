namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IExchangeCommApiService
    {
        IResultInfo<double> GetCommission(int licenseeId, int custId);

        Task<IResultInfo<double>> UpdateCommissionAsync(int licenseeId, int userId, int custId, double oldCommission, double newCommission);
    }
}