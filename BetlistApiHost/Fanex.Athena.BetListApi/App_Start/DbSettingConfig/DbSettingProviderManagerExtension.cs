using System;
using Fanex.Athena.Common.Utility.Services;
using Fanex.Data;

namespace Fanex.Athena.App_Start
{
    public static class DbSettingProviderManagerExtension
    {
        private const string SpNameConfigSource = "~/SpNameConfig/FanexSpName.xml";

        [CLSCompliant(false)]
        public static DbSettingProviderManager UseShardDbSettingProvider(this DbSettingProviderManager dbSettingProviderManager)
            => dbSettingProviderManager.Use(new ShardDbSettingProvider(new ShardInfoAccessor(),
                    resourcePath: SpNameConfigSource));
    }
}