namespace Fanex.Athena.Service.Casino.Interfaces
{
    using System.Data;

    public interface IBasicInfoApiWrapper
    {
        DataTable GetGameKind(string casinoNo);

        DataTable GetGameType(int gameKind);
    }
}