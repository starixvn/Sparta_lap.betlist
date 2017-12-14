namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    public interface IExchangeCommApiService
    {
        decimal GetCommission(int ibcCustId, int currencyId);

        decimal UpdateCommission(int ibcCustId, int commission);
    }
}