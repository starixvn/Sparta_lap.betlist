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
    using Interfaces;
    using Newtonsoft.Json;
    using Session.Interfaces;
    using Session.Model;

    public class LicenseeSettingService : ILicenseeSettingService
    {
        private const string FakeLicenseeSettingsCacheKey = "LRF_FakeLicenseeSettings";

        private readonly IMemoryCacheService _cache;

        private readonly ISessionStorage _sessionStorage;

        public LicenseeSettingService(IMemoryCacheService cache, ISessionStorage sessionStorage)
        {
            _cache = cache;
            _sessionStorage = sessionStorage;
        }

        public bool IsFakeLicensee()
        {
            return GetFakeLicenseeSetting() != null;
        }

        public bool IsAuthorizedOnBetList()
        {
            var authorized = _sessionStorage.LicenseeSession != null;
            if (authorized)
            {
                var setting = GetFakeLicenseeSetting();
                if (setting != null)
                {
                    authorized = !setting.DisableBetList;
                }
            }

            return authorized;
        }

        public FakeLicenseeSetting GetFakeLicenseeSetting()
        {
            if (_sessionStorage.LicenseeSession == null)
            {
                return null;
            }

            var settings = _cache.Get<IEnumerable<FakeLicenseeSetting>>(FakeLicenseeSettingsCacheKey);
            if (settings == null)
            {
                settings = GetFakeLicenseeSettingList();
                _cache.Set(FakeLicenseeSettingsCacheKey, settings);
            }

            var licenseeName = _sessionStorage.LicenseeSession.LicenseeName;
            return settings.FirstOrDefault(x => x.SiteName.Equals(licenseeName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<FakeLicenseeSetting> GetFakeLicenseeSettingList()
        {
            IEnumerable<FakeLicenseeSetting> result = null;
            var jsonContent = GetFakeLicenseeSettingContent();

            if (!string.IsNullOrEmpty(jsonContent))
            {
                result = JsonConvert.DeserializeObject<IEnumerable<FakeLicenseeSetting>>(jsonContent);
            }

            return result ?? new List<FakeLicenseeSetting>();
        }

        protected internal virtual string GetFakeLicenseeSettingContent()
        {
            const string FilePath = "~/App_Data/FakeLicenseeSetting.json";
            using (var reader = new StreamReader(HostingEnvironment.MapPath(FilePath)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}