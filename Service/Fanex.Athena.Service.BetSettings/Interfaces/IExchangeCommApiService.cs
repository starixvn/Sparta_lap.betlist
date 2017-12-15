namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    public interface IExchangeCommApiService
    {
        decimal GetCommission(int custId, int currencyId);

        decimal UpdateCommission(int userId, int custId, decimal commission);
    }
}