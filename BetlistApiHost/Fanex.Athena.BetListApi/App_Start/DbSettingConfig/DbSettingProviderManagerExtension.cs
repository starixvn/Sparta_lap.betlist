using System;
using Fanex.Athena.Common.Utility.Services;
using Fanex.Data;

namespace Fanex.Athena.App_Start
{
    public static class DbSettingProviderManagerExtension
    {
        [CLSCompliant(false)]
        public static DbSettingProviderManager UseShardDbSettingProvider(this DbSettingProviderManager dbSettingProviderManager)
            => dbSettingProviderManager.Register(ShardDbSettingProvider.ProviderName, new ShardDbSettingProvider(new ShardInfoAccessor()).CreateObjectDbFunc);
    }
}