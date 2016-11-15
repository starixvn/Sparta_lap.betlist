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
    using User.Common;

    public class OneAuthenticationService : IOneAuthenticationService
    {
        public IResultInfo SignIn(int licenseeId, OneAuthenticationToken tokenInfo)
        {
            throw new NotImplementedException();
        }

        public LicenseeSiteName SingleLicenseeSiteName(string siteName)
        {
            const string SiteNameFilePath = "~/App_Data/LicenseeSiteName.json";
            var defaultValue = new LicenseeSiteName();
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                var items = GetLiceseeSiteNameList(SiteNameFilePath);

                if (items != null)
                {
                    var item = items.Where(i => i.SiteName.Equals(siteName)).FirstOrDefault();
                    return item != null ? item : defaultValue;
                }
            }

            return defaultValue;
        }

        public OneAuthenticationToken GetTokenInfo(string token)
        {
            var oneBookService = new OneBookSecurityWrapper();
            return oneBookService.Authenticate(token);
        }

        protected internal virtual TextReader GetStreamReader(string path)
        {
            return new StreamReader(path);
        }

#pragma warning disable CS3003 // Type is not CLS-compliant
        protected internal virtual ICache GetMemCacheService()
#pragma warning restore CS3003 // Type is not CLS-compliant
        {
            return CacheService.CreateInstance(CacheType.MemoryCache);
        }

        private IList<LicenseeSiteName> GetLiceseeSiteNameList(string filePath)
        {
            const string LicenseeSiteNameCacheKey = "OneAuthenticationService_LicenseeSiteNameCacheKey";
            const int CacheTimeoutMinute = 30;
            var cacheService = GetMemCacheService();
            IList<LicenseeSiteName> result = cacheService.Get<IList<LicenseeSiteName>>(LicenseeSiteNameCacheKey);

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
                            cacheService.Set(LicenseeSiteNameCacheKey, result, DateTime.Now.AddMinutes(CacheTimeoutMinute));
                        }
                    }
                }
            }

            return result;
        }
    }
}