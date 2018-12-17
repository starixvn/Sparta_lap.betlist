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
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Interfaces;
    using Newtonsoft.Json;

    public class LicenseeSettingService : ILicenseeSettingService
    {
        private const string FakeLicenseeSettingsCacheKey = "LRF_FakeLicenseeSettings";

        private readonly IMemoryCacheService _cache;

        public LicenseeSettingService(IMemoryCacheService cache)
        {
            _cache = cache;
        }

        public bool IsFakeLicensee(string licenseeName)
        {
            return GetFakeLicenseeSetting(licenseeName) != null;
        }

        public bool IsAuthorizedOnBetList(string licenseeName)
        {
            var authorized = !string.IsNullOrEmpty(licenseeName);
            if (authorized)
            {
                var setting = GetFakeLicenseeSetting(licenseeName);
                if (setting != null)
                {
                    authorized = !setting.DisableBetList;
                }
            }

            return authorized;
        }


        public FakeLicenseeSetting GetFakeLicenseeSetting(string licenseeName)
        {
            return GetAndCacheFakeLicenseeSettingList()?.FirstOrDefault(x => x.SiteName.Equals(licenseeName, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<FakeLicenseeSetting> GetAndCacheFakeLicenseeSettingList()
        {
            var settings = _cache.Get<IEnumerable<FakeLicenseeSetting>>(FakeLicenseeSettingsCacheKey);
            if (settings == null)
            {
                settings = GetFakeLicenseeSettingList();
                if (settings.Any())
                {
                    _cache.Set(FakeLicenseeSettingsCacheKey, settings);
                }
            }

            return settings;
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