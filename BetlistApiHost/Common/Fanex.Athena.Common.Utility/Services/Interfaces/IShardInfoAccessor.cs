namespace Fanex.Athena.Common.Utility.Services
{
    public interface IShardInfoAccessor
    {
        bool TryUseShardDB(string key, out string dbServer);

        string GetShardSPKey(string key);

        bool UseShardDB { get; }
    }
}
