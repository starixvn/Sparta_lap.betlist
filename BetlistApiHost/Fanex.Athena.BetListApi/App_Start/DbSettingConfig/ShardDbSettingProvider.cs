using System;
using System.Data.SqlClient;
using Fanex.Athena.Common.Utility.Services;
using Fanex.Data;
using Fanex.Data.SqlServer;

namespace Fanex.Athena.App_Start
{
    [CLSCompliant(false)]
    public class ShardDbSettingProvider
    {
        private readonly IShardInfoAccessor _shardInfoAccessor;

        public ShardDbSettingProvider(IShardInfoAccessor shardInfoAccessor)
        {
            _shardInfoAccessor = shardInfoAccessor;
        }

        public static string ProviderName => nameof(ShardDbSettingProvider);

        public IObjectDb CreateObjectDbFunc(string settingKey, int? commandTimeout, string serverIp)
        {
            if (_shardInfoAccessor.TryUseShardDB(settingKey, out var dbServer))
            {
                serverIp = dbServer;
            }

            return new SqlObjectDb(settingKey, commandTimeout, serverIp);
        }
    }
}