using Fanex.Athena.Common.Utility.Services;
using Fanex.Data;
using System;
using System.Configuration;

namespace Fanex.Athena.App_Start
{
    public static class DbSettingProviderManagerExtension
    {
        const string SpNameConfigSource = "~/SpNameConfig/FanexSpName.xml";

        [CLSCompliant(false)]
        public static DbSettingProviderManager UseShardDbSettingProvider(this DbSettingProviderManager dbSettingProviderManager)
            => dbSettingProviderManager.Use(new ShardDbSettingProvider(new ShardInfoAccessor(),
                    resourcePath: SpNameConfigSource));

    }
}