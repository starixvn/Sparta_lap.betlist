// -----------------------------------------------------------------------
// <copyright file="MoveBetService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Interfaces;
    using Newtonsoft.Json;
    using Users;
    using Users.Interfaces;

    public class FakeLicenseeSettingService : IFakeLicenseeSettingService
    {
        private const string CacheKey = "LRF_FakeLicenseeSettings";

        private readonly IMemoryCacheService _cache;

        private readonly ISessionService _sessionService;

        public FakeLicenseeSettingService() : this(null, null)
        {
        }

        public FakeLicenseeSettingService(IMemoryCacheService cache, ISessionService sessionService)
        {
            _cache = cache ?? new MemoryCacheService();
            _sessionService = sessionService ?? new SessionService();
        }

        public bool IsFakeLicensee()
        {
            return GetSetting() == null;
        }

        public FakeLicenseeSetting GetSetting()
        {
            if (_sessionService.LicenseeSession == null)
            {
                return null;
            }

            var settings = _cache.Get<IEnumerable<FakeLicenseeSetting>>(CacheKey);
            if (settings == null)
            {
                settings = GetSettingList();
                _cache.Set(CacheKey, settings);
            }

            var licenseeName = _sessionService.LicenseeSession.LicenseeName;
            return settings.FirstOrDefault(x => x.SiteName.Equals(licenseeName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<FakeLicenseeSetting> GetSettingList()
        {
            IEnumerable<FakeLicenseeSetting> result = null;
            var jsonContent = GetSettingFileContent();

            if (!string.IsNullOrEmpty(jsonContent))
            {
                result = JsonConvert.DeserializeObject<IEnumerable<FakeLicenseeSetting>>(jsonContent);
            }

            return result ?? new List<FakeLicenseeSetting>();
        }

        protected internal virtual string GetSettingFileContent()
        {
            const string FilePath = "~/App_Data/FakeLicenseeSetting.json";
            using (var reader = new StreamReader(HostingEnvironment.MapPath(FilePath)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}