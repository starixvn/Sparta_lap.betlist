namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System.Data;

    public interface IBasicInfoApiWrapper
    {
        DataTable GetGameKind(string casinoNo);

        DataTable GetGameType(int gameKind,int sbCustID,int sbExchangeID);
    }
}