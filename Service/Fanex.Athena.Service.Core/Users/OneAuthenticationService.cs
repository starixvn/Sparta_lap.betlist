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
    using Model.Core.DTO;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using Resources;
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

#pragma warning disable CS3008 // Identifier is not CLS-compliant
        protected internal TextReader _textReader { get; set; }
#pragma warning restore CS3008 // Identifier is not CLS-compliant

        public IResultInfo SignIn(HttpContextBase currentContext, int licenseeId, OneAuthenticationToken tokenInfo)
        {
            var userSession = _sessionService.UserSession;
            var licenseeSession = _sessionService.LicenseeSession;
            var language = Util.MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                       .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
            if (IsAuthenticated(currentContext, userSession, licenseeSession))
            {
                var isSameSession = VerifySessionWithToken(userSession, licenseeSession, tokenInfo, licenseeId);
                if (isSameSession)
                {
                    if (!language.Equals(userSession.Language))
                    {
                        _sessionService.UpdateLanguage(language);
                    }

                    return new ResultInfo
                    {
                        ExtendedData = tokenInfo.ActionLink
                    };
                }
            }

            return SetAuthorize(licenseeId, language, tokenInfo);
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
                    var item = items.FirstOrDefault(i => i.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase));
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

        protected internal virtual IResultInfo SetAuthorize(int licenseeId, string languageCode, OneAuthenticationToken tokenInfo)
        {
            var licenseeNotExistedError = new ResultInfo
            {
                Code = int.Parse(OneAuthorizationErrorCode.LicenseeIsNotExisted),
                Message = ViewResources.LicenseeIsNotExisted
            };

            var licenseeInfo = _licenseeService.GetLicenseeById(licenseeId);
            if (licenseeInfo == null)
            {
                return licenseeNotExistedError;
            }

            var currencyOfLicensee = _licenseeService.GetCurrencies(licenseeId);

            if (currencyOfLicensee.Count <= 0)
            {
                return new ResultInfo
                {
                    Code = int.Parse(OneAuthorizationErrorCode.LicenseeNotOfferedCurrency),
                    Message = ViewResources.LicenseeNotOfferedCurrency
                };
            }

            var agentList = _licenseeService.GetCustByLicensee(licenseeId);
            if (agentList.Count <= 0)
            {
                return licenseeNotExistedError;
            }

            CreateSession(languageCode, tokenInfo, licenseeInfo, currencyOfLicensee, agentList);
            var authName = $"{tokenInfo.UserName}_{tokenInfo.VendorID}_{tokenInfo.SiteName}";
            _formsAuthenticationService.SetAuthCookie(authName, false);

            return new ResultInfo
            {
                ExtendedData = tokenInfo.ActionLink
            };
        }

        [CLSCompliant(false)]
        protected internal virtual ICache GetMemCacheService() => CacheService.CreateInstance(CacheType.MemoryCache);

        private static bool VerifySessionWithToken(UserSession userSession, LicenseeSession licenseeSession, OneAuthenticationToken tokenInfo, int licenseeId)
        {
            return userSession.UserId == tokenInfo.UserSEQ && userSession.UserName == tokenInfo.UserName
                && userSession.VendorID == tokenInfo.VendorID && userSession.VendorIdentityID == tokenInfo.UserID
                && licenseeSession.LicenseeId == licenseeId;
        }

        private static bool IsAuthenticated(HttpContextBase currentContext, UserSession userSession, LicenseeSession licenseeSession)
        {
            var identity = currentContext.User.Identity;
            var identityValidation = identity != null && identity.IsAuthenticated && !string.IsNullOrEmpty(identity.Name);

            var sessionValidation = userSession != null && licenseeSession != null && userSession.UserId > 0 &&
                !string.IsNullOrEmpty(userSession.VendorID) && !string.IsNullOrEmpty(userSession.VendorIdentityID)
                && userSession.IsInternal && licenseeSession.LicenseeId > 0 && userSession.ApplicationMode == AppMode.Parasitic;

            return identityValidation && sessionValidation;
        }

        private void CreateSession(
            string languageCode,
            OneAuthenticationToken tokenInfo,
            Licensee licenseeInfo,
            IList<Currency> currencyOfLicensee,
            IList<LicenseeCustomer> agentList)
        {
            var licSession = new LicenseeSession
            {
                LicenseeId = licenseeInfo.Id,
                LicenseeType = agentList
                                        .Where(s => !string.IsNullOrEmpty(s.Description))
                                        .Select(s => s.Description)
                                        .Distinct()
                                        .ToList(),
                CurrencyName = agentList.Select(s => s.CurrencyName).Distinct().ToList(),
                IsAllowedCustomerComm = licenseeInfo.IsAllowedCustomerComm,
                IsEnableBA = licenseeInfo.IsEnableBA,
                LicenseeName = licenseeInfo.Name,
                UserId = tokenInfo.UserSEQ,
                UserName = tokenInfo.UserName
            };

            var userSerssion = new UserSession
            {
                UserId = tokenInfo.UserSEQ,
                OnUserId = tokenInfo.UserSEQ,
                UserName = tokenInfo.UserName,
                IsAdmin = false,
                IsInternal = true,
                FunctionIdList = new List<int>(),
                PortalFunctionList = GetFunctionList(),
                Language = languageCode,
                CurrencyList = currencyOfLicensee.Distinct().ToDictionary(x => x.Name, x => x.CurrencyId),
                DefaultReportUrl = "BalanceReport",
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID,
                VendorIdentityID = tokenInfo.UserID,
            };

            _sessionService.CreateNewSession(userSerssion, licSession);
        }

        private IList<PortalSystemFunction> GetFunctionList()
        {
            const string FunctionFilePath = "~/App_Data/PortalSystemFunction.json";
            IList<PortalSystemFunction> result = null;
            using (var reader = _textReader != null ? _textReader : new StreamReader(HostingEnvironment.MapPath(FunctionFilePath)))
            {
                var temp = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(temp))
                {
                    result = JsonConvert.DeserializeObject<IList<PortalSystemFunction>>(temp);
                }
            }

            return result == null ? new List<PortalSystemFunction>() : result;
        }

        private IList<LicenseeSiteName> GetLiceseeSiteNameList(string filePath)
        {
            const string LicenseeSiteNameCacheKey = "OneAuthenticationService_LicenseeSiteNameCacheKey";
            const int CacheTimeoutMinute = 30;
            var cacheService = GetMemCacheService();

            var result = cacheService.Get<IList<LicenseeSiteName>>(LicenseeSiteNameCacheKey);

            if (result == null || !result.Any())
            {
                using (var reader = _textReader != null ? _textReader : new StreamReader(HostingEnvironment.MapPath(filePath)))
                {
                    var temp = reader.ReadToEnd();
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