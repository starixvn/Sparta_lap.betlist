namespace Fanex.Athena.BetList.Service.Interfaces
{
    public interface ILogAggregateService
    {
        void Append(string line);

        void SendLog();
    }
}