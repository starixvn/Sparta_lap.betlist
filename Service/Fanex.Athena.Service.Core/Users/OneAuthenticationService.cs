namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Caching;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using User.Common;

    public class OneAuthenticationService : IOneAuthenticationService
    {
        private readonly ISessionService _sessionService;
        private readonly ILicenseeService _licenseeService;
        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public OneAuthenticationService() : this(null, null, null)
        {
        }

        public OneAuthenticationService(ISessionService sessionService, ILicenseeService licenseeService, IFormsAuthenticationService formsAuthenticationService)
        {
            _sessionService = sessionService ?? new SessionService();
            _licenseeService = licenseeService ?? new LicenseeService();
            _formsAuthenticationService = formsAuthenticationService ?? new FormsAuthenticationService();
        }

        public IResultInfo SignIn(HttpContextBase currentContext, int licenseeId, OneAuthenticationToken tokenInfo)
        {
            var result = new ResultInfo();
            var userSession = _sessionService.UserSession;
            var licenseeSession = _sessionService.LicenseeSession;
            if (IsAuthenticated(currentContext, userSession, licenseeSession))
            {
                var isSameSession = VerifySessionWithToken(userSession, licenseeSession, tokenInfo);
                if (isSameSession)
                {
                    var language = Util.MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                        .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
                    if (!language.Equals(userSession.Language))
                    {
                        _sessionService.UpdateLanguage(language);
                    }
                }
                else
                {
                    SetAuthorize(licenseeId, tokenInfo);
                }
            }
            else
            {
                SetAuthorize(licenseeId, tokenInfo);
            }

            return result;
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

        protected internal void SetAuthorize(int licenseeId, OneAuthenticationToken tokenInfo)
        {

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

        private bool VerifySessionWithToken(UserSession userSession, LicenseeSession licenseeSession, OneAuthenticationToken tokenInfo)
        {
            throw new NotImplementedException();
        }

        private bool IsAuthenticated(HttpContextBase currentContext, UserSession userSession, LicenseeSession licenseeSession)
        {
            var identity = currentContext.User.Identity;
            var identityValidation = identity != null && identity.IsAuthenticated && !string.IsNullOrEmpty(identity.Name);
            var sessionValidation = userSession != null && licenseeSession != null && userSession.UserId > 0 &&
                !string.IsNullOrEmpty(userSession.VendorID) && !string.IsNullOrEmpty(userSession.VendorIdentityID)
                && userSession.IsInternal == true && licenseeSession.LicenseeId > 0 && userSession.ApplicationMode == AppMode.Parasitic;
            return identityValidation && sessionValidation;

        }

    }
}