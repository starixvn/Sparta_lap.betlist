namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    public interface IExchangeCommApiService
    {
        decimal GetCommission(int licenseeId, int custId, int currencyId);

        decimal UpdateCommission(int licenseeId, int userId, int custId, decimal commission);
    }
}