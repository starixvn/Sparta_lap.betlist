using System;
using System.Web;

namespace Fanex.Athena.Common.Utility.Services
{
    public class ShardInfoAccessor : IShardInfoAccessor
    {
        private const string PrefixSharding = "shard_";
        private const string PrefixSiteDB = "sitedb_";

        public bool TryUseShardDB(string key, out string dbServer)
        {
            if (IsShardSP(key) && UseShardDB)
            {
                dbServer = SiteDBServer;
                return true;
            }
            dbServer = null;
            return false;
        }

        public bool UseShardDB => !ForceIgnoreSharding && !string.IsNullOrEmpty(SiteDBServer);

        public string GetShardSPKey(string key) => key.StartsWith(PrefixSiteDB, StringComparison.OrdinalIgnoreCase)
            ? key : $"SiteDB_{key.Substring(PrefixSharding.Length)}";

        private static bool IsShardSP(string key) => key.StartsWith(PrefixSharding, StringComparison.OrdinalIgnoreCase)
            || key.StartsWith(PrefixSiteDB, StringComparison.OrdinalIgnoreCase);

        private static string SiteDBServer => HttpContext.Current?.Request.Headers["X-SiteDBServer"];

        private static bool ForceIgnoreSharding => bool.TrueString.Equals(HttpContext.Current?.Request.Headers["X-IgnoreSharding"], StringComparison.OrdinalIgnoreCase);
    }
}