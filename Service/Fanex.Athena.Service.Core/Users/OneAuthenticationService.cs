namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Caching;
    using Newtonsoft.Json;

    public class OneAuthenticationService : IOneAuthenticationService
    {
        public IResultInfo InsertLicenseeUser(OneAuthenticationToken tokenInfo)
        {
            throw new NotImplementedException();
        }

        public IResultInfo SignIn(int licenseeId, OneAuthenticationToken tokenInfo)
        {
            throw new NotImplementedException();
        }

        public LicenseeSiteName SingleLicenseeSiteName(string siteName)
        {
            const string SiteNameFilePath = "~/App_Data/LicenseeSiteName.json";

            if (!string.IsNullOrWhiteSpace(siteName))
            {
                var items = GetLiceseeSiteNameList(SiteNameFilePath);

                if (items != null)
                {
                    var item = items.Where(i => i.SiteName.Equals(siteName)).FirstOrDefault();
                    return item != null ? item : new LicenseeSiteName();
                }
            }

            return new LicenseeSiteName();
        }

        protected internal virtual TextReader GetStreamReader(string path)
        {
            return new StreamReader(path);
        }

        private IList<LicenseeSiteName> GetLiceseeSiteNameList(string filePath)
        {
            const string LicenseeSiteNameCacheKey = "OneAuthenticationService_LicenseeSiteNameCacheKey";
            const int CacheTimeoutMinute = 30;
            var memCacheService = Fanex.Caching.CacheService.CreateInstance(CacheType.MemoryCache);
            IList<LicenseeSiteName> result = memCacheService.Get<IList<LicenseeSiteName>>(LicenseeSiteNameCacheKey);

            if (result == null || !result.Any())
            {
                using (var _textReader = GetStreamReader(HostingEnvironment.MapPath(filePath)))
                {
                    var temp = _textReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(temp))
                    {
                        result = JsonConvert.DeserializeObject<IList<LicenseeSiteName>>(temp);
                        if (result.Any())
                        {
                            memCacheService.Set(LicenseeSiteNameCacheKey, result, DateTime.Now.AddMinutes(CacheTimeoutMinute));
                        }
                    }
                }
            }

            return result;
        }

        public OneAuthenticationToken GetTokenInfo(string token)
        {
            throw new NotImplementedException();
        }
    }
}